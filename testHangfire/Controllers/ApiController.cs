using System;
using Hangfire;
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

        [Route("/api/test2")]
        public IActionResult Index2()
        {
            var testJob = new TestJobArg
            {
                Id = 123
            };

            var customTimeZone = TimeZoneInfo.CreateCustomTimeZone("Asia/Taipei",
                                                                   new TimeSpan(08, 00, 00),
                                                                   "tw +8",
                                                                   "Asia/Taipei");

            RecurringJob.AddOrUpdate<TestJob2>($"jobname444 {Guid.NewGuid()}", a => a.Execute(testJob, null),
                                              "10 11 * * *" ,
                                               customTimeZone);

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