using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        
        static void Main()
        {
            ThreadStart threadDelegate = new ThreadStart(Work.DoWork);
            Thread newThread = new Thread(threadDelegate);
            newThread.Start();

            Work w = new Work();
            w.Data = 42;
            threadDelegate = new ThreadStart(w.DoMoreWork);
            newThread = new Thread(threadDelegate);
            newThread.Start();
            Console.ReadLine();

        }
        
    }
    class Work
    {
        public int Data;
        public static void DoWork()
        {
            Console.WriteLine("Static Thread Procedure. ");

        }

        public void DoMoreWork()
        {
            Console.WriteLine("instance Thread Procedure. Data={0} ", Data);
        }
    }
}
