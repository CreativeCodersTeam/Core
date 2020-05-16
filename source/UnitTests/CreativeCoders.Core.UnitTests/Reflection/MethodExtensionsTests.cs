using System;
using CreativeCoders.Core.Reflection;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Reflection
{
    public class MethodExtensionsTests
    {
        [Fact]
        public void ExecuteMethod_ExistingMethod_MethodIsCalled()
        {
            var testObject = A.Fake<IReflectionTestObject>();
            
            testObject.ExecuteMethod(nameof(testObject.Execute));

            A.CallTo(() => testObject.Execute()).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void ExecuteMethod_NotExistingMethod_ThrowsException()
        {
            var testObject = A.Fake<IReflectionTestObject>();
            
            Assert.Throws<MissingMethodException>(() => testObject.ExecuteMethod(nameof(testObject.Execute) + "1234"));
        }
        
        [Fact]
        public void ExecuteMethod_OverloadedExistingMethod_MethodIsCalled()
        {
            var testObject = A.Fake<IReflectionTestObject>();
            
            testObject.ExecuteMethod(nameof(testObject.ExecuteEx));

            A.CallTo(() => testObject.ExecuteEx()).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void ExecuteMethod_OverloadedExistingMethodWithOneArgument_MethodIsCalled()
        {
            var testObject = A.Fake<IReflectionTestObject>();
            
            testObject.ExecuteMethod(nameof(testObject.ExecuteEx), 1234);

            A.CallTo(() => testObject.ExecuteEx(1234)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void ExecuteMethod_OverloadedExistingMethodWithTwoArgument_MethodIsCalled()
        {
            var testObject = A.Fake<IReflectionTestObject>();
            
            testObject.ExecuteMethod(nameof(testObject.ExecuteEx), 1234, "Test");

            A.CallTo(() => testObject.ExecuteEx(1234, "Test")).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void ExecuteMethod_OverloadedExistingMethodWithThreeArgument_MethodIsCalled()
        {
            var testObject = A.Fake<IReflectionTestObject>();
            
            testObject.ExecuteMethod(nameof(testObject.ExecuteEx), 1234, "Test", true);

            A.CallTo(() => testObject.ExecuteEx(1234, "Test", true)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void ExecuteMethod_ExistingMethodWithReturnValue_MethodIsCalled()
        {
            var testObject = A.Fake<IReflectionTestObject>();

            A.CallTo(() => testObject.Calculate()).Returns(1234);
            
            var result = testObject.ExecuteMethod<int>(nameof(testObject.Calculate));

            A.CallTo(() => testObject.Calculate()).MustHaveHappenedOnceExactly();
            Assert.Equal(1234, result);
        }
        
        [Fact]
        public void ExecuteMethod_NotExistingMethodWithReturnValue_ThrowsException()
        {
            var testObject = A.Fake<IReflectionTestObject>();
            
            Assert.Throws<MissingMethodException>(() => testObject.ExecuteMethod<int>(nameof(testObject.Calculate) + "1234"));
        }
        
        [Fact]
        public void ExecuteMethod_OverloadedExistingMethodWithOneArgumentWithReturnValue_MethodIsCalled()
        {
            var testObject = A.Fake<IReflectionTestObject>();
            
            A.CallTo(() => testObject.Calculate(3456)).Returns(1234);
            
            var result = testObject.ExecuteMethod<int>(nameof(testObject.Calculate), 3456);

            A.CallTo(() => testObject.Calculate(3456)).MustHaveHappenedOnceExactly();
            Assert.Equal(1234, result);
        }
        
        [Fact]
        public void ExecuteMethod_OverloadedExistingMethodWithTwoArgumentWithReturnValue_MethodIsCalled()
        {
            var testObject = A.Fake<IReflectionTestObject>();
            
            A.CallTo(() => testObject.Calculate(3456, "Test")).Returns(1234);
            
            var result = testObject.ExecuteMethod<int>(nameof(testObject.Calculate), 3456, "Test");

            A.CallTo(() => testObject.Calculate(3456, "Test")).MustHaveHappenedOnceExactly();
            Assert.Equal(1234, result);
        }
        
        [Fact]
        public void ExecuteGenericMethod_CallVoidMethod_MethodWasCalled()
        {
            const int value = 1234;
            var instance = new GenericMethodTestClass();

            instance.ExecuteGenericMethod(nameof(GenericMethodTestClass.DoSomething),
                new [] {new GenericArgument("T", typeof(int))}, value);

            Assert.Equal(value.ToString(), instance.Data);
        }

        [Fact]
        public void ExecuteGenericMethod_CallVoidMethodWithType_MethodWasCalled()
        {
            const int value = 1234;
            var instance = new GenericMethodTestClass();

            instance.ExecuteGenericMethod(nameof(GenericMethodTestClass.DoSomething),
                new[] { typeof(int) }, value);

            Assert.Equal(value.ToString(), instance.Data);
        }

        [Fact]
        public void ExecuteGenericMethod_CallMethodWithResult_MethodReturnsResult()
        {
            const int value = 12345;
            var instance = new GenericMethodTestClass();

            var result = instance.ExecuteGenericMethod<int>(nameof(GenericMethodTestClass.GetData),
                new[] { new GenericArgument("T", typeof(int)) }, value);

            Assert.Equal(value, result);
        }

        [Fact]
        public void ExecuteGenericMethod_CallMethodWithResultAndType_MethodReturnsResult()
        {
            const int value = 12345;
            var instance = new GenericMethodTestClass();

            var result = instance.ExecuteGenericMethod<int>(nameof(GenericMethodTestClass.GetData),
                new[] { typeof(int) }, value);

            Assert.Equal(value, result);
        }

        [Fact]
        public void ExecuteGenericMethod_CallMethodWithResultAndTwoGenericTypeParameters_MethodReturnsResult()
        {
            const int value = 12345;
            var dateTime = DateTime.Now;

            var instance = new GenericMethodTestClass();

            var result = instance.ExecuteGenericMethod<string>(nameof(GenericMethodTestClass.GetData),
                new[]
                {
                    new GenericArgument("T1", typeof(int)),
                    new GenericArgument("T2", typeof(DateTime))
                },
                value,
                dateTime);

            Assert.Equal(value.ToString() + dateTime, result);
        }

        [Fact]
        public void ExecuteGenericMethod_CallMethodWithResultAndTwoGenericTypeParametersAndType_MethodReturnsResult()
        {
            const int value = 12345;
            var dateTime = DateTime.Now;

            var instance = new GenericMethodTestClass();

            var result = instance.ExecuteGenericMethod<string>(nameof(GenericMethodTestClass.GetData),
                new[]
                {
                    typeof(int),
                    typeof(DateTime)
                },
                value,
                dateTime);

            Assert.Equal(value.ToString() + dateTime, result);
        }

        [Fact]
        public void ExecuteGenericMethod_CallMethodWithResultAndTwoGenericTypeParametersAndTypeNotExactlyMatching_MethodReturnsResult()
        {
            var value = new TestDataClassSpecial();
            var dateTime = DateTime.Now;

            var instance = new GenericMethodTestClass();

            var result = instance.ExecuteGenericMethod<string>(nameof(GenericMethodTestClass.GetData),
                new[]
                {
                    typeof(TestDataClassBase),
                    typeof(DateTime)
                },
                value,
                dateTime);

            Assert.Equal(value.ToString() + dateTime, result);
        }

        [Fact]
        public void ExecuteGenericMethod_CallNotExistingMethod_ThrowsException()
        {
            var instance = new GenericMethodTestClass();

            Assert.Throws<MissingMethodException>(() => instance.ExecuteGenericMethod("NotExisting",
                new[] {new GenericArgument("T", typeof(int))}, 1234));

            Assert.Throws<MissingMethodException>(() =>
                instance.ExecuteGenericMethod<string>(nameof(GenericMethodTestClass.GetData),
                    new[]
                    {
                        new GenericArgument("T2", typeof(int)),
                        new GenericArgument("T3", typeof(DateTime))
                    },
                    123,
                    345));
        }

        [Fact]
        public void ExecuteGenericMethod_CallNotExistingMethodAndType_ThrowsException()
        {
            var instance = new GenericMethodTestClass();

            Assert.Throws<MissingMethodException>(() => instance.ExecuteGenericMethod("NotExisting",
                new[] { new GenericArgument("T", typeof(int)) }, 1234));

            Assert.Throws<MissingMethodException>(() =>
                instance.ExecuteGenericMethod<string>(nameof(GenericMethodTestClass.GetData),
                    new[]
                    {
                        typeof(int),
                        typeof(DateTime),
                        typeof(string)
                    },
                    123,
                    345));
        }

        //[Fact]
        //public void ExecuteGenericMethod_CallMethodWithParamsEmpty_CallSucceeded()
        //{
        //    var instance = new GenericMethodTestClass();

        //    instance.ExecuteGenericMethod(nameof(GenericMethodTestClass.SetDataWithParam), new[] { typeof(int) });

        //    Xunit.Assert.Equal(typeof(int).Name, instance.Data);
        //}

        //[Fact]
        //public void ExecuteGenericMethod_CallMethodWithParams_CallSucceeded()
        //{
        //    var instance = new GenericMethodTestClass();

        //    instance.ExecuteGenericMethod(nameof(GenericMethodTestClass.SetDataWithParam), new[] {typeof(int)}, "Test");

        //    Xunit.Assert.Equal(typeof(int).Name + "Test", instance.Data);
        //}

        [Fact]
        public void CreateGenericMethodFunc_NoneGenericFunc_CallSucceeded()
        {
            const int value = 1234;
            var dateTimeNow = DateTime.Now;

            var instance = new GenericMethodTestClass();

            var func = instance.CreateGenericMethodFunc<string>(nameof(GenericMethodTestClass.GetData),
                new [] {typeof(int), typeof(DateTime)}, typeof(int), typeof(DateTime));

            var result = func(new object[] {value, dateTimeNow});

            Assert.Equal(value.ToString() + dateTimeNow, result);
        }

        [Fact]
        public void CreateGenericMethodFunc_FuncWithOneGenericParameters_CallSucceeded()
        {
            const int value = 1234;
            
            var instance = new GenericMethodTestClass();

            var func = instance.CreateGenericMethodFunc<int, string>(nameof(GenericMethodTestClass.GetDataText), new []{ typeof(int) }, typeof(int));

            var result = func(value);

            Assert.Equal(value.ToString(), result);
        }

        [Fact]
        public void CreateGenericMethodFunc_FuncWithTwoGenericParameters_CallSucceeded()
        {
            const int value = 1234;
            var dateTimeNow = DateTime.Now;

            var instance = new GenericMethodTestClass();

            var func = instance.CreateGenericMethodFunc<int, DateTime, string>(nameof(GenericMethodTestClass.GetData),
                new[] {typeof(int), typeof(DateTime)}, typeof(int), typeof(DateTime));

            var result = func(value, dateTimeNow);

            Assert.Equal(value.ToString() + dateTimeNow, result);
        }

        [Fact]
        public void CreateGenericMethodFunc_FuncWithThreeGenericParameters_CallSucceeded()
        {
            const int value = 1234;
            var dateTimeNow = DateTime.Now;
            const bool boolValue = true;

            var instance = new GenericMethodTestClass();

            var func = instance.CreateGenericMethodFunc<int, DateTime, bool, string>(
                nameof(GenericMethodTestClass.GetData3), new[] { typeof(int), typeof(DateTime), typeof(bool) },
                typeof(int), typeof(DateTime), typeof(bool));

            var result = func(value, dateTimeNow, boolValue);

            Assert.Equal(value.ToString() + dateTimeNow + boolValue, result);
        }

        [Fact]
        public void CreateGenericMethodFunc_FuncWithFourGenericParameters_CallSucceeded()
        {
            const int value = 1234;
            var dateTimeNow = DateTime.Now;
            const bool boolValue = true;
            const double doubleValue = 123.456;

            var instance = new GenericMethodTestClass();

            var func = instance.CreateGenericMethodFunc<int, DateTime, bool, double, string>(
                nameof(GenericMethodTestClass.GetData4), new[] { typeof(int), typeof(DateTime), typeof(bool), typeof(double) },
                typeof(int), typeof(DateTime), typeof(bool), typeof(double));

            var result = func(value, dateTimeNow, boolValue, doubleValue);

            Assert.Equal(value.ToString() + dateTimeNow + boolValue + doubleValue, result);
        }
    }
}