using System;
using System.Threading;

namespace KeepMeAwake
{
    class Program
    {
        static ManualResetEvent mStopFlag = new ManualResetEvent(false);
        static int mInterval = 30 * 1000; //30s

        static void Main(string[] args)
        {
            Log("Keep me awake started.");

            Thread t = new Thread(new ThreadStart(Worker));
            t.Name = "AWAKE_PROC";
            t.Start();

            Log("Press any key to exit.");
            Console.ReadKey();

            Log("Received terminate signal.");
            mStopFlag.Set();
        }

        private static void Worker()
        {
            for (; ; )
            {
                var stat = NativeMethods.SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED);
                Log("Sent ES_SYSTEM_REQUIRED.");

                if (mStopFlag.WaitOne(mInterval))
                    break;
            }
        }

        static void Log(string format, params object?[] args)
        {
            Console.Write("{0}> ", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            Console.WriteLine(format, args);
        }
    }
}
