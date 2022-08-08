using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            AdditionListRu();
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
        SetlistRu(htmlNode);
        htmlNode.ReplaceLinkWithoutHost("https://wooordhunt.ru/");
        return htmlNode.OuterHtml;
    }

    private void SetlistRu(HtmlNode htmlNode)
    {
        HtmlNodeCollection collOriginal= htmlNode.SelectNodes("//p[@class='ex_o']");
        HtmlNodeCollection collTranslate = htmlNode.SelectNodes("//p[@class='ex_t human']");

        if (collOriginal != null && collTranslate != null)
        {
            string ru, eng;
            for (int i = 0; i < collOriginal.Count && i < collTranslate.Count; i++)
            {
                eng = collOriginal[i].InnerText;
                ru = collTranslate[i].InnerText;
                if (eng.IsNullOrEmpty() && ru.IsNullOrEmpty())
                {
                    CheckWord newCheckWord = new CheckWord();
                    newCheckWord.RuPhrase = ru;
                    newCheckWord.EngPhrase = eng;
                    CheckWords.Add(newCheckWord);
                }
            }
        }
    }

    private void AdditionListRu()
    {
        string adress = $"https://context.reverso.net/translation/english-russian/{wordEng}";
if ($"https://context.reverso.net/translation/english-russian/{wordEng}".ExitURL()) {
        Requst requst = new Requst($"https://context.reverso.net/translation/english-russian/{wordEng}");
        string response = requst.Response;
        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(response);
        HtmlNode htmlNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"examples-content\"]");
        HtmlNodeCollection collAll = htmlNode.SelectNodes("//div[contains(@class,'example')]");
        if (collAll!=null)
        foreach (HtmlNode Block in collAll)
        {
            string eng = Block.SelectSingleNode("//div[@class='src ltr']").InnerText;
            string ru = Block.SelectSingleNode("//div[@class='trg ltr']").InnerText;
            if (eng.IsNullOrEmpty() && ru.IsNullOrEmpty())
            {
                CheckWord newCheckWord = new CheckWord();
                newCheckWord.RuPhrase = ru;
                newCheckWord.EngPhrase = eng;
                CheckWords.Add(newCheckWord);
            }
        }


        }
    }

    public static string DeleteWordNeedTranslate(string wordRu)
    {
        string regex = "<h3 style=\"margin-bottom:10px\">Примеры, ожидающие перевода.+?(?=<h3)";
        return Regex.Replace(wordRu, regex,"");
    }

}