using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Support;

namespace Support
{
    public class CheckWord
    {
        [Key]
        public int Id { get; set; }
        public string EngPhrase { get; set; }
        public string RuPhrase { get; set; }
        public int? TranslateId { get; set; }
    }

    public class WordEng
    {
        [Key]
        public int Id { get; set; }
        public String link { get; set; }
        public String Word { get; set; }
        public String obj { get; set; }
        public String Level { get; set; }
        public String HtmlEng { get; set; }
        public int? WordId { get; set; }
    }

    public class Translate
    {
        [Key]
        public int Id { get; set; }
        public String WordEng { get; set; }
        public String HtmlRu { get; set; }
        public List<CheckWord> CheckWords { get; set; }
        public List<Word> Words { get; set; }
        public Translate()
        { }

        public Translate(string wordEng, string htmlRu)
        {
            //Id = ++Count;
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

        public int TranslateId { get; set; }

        public Word()
        {
        }


        public Word(WordEng wordEng, Translate translate = null, List<CheckWord> checkWords = null)
        {
            WordEng = wordEng;
            Id = wordEng.Id;

            if (translate != null)
            {
                WordRu = translate;
                WordRu.CheckWords = checkWords;
            }
            else
            {
                WriteInHtmlRu rus = new WriteInHtmlRu(wordEng.Word);
                WordRu = rus.Translate;
                WordRu.CheckWords = rus.CheckWords;
            }
        }

    }
}
