using System;

namespace CalculatorAutomation
{
    public interface IRunnable
    {
        void Run();
    }

    public abstract class BaseProcess : IRunnable, IDisposable
    {
        public abstract void Run();
        public abstract void Dispose();
    }
}
