using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Support;

namespace Support
{
    public class CheckWord
    {
        public int Id { get; set; }
        public string EngPhrase { get; set; }
        public string RuPhrase { get; set; }
    }

    public class WordEng
    {
        public int Id { get; set; }
        public String link { get; set; }
        public String Word { get; set; }
        public String obj { get; set; }
        public String Level { get; set; }
        public String HtmlEng { get; set; }
    }

    public class Translate
    {
        public int Id { get; set; }
        public String WordEng { get; set; }
        public String HtmlRu { get; set; }

        public Translate (string wordEng, string htmlRu)
        {
            WordEng = wordEng;
            HtmlRu = htmlRu;
        }
    }

    public class Word
    {

        [Key]
        public int Id { get; set; }
        public WordEng WordEng { get; set; }
        public Translate WordRu { get; set; }
        public List<CheckWord> CheckWords { get; set; }

        public Word(){}

        public Word(WordEng wordEng,Translate translate=null, List<CheckWord> checkWords=null)
        {
            WordEng = wordEng;

            if (translate != null)
            {
                WordRu = translate;
                CheckWords=checkWords;
            }
            else
            {
                WriteInHtmlRu rus = new WriteInHtmlRu(wordEng.Word);
                WordRu = rus.Translate;
                CheckWords=rus.CheckWords;
            }
        }

    }
}
