using System;
using Core.Runtime.Services;

namespace Core.Runtime.Services.Delayer
{
    public interface IActionDelayerService : ISingleton
    {
        void Delay(float durationInSeconds, Action action, string code = "");
        void Cancel(string code);
        void CancelAll();
    }
}