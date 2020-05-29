using System;
using Hangfire;
using Hangfire.Storage.SQLite.Entities;
using Microsoft.AspNetCore.Mvc;

namespace testHangfire.Controllers
{
    public class ApiController : Controller
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ApiController(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }
        
        [Route("/api/test")]
        public IActionResult Index()
        {
            var testJob = new TestJobArg
            {
                Id = 123
            };

            var t1 = _backgroundJobClient.Enqueue<TestJob>(a => a.Execute(testJob, null));
            
            Console.WriteLine($" t1 - {t1}");
            
            // RecurringJob.AddOrUpdate<TestJob2>(a => a.Execute(testJob, null), Cron.Minutely);
            
            // testJob = new TestJobArg
            // {
            //     Id = 456
            // };
            //
            // var t2 = _backgroundJobClient.Schedule<TestJob>(a => a.Execute(testJob, null), TimeSpan.FromMinutes(1));
            //
            // Console.WriteLine($" t2 - {t2}");

            return Ok();
        }
    }
}