using System;
using System.Threading;
using Tharga.Quilt4Net;

namespace Tharga.ConsoleSample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var rng = new Random();

            var cnt = Counter.Start("Main");

            Configuration.ClientToken = "5H4ZP7EQJWPM2539782YACX6V57J62DF";
            Configuration.Target.Location = "http://localhost:50154/";
            Counter.Checkpoint(cnt, "Config");

            Session.Register();
            cnt.Step("Register Session");

            Issue.Register("ABC", Issue.MessageIssueLevel.Warning);
            cnt.Step("Register Issue");

            for (var i = 0; i < 10; i++)
            {
                var loop = Counter.Start("Lap " + i);

                for (var j = 0; j < rng.Next(3, 5); j++)
                {
                    var inner = new CounterInfo("Inner " + j);
                    Thread.Sleep(rng.Next(100, 500));
                    loop.Step(inner);
                }

                cnt.Step(loop);
            }

            Session.End();

            Counter.Register(cnt);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}