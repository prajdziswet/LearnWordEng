using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Support
{
    public class WriteInHtmlEng
    {

        private WordEng wordEng { get; }

        public List<WordEng> WordEngAdd { get;}=new List<WordEng>();

        private String host = "https://" + new Uri("https://www.oxfordlearnersdictionaries.com/wordlists/oxford3000-5000").Host + "/";
        public WriteInHtmlEng(WordEng wordEng)
        {
            string link = wordEng.link;

                this.wordEng = wordEng;
                Console.WriteLine(wordEng.link);
                string responseEng= Response(link);
                if (!responseEng.IsNullOrEmpty())
                wordEng.HtmlEng = NormolaziEng(responseEng);

            for (int i = 0; i < WordEngAdd.Count; i++)
            {
                WordEng element = WordEngAdd[i];
                string response = Response(element.link);
                if (!response.IsNullOrEmpty())
                    element.HtmlEng = NormolaziEng(response);
            }

        }

        private string NormolaziEng(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            IEnumerable<HtmlNode> addition = htmlDoc.GetElementbyId("relatedentries")?
                .SelectSingleNode("//ul[@class='list-col']")?.ChildNodes.Where(x => x.Name == "li");

            HtmlNode htmlNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='entry']");
            //----------------------------------------------------------------------------
            //Comment in final version, It's file for if I want to make offline-programm
            //Download.Run(htmlNode);
            //----------------------------------------------------------------------------
            string returntext = null;
            if (htmlNode != null)
            {
                htmlNode.CheakAndRemove("//span[@class='dictlink-g']");
                htmlNode.CheakAndRemove("//div[@class='pron-link']");
                htmlNode.CheakAndRemove("//a[@class='responsive_display_inline_on_smartphone link-right']");

                htmlDoc.GetElementbyId("ring-links-box")?.Remove();

                if(addition != null)
                foreach (var element in addition)
                {
                    AddWord(element);
                }
                htmlNode.ReplaceLinkWithoutHost("https://www.oxfordlearnersdictionaries.com/");

                returntext = htmlNode.OuterHtml;
                returntext = Regex.Replace(returntext, "<div class=\"symbols.+?div>", "<span class=\"star-btn\" aria-hidden=\"true\">​</span>");
            }



            return returntext;
        }

        private void AddWord(HtmlNode element)
        {
            string word = wordEng.Word;
            string obj, href, text,response;
                var alink = element.ChildNodes.FirstOrDefault(x => x.Name == "a");
                href = FragmentLink(alink?.GetAttributeValue("href", null));
                if (href.IsNullOrEmpty()) return;
                obj = alink?.FirstChild.ChildNodes.FirstOrDefault(x => x.Name == "pos-g")?.InnerText;
                var ddd= alink?.FirstChild.ChildNodes.FirstOrDefault(x => x.Name == "#text")?.InnerText;
                text = alink?.FirstChild.ChildNodes.FirstOrDefault(x => x.Name == "#text")?.InnerText.Trim();
                if (!String.IsNullOrEmpty(text) && word == text && wordEng.obj != obj&&!String.IsNullOrEmpty(obj) &&
                    !String.IsNullOrEmpty(href) &&!WordEngAdd.Any(x => x.obj == obj && x.Word == word))
                {
                        WordEng wordclass = new WordEng();
                        wordclass.obj = obj;
                        wordclass.Word = word;
                        wordclass.link = href;
                        WordEngAdd.Add(wordclass);

                }
        }

        private int countDelay = 1;
        private string Response(string link)
        {
            if (!link.IsNullOrEmpty() && (host + link).ExitURL())
            {
                try
                {
                    string ret = (new Requst(host + link)).Response;
                    return ret;
                }
                catch (Exception e)
                {
                    Console.WriteLine(100 * countDelay);
                    Task.Delay(100 * countDelay++);
                    return Response(link);
                }

            }
            else return null;
        }

        private string FragmentLink(string link)
        {
            if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
            {
                return (new Uri(link)).LocalPath;
            }
            else return link;
        }

        public static string DeleteMarkImage(string wordEng)
        {
            string regex = "<span class=\"ox-enlarge-label\">enlarge image</span>";
            return wordEng.Replace(regex,"");
        }

    }
}
