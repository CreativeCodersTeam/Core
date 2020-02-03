using System;
using System.Threading;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading
{
    public class LockExtensionTests
    {
        [Fact]
        public void LockTest()
        {
            var actionExecuted = false;
            var obj = new object();
            var action = new Action(() => actionExecuted = true);
            obj.Lock(action);

            Assert.True(actionExecuted);
        }

        [Fact]
        public void LockFuncTest()
        {
            var obj = new object();
            var funcResult = obj.Lock(() => true);

            Assert.True(funcResult);
        }

        [Fact]
        public void ReadActionTest()
        {
            var actionExecuted = false;
            var lockSlim = new ReaderWriterLockSlim();
            lockSlim.Read(new Action(() => actionExecuted = true));

            Assert.True(actionExecuted);
        }

        [Fact]
        public void ReadActionTestWithTimeout()
        {
            var actionExecuted = false;
            var lockSlim = new ReaderWriterLockSlim();
            lockSlim.Read(new Action(() => actionExecuted = true), 1234);

            Assert.True(actionExecuted);
        }

        [Fact]
        public void WriteActionTest()
        {
            var actionExecuted = false;
            var lockSlim = new ReaderWriterLockSlim();
            lockSlim.Write(new Action(() => actionExecuted = true));

            Assert.True(actionExecuted);
        }

        [Fact]
        public void WriteActionTestWithTimeout()
        {
            var actionExecuted = false;
            var lockSlim = new ReaderWriterLockSlim();
            lockSlim.Write(new Action(() => actionExecuted = true), 1234);

            Assert.True(actionExecuted);
        }

        [Fact]
        public void ReadFuncTest()
        {
            var lockSlim = new ReaderWriterLockSlim();
            var funcResult = lockSlim.Read(() => true);

            Assert.True(funcResult);
        }

        [Fact]
        public void ReadFuncTestWithTimeout()
        {
            var lockSlim = new ReaderWriterLockSlim();
            var funcResult = lockSlim.Read(() => true, 1234);

            Assert.True(funcResult);
        }

        [Fact]
        public void WriteFuncTest()
        {
            var lockSlim = new ReaderWriterLockSlim();
            var funcResult = lockSlim.Write(() => true);

            Assert.True(funcResult);
        }

        [Fact]
        public void WriteFuncTestWithTimeout()
        {
            var lockSlim = new ReaderWriterLockSlim();
            var funcResult = lockSlim.Write(() => true, 1234);

            Assert.True(funcResult);
        }
    }
}