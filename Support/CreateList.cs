using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;


namespace Support
{
    public class CreateList
    {
        private HtmlDocument htmlDoc = new HtmlDocument();

        private List<WordEng> wordsEng = new List<WordEng>();
        private List<WordEng> additionaList=new List<WordEng>();

        private List<Word> words = new List<Word>();
        public IList<Word> Words
        { get { return words.AsReadOnly(); } }

        //private String adress = "https://www.oxfordlearnersdictionaries.com/wordlists/oxford3000-5000";

        public CreateList()
        {
            CreateListEngWords();
            Console.WriteLine("CreatedListEngWords");
            Console.WriteLine();
            CreateListAdditional();
            Console.WriteLine("CreatedListAdditional");
            wordsEng.AddRange(additionaList);
            Console.WriteLine($"Common count Words{wordsEng.Count}");
            Console.WriteLine();
            CreateListWords();
            Console.WriteLine("CreatedListWords, end");
        }

        private void CreateListEngWords()
        {
            //CreateGetRequest();getRequst.Response
            string getResponse = tempreturnOxford();
            htmlDoc.LoadHtml(getResponse);
            HtmlNodeCollection listTag = htmlDoc.DocumentNode.SelectNodes("//li[@data-hw]");
            foreach (HtmlNode element in listTag)
            {
                WordEng word = new WordEng();
                word.Word = element.GetAttributeValue("data-hw", "");
                word.Level = element.GetAttributeValue("data-ox5000", null) ??
                             element.GetAttributeValue("data-ox3000", null);
                word.obj = element.ChildNodes.FirstOrDefault(x => x.Name == "span")?.InnerHtml;
                word.link = FragmentLink(element.ChildNodes.FirstOrDefault(x => x.Name == "a")?.GetAttributeValue("href", null));
                if (!wordsEng.Any(x=>x.link==word.link))
                {
                    wordsEng.Add(word);
                }
                //if (wordsEng.Count==2) break;
            }
            Console.WriteLine(wordsEng.Count);
        }

        private void CreateListAdditional()
        {
            int index = 1,count= wordsEng.Count;
            foreach (WordEng element in wordsEng)
            {
                WriteInHtmlEng writeInHtml = new WriteInHtmlEng(element);
                var collection = writeInHtml.WordEngAdd;
                foreach (WordEng element2 in collection)
                {
                    if (!additionaList.Any(x=>x.link==element2.link)&& !wordsEng.Any(x => x.link == element2.link))
                        additionaList.Add(element2);
                }
                Console.WriteLine($"\r{index++}/{count}");
            }
        }

        private void CreateListWords()
        {
            int index = 1, count = wordsEng.Count;
            foreach (WordEng element in wordsEng)
            {
                Word newWord;
                Word word = Words.FirstOrDefault(x => x.WordEng.Word == element.Word && x.WordRu != null);
                if (word==null) newWord = new Word(element);
                else
                newWord = new Word(element,word.WordRu,word.CheckWords);
                words.Add(newWord);
                Console.Write($"\r{index++}/{count}");
            }
        }

        string tempreturnOxford()
        {
            using (StreamReader stream=new StreamReader($"{Directory.GetCurrentDirectory()}\\Oxsford.txt"))
            {
                return stream.ReadToEnd();
            }
        }

        private string FragmentLink(string link)
        {
            if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
            {
                return (new Uri(link)).LocalPath;
            }
            else return link;
        }
    }

}
