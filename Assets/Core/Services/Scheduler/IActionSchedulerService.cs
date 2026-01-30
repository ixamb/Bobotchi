using System;

namespace Core.Services.Scheduler
{
    public interface IActionSchedulerService : ISingleton
    {
        ActionScheduler CreateScheduler(string code, Action action, float durationInSeconds, SchedulerEndAction endAction);
        ActionScheduler GetScheduler(string code);
        void DestroyScheduler(string code);
        void DestroyAllSchedulers();
    }
}