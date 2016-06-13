using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LAB2
{
    /// <summary>
    /// ����������� ����
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
        List<string> book = new List<string>();									// ������ �����
        public HashSet<string> wordsToIgnore = new HashSet<string>();			// �����, ������� �� �����������
        public SortedDictionary<string, uint> freeq = new SortedDictionary<string, uint>(); // ����� ���� � �� �������������
        public List<WordFreeq> Pairs = new List<WordFreeq>();								// ���� ��������� ������������� �� ��������������

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
            // ��������� �������� �����
            System.IO.StreamReader r = new System.IO.StreamReader(fileName);
            // ������������
            // ������ ���� �� ������ � ������ ��������
            book.Clear();
            foreach (var word in r.ReadToEnd().Split(' ', ',', '\n', '\t', '\0', '\r'))
                book.Add(word.ToLower());
            r.Close();
            // ������� ��� ��������
            book.RemoveAll(x => string.IsNullOrEmpty(x) || wordsToIgnore.Contains(x));
            // ���������� ������� ��� ����������� ������ �����
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
            // ����� ���� �����
            Console.WriteLine(watch.ElapsedMilliseconds);
            string maxWord = MaxFreeq.Key;
            Console.WriteLine(watch.ElapsedMilliseconds);
            // ����������� ��� ����
            SortedDictionary<string, uint> pairs = new SortedDictionary<string, uint>();		// ���� ���� � �� ����������
            for (int i = 1; i < book.Count - 1; ++i)
            {
                // ��������� ��, ��� �� �������� ���� �����
                if (book[i] != maxWord && book[i + 1] != maxWord) continue;
                // ������� ����
                string p = book[i] + " " + book[i + 1];
                // ������� ���� � ���������� ���
                if (pairs.ContainsKey(p)) pairs[p]++;
                else pairs[p] = 1;

               /* p = book[i-1] + " " + book[i];
                if (pairs.ContainsKey(p)) pairs[p]++;
                else pairs[p] = 1;*/
            }
            // ������� ������ ������������� ���
            Pairs.Clear();
            foreach (var v in pairs)
                Pairs.Add(new WordFreeq(v.Key, v.Value));
            Console.WriteLine(watch.ElapsedMilliseconds);
            // ��������� �� �������� �������������
            Pairs.Sort((a, b) => (int)(b.Freeq - a.Freeq));
            watch.Stop();
            Console.WriteLine(GC.GetTotalMemory(true));
            Console.WriteLine(watch.ElapsedMilliseconds);
            RunTime = watch.ElapsedMilliseconds;
        }
    }
}