using System;
using System.Threading;

namespace CalculatorAutomation
{
    public class Program
    {
        private BaseProcess _process; 

        public Program(BaseProcess process)
        {
            this._process = process;
        }

        /*public void Run()
        {
            Console.WriteLine("Opening program");
            this._process.Run();

            // Allow 2 seconds to view changes in the Console Log
            Thread.Sleep(2000);

            Console.WriteLine("Closing program");
            this._process.Dispose();

            // Allow 1 seconds to view changes in the Console Log
            Thread.Sleep(1000);
        }*/

        public 

        public static void Main(string[] args)
        {
            Program program = new Program(new CalculatorProcess());
            program.Run();
        }
    }
}
