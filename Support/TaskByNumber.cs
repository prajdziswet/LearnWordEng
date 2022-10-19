using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support
{
    public class TaskByNumberAdditionalList
    {
        private int NumberStream;
        public List<WordEng> trySecond=new List<WordEng>();
        public List<WordEng> RezultList=new List<WordEng>();
        private WordEng[] wordsEng;
        private Task[] tasks;
        private bool first=true;
        private object locker=new object();
        private int count=0;
        private int index=0;
        private int CountIndex;
        public TaskByNumberAdditionalList(List<WordEng> wordsEngList, int countIndex, int NumberStream=4)
        {
            CountIndex=countIndex;
            if (wordsEngList == null || wordsEngList.Count == 0) throw new ArgumentNullException();
            wordsEng = wordsEngList.ToArray();
            if (NumberStream <= 1) NumberStream = 2;
            this.NumberStream=NumberStream;
            CreateList();
            Wait();
        }

        private void CreateTask()
        {
            WordEng[] words;
            if (first)
            {
                words=wordsEng;
            }
            else
            {
                words = trySecond.ToArray();
            }

                tasks = new Task[words.Length];
                CreateTask(words);
        }

        private void CreateTask(WordEng[] words)
        {
            if (words.Length <= NumberStream)
                for (int i = 0; i < NumberStream && i < words.Length; i++)
                {
                    WordEng wordEng = words[i];
                    tasks[i] = new Task(() => CreateListAdditional(wordEng));
                }
            else
            {
                    for (int i1 = 0; i1 < words.Length; i1++)
                    {
                        WordEng wordEng = words[i1];
                    if (i1 < NumberStream)
                        tasks[i1] = new Task(() => CreateListAdditional(wordEng));
                    else
                    {
                        tasks[i1] = tasks[i1 - NumberStream].ContinueWith(task => CreateListAdditional(wordEng));
                    }
                    }

            }
        }

        private void Wait()
        {
            Task.WaitAll(tasks);
        }

        public void CreateList()
        {
            CreateTask();
            count = wordsEng.Length;
            Start((NumberStream>wordsEng.Length)? wordsEng.Length:NumberStream);
            Wait();
            //try Second and try not Stream
            if (trySecond.Count>0)
            {
                first = false;
                index = 0; count = trySecond.Count;
                CreateTask();
                int number = (NumberStream > trySecond.Count) ? trySecond.Count : NumberStream;
                trySecond = new List<WordEng>();
                Start(number);
                Wait();
                if (trySecond.Count > 0)
                {
                    index = 0;count = trySecond.Count;
                    WordEng[] lastNotStream= trySecond.ToArray();
                    trySecond = new List<WordEng>();
                    foreach (WordEng lastWordEng in lastNotStream)
                    {
                        CreateListAdditional(lastWordEng);
                    }
                }
             }
        }

        private void Start(int Lengt)
        {
                for (int i = 0; i < Lengt; i++)
                {
                    tasks[i].Start();
                }
        }

        private void CreateListAdditional(WordEng element)
        {
            try
            {
                WriteInHtmlEng writeInHtml = new WriteInHtmlEng(element);
                var collection = writeInHtml.WordEngAdd;
                foreach (WordEng element2 in collection)
                {
                    lock(locker)
                        if (!RezultList.Any(x => x.link == element2.link) &&
                            !wordsEng.Any(x => x.link == element2.link))
                        {
                            ////////////////////////////////////////
                            element2.Id = ++CountIndex;
                            RezultList.Add(element2);
                        }
                }
            }
            catch (Exception)
            {
                lock (locker) trySecond.Add(element);
                throw;
            }
            finally
            {
                lock (locker)
                {
                    Console.Clear();
                    index++;
                    Console.WriteLine(index + "//" + count);
                }
            }
        }
    }
}
