using System.Reactive.Concurrency;

namespace MultiConverter.Common.Testing;

public class ImmediateSchedulers : ISchedulerProvider
{
    public IScheduler CurrentThread { get; } = ImmediateScheduler.Instance;
    public IScheduler Dispatcher { get; } = ImmediateScheduler.Instance;
    public IScheduler Immediate { get; } = ImmediateScheduler.Instance;
    public IScheduler NewThread { get; } = ImmediateScheduler.Instance;
    public IScheduler TaskPoolLongRunning { get; } = ImmediateScheduler.Instance;
    public IScheduler TaskPool { get; } = ImmediateScheduler.Instance;
}
