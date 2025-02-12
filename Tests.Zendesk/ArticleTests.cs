using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Zendesk.Actions;
using Apps.Zendesk.Models.Identifiers;
using Apps.Zendesk.Models.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using ZendeskTests.Base;

namespace Tests.Zendesk
{
    [TestClass]
    public class ArticleTests:TestBase
    {
        public const string TestArticleId = "32193792125201";
        public const string TestSectionId = "32193681530257";
        public const string DefaultLocale = "en-us";
        public const string OtherLocale = "nl";
        public const string NewLocale = "de";

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
            var result = await actions.GetArticle(new ArticleIdentifier { Id = TestArticleId });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result.Id);
        }

        [TestMethod]
        public async Task SearchArticleMissingTranlsations_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var result = await actions.GetArticleMissingTranslations(new ArticleIdentifier { Id = TestArticleId });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsTrue(result.Locales.Count() > 0);
        }

        [TestMethod]
        public async Task DownloadArticle_default_locale_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var result = await actions.GetArticleAsFile(new ArticleIdentifier { Id = TestArticleId }, new LocaleIdentifier { Locale = DefaultLocale });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result.File);
        }

        [TestMethod]
        public async Task DownloadArticle_other_locale_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var result = await actions.GetArticleAsFile(new ArticleIdentifier { Id = TestArticleId }, new LocaleIdentifier { Locale = OtherLocale });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result.File);
        }

        [TestMethod]
        public async Task UploadArticle_content_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            var result = await actions.GetArticleAsFile(new ArticleIdentifier { Id = TestArticleId }, new LocaleIdentifier { Locale = DefaultLocale });

            var uploadResult = await actions.TranslateArticleFromFile(new ArticleOptimalIdentifier { }, new FileTranslationRequest { Locale = OtherLocale, File = result.File });
            Console.WriteLine(JsonConvert.SerializeObject(uploadResult, Formatting.Indented));

            Assert.IsTrue(result.File.Name.Contains(uploadResult.Title));
        }

        [TestMethod]
        public async Task UploadArticle_content_for_new_locale_works()
        {
            var actions = new ArticleActions(InvocationContext, FileManager);
            await actions.DeleteArticleTranslation(new ArticleIdentifier { Id = TestArticleId }, new LocaleIdentifier { Locale = NewLocale });
            var result = await actions.GetArticleAsFile(new ArticleIdentifier { Id = TestArticleId }, new LocaleIdentifier { Locale = DefaultLocale });

            var uploadResult = await actions.TranslateArticleFromFile(new ArticleOptimalIdentifier { }, new FileTranslationRequest { Locale = NewLocale, File = result.File });
            Console.WriteLine(JsonConvert.SerializeObject(uploadResult, Formatting.Indented));

            Assert.IsTrue(result.File.Name.Contains(uploadResult.Title));
        }

    }
}
