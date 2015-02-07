using System;
using Tharga.Quilt4Net;

namespace Tharga.ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Quilt4Net.Configuration.ClientToken = "5H4ZP7EQJWPM2539782YACX6V57J62DF";
            Quilt4Net.Configuration.Target.Location = "http://localhost:50154/";

            Quilt4Net.Session.Register();

            Quilt4Net.Issue.Register("ABC", Issue.MessageIssueLevel.Warning);

            Quilt4Net.Session.End();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
