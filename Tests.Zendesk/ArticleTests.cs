using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Zendesk.Actions;
using Apps.Zendesk.Models.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using ZendeskTests.Base;

namespace Tests.Zendesk
{
    [TestClass]
    public class ArticleTests:TestBase
    {

        [TestMethod]
        public async Task GetArticleIdFromHtmlReturnsValues()
        {
            var action = new ArticleActions(InvocationContext, FileManager);
            var fileName = "My very simple article.html";
            var input = new FileReference { Name = fileName };

            var input1 = new FileRequest{ File = input };
            var result = await action.GetArticleIdFromHtmlFile(input1);
            Console.WriteLine(result);

            Assert.IsNotNull(result);
        }
    }
}
