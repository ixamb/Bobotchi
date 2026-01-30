using System;

namespace Core.Services.Delayer
{
    public interface IActionDelayerService : ISingleton
    {
        void Delay(float durationInSeconds, Action action, string code = "");
        void Cancel(string code);
        void CancelAll();
    }
}