using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace Quartz.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private IScheduler sched;
        private ISchedulerFactory schedFact;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("$projectname$ entry point called", "Information");

            // Start init
            // construct job info
            IJobDetail jobDetail = new JobDetailImpl("myJob", null, typeof(TraceJob));
            // fire every hour
            ITrigger trigger = new SimpleTriggerImpl("myTrigger", null, 3, new TimeSpan(0, 0, 10));
            sched.ScheduleJob(jobDetail, trigger);
            // End init

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            // construct a scheduler factory
            schedFact = new StdSchedulerFactory();

            // get a scheduler
            sched = schedFact.GetScheduler();

            // Start the Scheduler
            sched.Start();

            return base.OnStart();
        }

        public override void OnStop()
        {
            // Shutdown the Scheduler
            sched.Shutdown(true);

            base.OnStop();
        }
    }
}
