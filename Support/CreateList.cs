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
using System.Threading;
using System.Xml.Linq;

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

        private static int IndexEng=0;

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
            SetIndexWordRU();
            words.ShuffleInPlace();
            PrepareDB();
        }

        private void PrepareDB()
        {
            List<string> levels = new List<string>() { "a1", "a2", "b1", "b2", "c1" };
            Console.WriteLine(words.Count);
            List<Word> wordstemp = new List<Word>();
            foreach (string level in levels)
            {
                List<Word> wordslevel= words.Where(x=>x.WordEng.Level==level).ToList();
                Console.WriteLine(wordslevel.Count);
                wordstemp.AddRange(wordslevel);
                List<Word> AdditionWords = words.Where(x=>wordslevel.Any(y=>x.WordEng.Level==null&&y.WordEng.Word==x.WordEng.Word)).ToList();
                Console.WriteLine(AdditionWords.Count);
                wordstemp.AddRange(AdditionWords);
            }
            Console.WriteLine(wordstemp.Count);
            words = wordstemp;
        }

        private void SetIndexWordRU()
        {
            int i = 0;
            foreach (Word element in words)
            {
                element.WordRu.Id = ++i;
                element.WordRu.WordId = element.Id;
            }
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
                if (wordsEng!=null&&!wordsEng.Any(x=>x.link==word.link))
                {
                    ///////////////////////////////
                    word.Id = ++IndexEng;
                    wordsEng.Add(word);
                }
                if (wordsEng.Count==2) break;
            }
            Console.WriteLine(wordsEng.Count);
        }

        private void CreateListAdditional()
        {
            TaskByNumberAdditionalList taskBy = new TaskByNumberAdditionalList(wordsEng, wordsEng.Count,8);
            List<WordEng> additionaList1=taskBy.RezultList;
            if (additionaList1!=null&& additionaList1.Count>0) additionaList= additionaList1;
        }

        private void CreateListAdditionalOld()
        {
            foreach (WordEng elemEng in wordsEng)
            {
                try
                {
                    WriteInHtmlEng writeInHtml = new WriteInHtmlEng(elemEng);
                    var collection = writeInHtml.WordEngAdd;
                    foreach (WordEng element2 in collection)
                    {
                            if (!additionaList.Any(x => x.link == element2.link) &&
                                !wordsEng.Any(x => x.link == element2.link))
                            {
                                ////////////////////////////////////////
                                element2.Id = ++IndexEng;
                                additionaList.Add(element2);
                            }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error"+elemEng);
                    throw;
                }
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
                newWord = new Word(element, word.WordRu, word.CheckWords);
                ///////////////////////////////
                words.Add(newWord);
                element.WordId =words.Count;
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
