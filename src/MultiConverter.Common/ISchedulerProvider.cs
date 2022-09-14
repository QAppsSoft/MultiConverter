using System.Reactive.Concurrency;

namespace MultiConverter.Common;

public interface ISchedulerProvider
{
    IScheduler CurrentThread { get; }
    IScheduler Dispatcher { get; }
    IScheduler Immediate { get; }
    IScheduler NewThread { get; }
    IScheduler TaskPoolLongRunning { get; }
    IScheduler TaskPool { get; }
}
