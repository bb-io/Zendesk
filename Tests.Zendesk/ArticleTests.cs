using Apps.Zendesk.Actions;
using Apps.Zendesk.Models.Blueprints;
using Apps.Zendesk.Models.Identifiers;
using Apps.Zendesk.Models.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Filters.Coders;
using Blackbird.Filters.Transformations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZendeskTests.Base;

namespace Tests.Zendesk
{
    [TestClass]
    public class ArticleTests:TestBase
    {
        public const string TestArticleId = "35514709559825";
        public const string TestSectionId = "11187138387601";
        public const string DefaultLocale = "en-us";
        public const string OtherLocale = "nl";
        public const string NewLocale = "fr";

        [TestMethod]
        public async Task Search_articles_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var result = await actions.SearchArticles(new SearchArticlesRequest { SectionIds = new List<string> { TestSectionId }, CreatedAfter = DateTime.Now.AddDays(-365) });
            Console.WriteLine(JsonConvert.SerializeObject(result.Articles, Formatting.Indented));

            Assert.IsTrue(result.Articles.Count() > 0);
        }

        [TestMethod]
        public async Task GetArticleIdFromHtml_returns_values()
        {
            var action = new ArticleActions(InvocationContext, FileManager);
            var fileName = "My very simple article.html";
            var input = new FileReference { Name = fileName };

            var input1 = new FileRequest{ File = input };
            var result = await action.GetArticleIdFromHtmlFile(input1);
            Console.WriteLine(result.ArticleId);

            Assert.AreEqual("29134116909073", result.ArticleId);
        }

        [TestMethod]
        public async Task GetArticle_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var result = await actions.GetArticle(new ArticleIdentifier { ContentId = TestArticleId });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result.ContentId);
        }

        [TestMethod]
        public async Task SearchArticleMissingTranlsations_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var result = await actions.GetArticleMissingTranslations(new ArticleIdentifier { ContentId = TestArticleId });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsTrue(result.Locales.Count() > 0);
        }

        [TestMethod]
        public async Task DownloadArticle_default_locale_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var result = await actions.GetArticleAsFile(new DownloadContentInput { ContentId = TestArticleId });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task DownloadArticle_has_Blacklake_requirements()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var result = await actions.GetArticleAsFile(new DownloadContentInput { ContentId = TestArticleId });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result.Content);

            var contentString = FileManager.ReadOutputAsString(result.Content);
            var codedContent = (new HtmlContentCoder()).Deserialize(contentString, result.Content.Name);

            Console.WriteLine(contentString);
            Assert.AreEqual(DefaultLocale, codedContent.Language);
            Assert.AreEqual(TestArticleId, codedContent.SystemReference.ContentId);
            Assert.AreEqual($"https://d3v-blackbird.zendesk.com/knowledge/editor/{TestArticleId}/en-us", codedContent.SystemReference.AdminUrl);
            Assert.AreEqual("Zendesk", codedContent.SystemReference.SystemName);
            Assert.AreEqual("https://www.zendesk.com", codedContent.SystemReference.SystemRef);
            Assert.IsNotNull(codedContent.SystemReference.ContentName);

            Console.WriteLine(codedContent.Provenance.Review.Person);
            Assert.IsNotNull(codedContent.Provenance.Review.Person);
            Assert.AreEqual("Zendesk", codedContent.Provenance.Review.Tool);
            Assert.AreEqual("https://www.zendesk.com", codedContent.Provenance.Review.ToolReference);

            Assert.IsTrue(codedContent.TextUnits.Any(x => x.Key is not null));
        }

        [TestMethod]
        public async Task DownloadArticle_other_locale_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var result = await actions.GetArticleAsFile(new DownloadContentInput { ContentId = TestArticleId, Locale = OtherLocale });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task UploadArticle_from_xliff_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);

            var fileReference = new FileReference { Name = "Multilingual AI Roundtable 2025 in Malmö!.html.xlf" };

            var result = await actions.TranslateArticleFromFile(new FileTranslationRequest { Locale = OtherLocale, Content = fileReference });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            var contentString = FileManager.ReadOutputAsString(result.Content);
            var transformation = Transformation.Parse(contentString, result.Content.Name);

            Assert.IsTrue(transformation.TargetSystemReference.SystemName == "Zendesk");
        }

        [TestMethod]
        public async Task UploadArticle_content_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var result = await actions.GetArticleAsFile(new DownloadContentInput { ContentId = TestArticleId });

            var uploadResult = await actions.TranslateArticleFromFile(new FileTranslationRequest { Locale = OtherLocale, Content = result.Content });
            Console.WriteLine(JsonConvert.SerializeObject(uploadResult, Formatting.Indented));

            Assert.IsTrue(result.Content.Name.Contains(uploadResult.Translation.Title));
        }

        [TestMethod]
        public async Task UploadArticle_content_for_new_locale_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            await actions.DeleteArticleTranslation(new ArticleIdentifier { ContentId = TestArticleId }, new LocaleIdentifier { Locale = NewLocale });
            var result = await actions.GetArticleAsFile(new DownloadContentInput { ContentId = TestArticleId });

            var uploadResult = await actions.TranslateArticleFromFile(new FileTranslationRequest { Locale = NewLocale, Content = result.Content });
            Console.WriteLine(JsonConvert.SerializeObject(uploadResult, Formatting.Indented));

            Assert.IsTrue(result.Content.Name.Contains(uploadResult.Translation.Title));
        }

        [TestMethod]
        public async Task UploadArticle_content_for_other_test_article()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var fileName = "Taking Flight How Blackbird is Changing the Language Space.html.xlf";
            var fileReference = new FileReference { Name = fileName };

            var uploadResult = await actions.TranslateArticleFromFile(new FileTranslationRequest { Locale = "nl", Content = fileReference });
            Console.WriteLine(JsonConvert.SerializeObject(uploadResult, Formatting.Indented));

            Assert.IsTrue(uploadResult.Translation.Id is not null);
        }

        [TestMethod]
        public async Task UploadArticle_content_for_other_test_article_de()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var fileName = "zendesk.html.xlf";
            var fileReference = new FileReference { Name = fileName };

            var uploadResult = await actions.TranslateArticleFromFile(new FileTranslationRequest { Locale = "de", Content = fileReference });
            Console.WriteLine(JsonConvert.SerializeObject(uploadResult, Formatting.Indented));

            Assert.IsTrue(uploadResult.Translation.Id is not null);
        }

        [TestMethod]
        public async Task Add_label_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            await actions.AddArticleLabel(new ArticleIdentifier { ContentId = TestArticleId }, new LocaleIdentifier { Locale = DefaultLocale }, "test");
            var result = await actions.GetArticle(new ArticleIdentifier { ContentId = TestArticleId });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result.Labels.Contains("test"));
        }

        [TestMethod]
        public async Task Delete_label_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var response = await actions.DeleteArticleLabel(new ArticleIdentifier { ContentId = "33549894133137" }, new LocaleIdentifier { Locale = "en-us" }, "19613072417041");
            var result = await actions.GetArticle(new ArticleIdentifier { ContentId = "33549894133137" });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));

            Assert.IsNotNull(result);
        }

    }
}
