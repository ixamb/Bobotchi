using System;

namespace Core.Runtime.Services.Delayer
{
    public interface IActionDelayerService : ISingleton
    {
        ActionDelayer Get(string code);
        void Delay(float durationInSeconds, Action action, string code = "");
        void Cancel(string code);
        void CancelAll();
    }
}