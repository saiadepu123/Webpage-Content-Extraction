using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebpageExtraction.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExtractWebpage : ControllerBase
    {
        [HttpGet]
        [Route("loadurl")]
        public dynamic LoadUrl(string url)
        {
            return ExtractContent(url);
        }

        dynamic ExtractContent(string url)
        {
            var sb = new StringBuilder();
            var images = new List<string>();
            // declare html document
            //var document = new HtmlWeb().Load("https://www.ibm.com/in-en");
            var document = new HtmlWeb().Load(url);
            var list = ExtractContent(document);
            var words = new List<string>();
            foreach (var sentense in list)
            {
                var wds = sentense.Split(' ').Select(wd => wd.Trim('\n','\t', ' ')).Where(w => w != "&amp;" && w!="");
                    words.AddRange(wds);
            }
            dynamic obj = new System.Dynamic.ExpandoObject();
            var t = document.DocumentNode.Descendants("img");
            var ImageURLs = document.DocumentNode.Descendants("img")
                                            .Select(e => e.GetAttributeValue("src", null))
                                            .Where(s => !String.IsNullOrEmpty(s));


            var top10Words = words.GroupBy(word => word)
                         .Select(group => new
                         {
                             Word = group.Key,
                             Count = group.Count()
                         })
                         .OrderByDescending(x => x.Count).Take(10);
            obj.images = ImageURLs;
            obj.wordscount = words.Count;
            obj.top10words = top10Words;
            return obj;
        }
        List<string> contentStrings = new List<string>();
        private List<string> ExtractContent(HtmlDocument currNode)
        {
            IEnumerable<HtmlNode> nodes = currNode.DocumentNode.Descendants().Where(n =>
               n.NodeType == HtmlNodeType.Text &&
               n.ParentNode.Name != "script" &&
               n.ParentNode.Name != "style");
            foreach (var node in nodes)
            {
                if (!node.HasChildNodes)
                {
                    var text = node.InnerText.Trim(' ', '\r', '\n');
                    if (!string.IsNullOrEmpty(text))
                    {
                        contentStrings.Add(text);
                    }
                }
            }
            return contentStrings;
        }
    }
}
