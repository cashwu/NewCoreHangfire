using System;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace testHangfire.Controllers
{
    [Queue("low")]
    public class TestJob 
    {
        private readonly ConstHelper _constHelper;

        public TestJob(ConstHelper constHelper)
        {
            _constHelper = constHelper;
        }
        
        [JobDisplayName("test job 123")]
        [AutomaticRetry(Attempts = 3)]
        public void Execute(TestJobArg arg, PerformContext context)
        {
            Console.WriteLine(arg.ToString());
            
            context.WriteLine($" context - {arg} , helper - {_constHelper.Test()}");
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