using System;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace testHangfire.Controllers
{
    [Queue("low")]
    public class TestJob 
    {
        [JobDisplayName("test job 123")]
        [AutomaticRetry(Attempts = 3)]
        public void Execute(TestJobArg arg, PerformContext context)
        {
            Console.WriteLine(arg.ToString());
            
            context.WriteLine($" context - {arg} ");
        }
    }
    
    [Queue("low")]
    public class TestJob2
    {
        [JobDisplayName("test job 123")]
        [AutomaticRetry(Attempts = 3)]
        public void Execute(TestJobArg arg, PerformContext context)
        {
            Console.WriteLine(arg.ToString());
            
            context.WriteLine($" context - {arg} ");
        }
    }
}