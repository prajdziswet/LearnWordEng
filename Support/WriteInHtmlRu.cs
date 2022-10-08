using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;

namespace Support;

public class WriteInHtmlRu
{
    public Translate Translate
    {
        get => new Translate(wordEng,HtmlRu);
    }
    private string wordEng { get; }
    private string HtmlRu { get; }
    public List<CheckWord> CheckWords { get;}= new List<CheckWord>();

    public WriteInHtmlRu (string word)
    {
        wordEng=word;
        string response = getResponse();
        if (response != null)
        {
            HtmlRu = NormolaziRU(response);
        }
    }

    private int countDelay = 1;
    private string getResponse()
    {
        if (!wordEng.IsNullOrEmpty() && ("https://wooordhunt.ru/word/" + wordEng).ExitURL())
        {
            try
            {
                string ret = (new Requst("https://wooordhunt.ru/word/" + wordEng)).Response;
                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine(100 * countDelay);
                Task.Delay(100 * countDelay++);
                return getResponse();
            }
        }
        else return null;
    }

    private string NormolaziRU(string html)
    {
        HtmlDocument htmlDoc= new HtmlDocument();
        htmlDoc.LoadHtml(html);
        HtmlNode htmlNode = htmlDoc.GetElementbyId("wd");
        htmlNode.CheakAndRemove("//*[@id='ppt']");
        htmlNode.CheakAndRemove("//div[@id='block_action_icons']");
        htmlNode.CheakAndRemove("//div[@class='trans_sound']");
        htmlNode.CheakAndRemove("//*[@id=\"content_switcher_block\"]");
        htmlNode.CheakAndRemove("//*[@id=\"word_rank_box\"]");
        htmlNode.CheakAndRemove("//*[@id=\"add_to_dict\"]");
        htmlNode.CheakAndRemove("//*[@id=\"other_dict\"]");
        htmlNode.CheakAndRemove("//*[@id=\"personal_ex_block\"]");
        
        htmlNode.ReplaceLinkWithoutHost("https://wooordhunt.ru/");
        CreateList(DeleteWordNeedTranslate(htmlNode.OuterHtml));
        return DeleteWordNeedTranslate(htmlNode.OuterHtml);
    }

    private void CreateList(string html)
    {
        CheckWords.AddRange(ReturnList(html));
        //AdditionListRu();
    }


    //I don't why don't work. Maybe, change on site..

    //private void SetlistRu(HtmlNode htmlNode)
    //{
    //    Console.WriteLine(htmlNode.OuterHtml);
    //    HtmlNodeCollection collOriginal= htmlNode.SelectNodes("//p[@class='ex_o']");
    //    HtmlNodeCollection collTranslate = htmlNode.SelectNodes("//p[@class='ex_t human']");

    //    if (collOriginal != null && collTranslate != null)
    //    {
    //        string ru, eng;
    //        for (int i = 0; i < collOriginal.Count && i < collTranslate.Count; i++)
    //        {
    //            eng = collOriginal[i].InnerText;
    //            ru = collTranslate[i].InnerText;
    //            if (!eng.IsNullOrEmpty() && !ru.IsNullOrEmpty())
    //            {
    //                CheckWord newCheckWord = new CheckWord();
    //                newCheckWord.RuPhrase = ru;
    //                newCheckWord.EngPhrase = eng;
    //                CheckWords.Add(newCheckWord);
    //            }
    //        }
    //    }
    //}

    //    ..Forbiten

    //    private void AdditionListRu()
    //    {
    //        string adress = $"https://context.reverso.net/translation/english-russian/{wordEng}";
    //if (adress.ExitURL()) {
    //        Requst requst = new Requst($"https://context.reverso.net/translation/english-russian/{wordEng}");
    //        string response = requst.Response;
    //        HtmlDocument htmlDoc = new HtmlDocument();
    //        htmlDoc.LoadHtml(response);
    //        HtmlNode htmlNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"examples-content\"]");
    //        HtmlNodeCollection collAll = htmlNode.SelectNodes("//div[contains(@class,'example')]");
    //        if (collAll!=null)
    //        foreach (HtmlNode Block in collAll)
    //        {
    //            string eng = Block.SelectSingleNode("//div[@class='src ltr']").InnerText;
    //            string ru = Block.SelectSingleNode("//div[@class='trg ltr']").InnerText;
    //            if (!eng.IsNullOrEmpty() && !ru.IsNullOrEmpty())
    //            {
    //                CheckWord newCheckWord = new CheckWord();
    //                newCheckWord.RuPhrase = ru;
    //                newCheckWord.EngPhrase = eng;
    //                CheckWords.Add(newCheckWord);
    //            }
    //        }


    //        }
    //    }

    public static string DeleteWordNeedTranslate(string wordRu)
    {
        string regex = "<h3 style=\"margin-bottom:10px\">Примеры, ожидающие перевода.+?(?=<h3)";
        return Regex.Replace(wordRu, regex,"");
    }

    public static List<CheckWord> ReturnList(string html)
    {
        List<CheckWord> retList =new List<CheckWord>();

        var divCollection = Regex.Matches(html, @"<div class=""ex.*?"".*?>(.*?)<\/div>");
        bool block = false;
        if (divCollection == null)
        {
            divCollection= Regex.Matches(html, @"<div class=""block phra.*?>(.*?)<\/div>");
            block = true;
        }


        if (divCollection!=null)
        foreach (Match div in divCollection)
        {
            if (div.Groups.Count >= 1)
            {
                string str=div.Groups[1].Value;
                var collection = Regex.Matches(str, @"(.*?)<i>(.*?)<\/i>");
                if (collection!=null)
                    foreach (Match element in collection)
                    {
                            if (element.Groups.Count>=2)
                            {
                                string eng;
                                    if (!block)
                                    eng= (element.Groups[1].Value.IndexOf(">") >= 0) ? element.Groups[1].Value.Remove(0, element.Groups[1].Value.IndexOf(">")) : element.Groups[1].Value;
                                    else eng= element.Groups[1].Value;
                                string ru = element.Groups[2].Value;
                                CheckWord checkWord=new CheckWord(){EngPhrase = eng,RuPhrase = ru};
                                retList.Add(checkWord);
                            }
                    }
            }
        }
        return retList;
    }

}