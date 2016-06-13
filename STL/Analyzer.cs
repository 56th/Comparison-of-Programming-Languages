using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LAB2
{
    /// <summary>
    /// анализирует файл
    /// </summary>
    public class Analyzer
    {
        public long RunTime;
        public class WordFreeq
        {
            public readonly string Word;
            public readonly uint Freeq;

            public WordFreeq(string Word, uint Freeq)
            {
                this.Word = Word;
                this.Freeq = Freeq;
            }
            public override string ToString()
            {
                return "[" + Word + ", " + Freeq + "]";
            }
        }
        List<string> book = new List<string>();									// строки книги
        public HashSet<string> wordsToIgnore = new HashSet<string>();			// слова, которые не учитываются
        public SortedDictionary<string, uint> freeq = new SortedDictionary<string, uint>(); // набор слов и их встречаемость
        public List<WordFreeq> Pairs = new List<WordFreeq>();								// пары элементов сортированные по встречаемовсти

        public KeyValuePair<string, uint> MaxFreeq
        {
            get
            {
                KeyValuePair<string, uint> max = new KeyValuePair<string, uint>("", 0);
                foreach (var v in freeq)
                {
                    if (max.Value == 0 || v.Value > max.Value) max = v;
                }
                return max;
            }
        }

        public readonly string FileName;
        public Analyzer(string fileName, string ignoreFile)
        {
            LoadExcludeWords(ignoreFile);
            this.FileName = fileName;
            foreach (var v in wordsToIgnore)
                this.wordsToIgnore.Add(v.ToLower());
            ReadBook(fileName);
        }
        void LoadExcludeWords(string fileName)
        {
            wordsToIgnore.Clear();
            System.IO.StreamReader r = new System.IO.StreamReader(fileName);
            foreach (var word in r.ReadToEnd().Split(' ', ',', '\n', '\t', '\0', '\r'))
                wordsToIgnore.Add(word.ToLower());
            r.Close();
        }

        void ReadBook(string fileName)
        {
            // открываем файловый поток
            System.IO.StreamReader r = new System.IO.StreamReader(fileName);
            // ограничитель
            // читаем файл по словам в нижнем регистре
            book.Clear();
            foreach (var word in r.ReadToEnd().Split(' ', ',', '\n', '\t', '\0', '\r'))
                book.Add(word.ToLower());
            r.Close();
            // удаляем все ненужное
            book.RemoveAll(x => string.IsNullOrEmpty(x) || wordsToIgnore.Contains(x));
            // определяем сколько раз встретилось каждое слово
            freeq.Clear();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.WriteLine(GC.GetTotalMemory(true));
            
            unchecked
            {
                for (int i = 0; i < book.Count; i++)
                {
                    if (freeq.ContainsKey(book[i])) freeq[book[i]]++;
                    else freeq[book[i]] = 1;
                }
            }
            // берем макс слово
            Console.WriteLine(watch.ElapsedMilliseconds);
            string maxWord = MaxFreeq.Key;
            Console.WriteLine(watch.ElapsedMilliseconds);
            // вытаскиваем все пары
            SortedDictionary<string, uint> pairs = new SortedDictionary<string, uint>();		// пары слов и их количества
            for (int i = 1; i < book.Count - 1; ++i)
            {
                // отсеиваем то, что не содержит макс слова
                if (book[i] != maxWord && book[i + 1] != maxWord) continue;
                // создаем пару
                string p = book[i] + " " + book[i + 1];
                // вставка пары и количества пар
                if (pairs.ContainsKey(p)) pairs[p]++;
                else pairs[p] = 1;

               /* p = book[i-1] + " " + book[i];
                if (pairs.ContainsKey(p)) pairs[p]++;
                else pairs[p] = 1;*/
            }
            // создаем список сортированных пар
            Pairs.Clear();
            foreach (var v in pairs)
                Pairs.Add(new WordFreeq(v.Key, v.Value));
            Console.WriteLine(watch.ElapsedMilliseconds);
            // сортируем по убыванию встречаемости
            Pairs.Sort((a, b) => (int)(b.Freeq - a.Freeq));
            watch.Stop();
            Console.WriteLine(GC.GetTotalMemory(true));
            Console.WriteLine(watch.ElapsedMilliseconds);
            RunTime = watch.ElapsedMilliseconds;
        }
    }
}