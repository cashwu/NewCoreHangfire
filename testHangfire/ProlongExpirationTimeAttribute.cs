using System;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace testHangfire
{
    public class ProlongExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateApplied(ApplyStateContext filterContext, IWriteOnlyTransaction transaction)
        {
            filterContext.JobExpirationTimeout = TimeSpan.FromDays(2);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromDays(2);
        }
    }
}