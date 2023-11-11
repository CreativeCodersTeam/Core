using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading;

public class SimpleMonitorTests
{
    [Fact]
    public void Ctor_NewInstance_IsNotBusy()
    {
        var monitor = new SimpleMonitor();

        Assert.False(monitor.Busy);
    }

    [Fact]
    public void Enter_Call_IsBusy()
    {
        var monitor = new SimpleMonitor();

        monitor.Enter();

        Assert.True(monitor.Busy);
    }

    [Fact]
    public void Enter_DisposeAfter_IsNotBusy()
    {
        var monitor = new SimpleMonitor();

        monitor.Enter();
        monitor.Dispose();

        Assert.False(monitor.Busy);
    }

    [Fact]
    public void Enter_EnterTwoTimesDisposeOneTime_IsBusy()
    {
        var monitor = new SimpleMonitor();

        monitor.Enter();
        monitor.Enter();
        monitor.Dispose();

        Assert.True(monitor.Busy);
    }

    [Fact]
    public void Enter_EnterTwoTimesDisposeTwoTime_IsNotBusy()
    {
        var monitor = new SimpleMonitor();

        monitor.Enter();
        monitor.Enter();
        monitor.Dispose();
        monitor.Dispose();

        Assert.False(monitor.Busy);
    }
}
