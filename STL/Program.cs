using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2
{
	class Program
	{
		static void Main(string[] args)
		{
			Analyzer a = new Analyzer("Martin Eden.txt", "ignore.txt");
			Console.WriteLine("MaxFreeQ={0}", a.MaxFreeq);
			int n = 15;
			for (int i = 0; i < a.Pairs.Count && i < n; ++i)
				Console.WriteLine("{0}\t{1}", i + 1, a.Pairs[i]);
            Console.WriteLine(a.RunTime); 
			Console.ReadKey();
		}
	}
}