using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support
{
    public class TaskByNumber
    {
        private Action<WordEng> action;
        private int NumberStream;
        private WordEng[] wordsSource;
        private Task[] tasks;
        public TaskByNumber(Action<WordEng> action,WordEng[] words,int NumberStream)
        {
            this.action = action;
            this.NumberStream=NumberStream;
            wordsSource = words;
            tasks = new Task[words.Length];
        }

        private void CreateTask()
        {
            for (int i = 0; i < NumberStream; i++)
            {
                tasks[i] = new Task(() => action(arrayWordEng[i]));
            }
        }
        public void Start()
        {
            if (NumberStream > wordsSource.Length)
            {
                for (int i = 0; i < NumberStream; i++)
                {
                    tasks[i].Start();
                }
            }
            else
            {

            }
        }

        private void F1(WordEng[] arrayWordEng)
        {
            if (arrayWordEng != null && arrayWordEng.Length > 0)
            {
                Task[] tasks = new Task[arrayWordEng.Length];
                for (int i = 0; i < arrayWordEng.Length; i++)
                {
                    tasks[i] = new Task(() => action(arrayWordEng[i]));
                    tasks[i].Start();
                }
                Task.WaitAll(tasks);
            }
        }
        //private void CreateListAdditional(WordEng element)
        //{
        //    WriteInHtmlEng writeInHtml = new WriteInHtmlEng(element);
        //    var collection = writeInHtml.WordEngAdd;
        //    foreach (WordEng element2 in collection)
        //    {
        //        if (!additionaList.Any(x => x.link == element2.link) && !wordsEng.Any(x => x.link == element2.link))
        //            additionaList.Add(element2);
        //    }
        //}
    }
}
