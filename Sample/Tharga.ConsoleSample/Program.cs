using System;
using Tharga.Quilt4Net;

namespace Tharga.ConsoleSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cnt = Counter.Start("Main");

            Configuration.ClientToken = "5H4ZP7EQJWPM2539782YACX6V57J62DF";
            Configuration.Target.Location = "http://localhost:50154/";

            Session.Register();

            Issue.Register("ABC", Issue.MessageIssueLevel.Warning);

            Session.End();

            Counter.Register(cnt);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}