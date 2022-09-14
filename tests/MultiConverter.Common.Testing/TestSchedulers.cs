using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;

namespace MultiConverter.Common.Testing;

public sealed class TestSchedulers : ISchedulerProvider
{
    public TestScheduler CurrentThread { get; } = new();

    public TestScheduler Dispatcher { get; } = new();

    public TestScheduler Immediate { get; } = new();

    public TestScheduler NewThread { get; } = new();

    public IScheduler TaskPoolLongRunning { get; } = new TestScheduler();

    public TestScheduler TaskPool { get; } = new();

    #region Explicit implementation of ISchedulerService

    IScheduler ISchedulerProvider.CurrentThread => CurrentThread;
    IScheduler ISchedulerProvider.Dispatcher => Dispatcher;
    IScheduler ISchedulerProvider.Immediate => Immediate;
    IScheduler ISchedulerProvider.NewThread => NewThread;
    IScheduler ISchedulerProvider.TaskPoolLongRunning => TaskPoolLongRunning;
    IScheduler ISchedulerProvider.TaskPool => TaskPool;

    #endregion Explicit implementation of ISchedulerService
}
