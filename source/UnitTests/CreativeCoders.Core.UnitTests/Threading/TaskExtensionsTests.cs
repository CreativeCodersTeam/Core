using System.Threading.Tasks;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading;

public class TaskExtensionsTests
{
    [Fact]
    public async Task ToTask()
    {
        const int value = 1234;
            
        var task = (Task) GetIntValue(value);

        var result = await task.ToTask<int>();

        Assert.IsType<int>(result);
        Assert.Equal(value, result);
    }

    private static async Task<T> GetIntValue<T>(T value)
    {
        await Task.Delay(500);
            
        return value;
    } 
}