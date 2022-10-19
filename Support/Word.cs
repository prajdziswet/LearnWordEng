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
        public int? WordId { get; set; }
        //private static int Count = 0;
        //public CheckWord()
        //{
        //    Id = ++Count;
        //}
    }

    public class WordEng
    {
        [Key]
        public int Id { get; set; }
        public String? link { get; set; }
        public String? Word { get; set; }
        public String? obj { get; set; }
        public String? Level { get; set; }
        public String? HtmlEng { get; set; }
        public int WordId { get; set; }
        //private static int Count=0;
        //public WordEng()
        //{
        //    Id = ++Count;
        //}
    }

    public class Translate
    {
        [Key]
        public int Id { get; set; } 
        public String WordEng { get; set; }
        public String HtmlRu { get; set; }
        public int? WordId { get; set; }
        //private static int Count = 0;
        public Translate()
        {}

        public Translate (string wordEng, string htmlRu)
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
        public List<CheckWord> CheckWords { get; set; }

        //private static int Count = 0;

        public Word()
        {
            //Id = ++Count; 
        }

        //public Word(int id, WordEng wordEng, Translate wordRu, List<CheckWord> checkWords)
        //{
        //    Id = id;
        //    WordEng = wordEng;
        //    WordRu = wordRu;
        //    CheckWords = checkWords;
        //}

        public Word(WordEng wordEng,Translate translate=null, List<CheckWord> checkWords=null)
        {
            WordEng = wordEng;
            Id = wordEng.Id;

            if (translate != null)
            {
                WordRu = translate;
                CheckWords=checkWords;
            }
            else
            {
                WriteInHtmlRu rus = new WriteInHtmlRu(wordEng.Word,Id);
                WordRu = rus.Translate;
                CheckWords=rus.CheckWords;
            }
        }

    }
}
