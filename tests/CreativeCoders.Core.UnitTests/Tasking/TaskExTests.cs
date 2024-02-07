using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using CreativeCoders.Core.Tasking;

namespace CreativeCoders.Core.UnitTests.Tasking
{
    public class TaskExTests
    {
        [Fact]
        public void AsCompletedTask_WithNonNullAction_ReturnsCompletedTask()
        {
            // Arrange
            var actionCallCount = 0;
            var action = () => { actionCallCount++; };

            // Act
            var task = action.AsCompletedTask();

            // Assert
            task
                .Should()
                .BeSameAs(Task.CompletedTask);

            task.IsCompleted
                .Should()
                .BeTrue();

            actionCallCount
                .Should()
                .Be(1);
        }

        [Fact]
        public void AsCompletedTask_WithNullAction_ThrowsArgumentNullException()
        {
            // Arrange
            Action action = null;

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Action act = () => action.AsCompletedTask();

            // Assert
            act
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void AsCompletedValueTask_WithNonNullAction_ReturnsCompletedValueTask()
        {
            // Arrange
            var actionCallCount = 0;
            var action = () => { actionCallCount++; };

            // Act
            var task = action.AsCompletedValueTask();

            // Assert
            task
                .Should()
                .Be(ValueTask.CompletedTask);

            task.IsCompleted
                .Should()
                .BeTrue();

            actionCallCount
                .Should()
                .Be(1);
        }

        [Fact]
        public void AsCompletedValueTask_WithNullAction_ThrowsArgumentNullException()
        {
            // Arrange
            Action action = null;

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Action act = () => action.AsCompletedValueTask();

            // Assert
            act
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}
