using System;
using Xunit;

namespace CreativeCoders.Core.UnitTests
{
    public class DelegateClassFactoryTests
    {
        [Fact]
        public void Create_WithClassTypeAsParameter_ReturnsCorrectClass()
        {
            var classFactory = new DelegateClassFactory(CreateClass);

            var instance = classFactory.Create(typeof(DelegateTestClass));
            
            Assert.IsType<DelegateTestClass>(instance);
        }
        
        [Fact]
        public void Create_WithSetupWithClassTypeAsParameter_ReturnsCorrectClass()
        {
            var classFactory = new DelegateClassFactory(CreateClass);

            var instance = classFactory.Create(typeof(DelegateTestClass), x => ((DelegateTestClass) x).Text = "1234");
            
            Assert.IsType<DelegateTestClass>(instance);
            Assert.Equal("1234", ((DelegateTestClass) instance).Text);
        }
        
        [Fact]
        public void Create_WithClassTypeAsGenericParameter_ReturnsCorrectClass()
        {
            var classFactory = new DelegateClassFactory(CreateClass);

            var instance = classFactory.Create<DelegateTestClass>();
            
            Assert.IsType<DelegateTestClass>(instance);
        }
        
        [Fact]
        public void Create_WithSetupWithClassTypeAsGenericParameter_ReturnsCorrectClass()
        {
            var classFactory = new DelegateClassFactory(CreateClass);

            var instance = classFactory.Create<DelegateTestClass>(x => x.Text = "abcd");
            
            Assert.IsType<DelegateTestClass>(instance);
            Assert.Equal("abcd", instance.Text);
        }
        
        [Fact]
        public void Create_GenericWithClassTypeAsGenericParameter_ReturnsCorrectClass()
        {
            var classFactory = new DelegateClassFactory<DelegateTestClass>(() => new DelegateTestClass());

            var instance = classFactory.Create();
            
            Assert.IsType<DelegateTestClass>(instance);
        }
        
        [Fact]
        public void Create_GenericWithClassTypeAsParameter_ReturnsCorrectClass()
        {
            var classFactory = new DelegateClassFactory<DelegateTestClass>(() => new DelegateTestClass());

            var instance = classFactory.Create(typeof(DelegateTestClass));
            
            Assert.IsType<DelegateTestClass>(instance);
        }
        
        [Fact]
        public void Create_GenericWithWrongClassTypeAsParameter_ThrowsException()
        {
            var classFactory = new DelegateClassFactory<DelegateTestClass>(() => new DelegateTestClass());

            Assert.Throws<InvalidOperationException>(() => classFactory.Create(typeof(string)));
        }
        
        [Fact]
        public void Create_GenericWithSetupWithClassTypeAsGenericParameter_ReturnsCorrectClass()
        {
            var classFactory = new DelegateClassFactory<DelegateTestClass>(() => new DelegateTestClass());

            var instance = classFactory.Create(x => x.Text = "abcd");
            
            Assert.IsType<DelegateTestClass>(instance);
            Assert.Equal("abcd", instance.Text);
        }
        
        [Fact]
        public void Create_GenericWithClassTypeAsGenericParameterForWrongType_ThrowsException()
        {
            var classFactory = new DelegateClassFactory<DelegateTestClass>(CreateClass<DelegateTestClass>);

            Assert.Throws<InvalidOperationException>(() => classFactory.Create());
        }
        
        [Fact]
        public void Create_GenericWithSetupWithClassTypeAsGenericParameterForWrongType_ThrowsException()
        {
            var classFactory = new DelegateClassFactory<DelegateTestClass>(CreateClass<DelegateTestClass>);

            Assert.Throws<InvalidOperationException>(() => classFactory.Create(x => x.Text = "abcd"));
        }

        private static object CreateClass(Type classType)
        {
            if (classType == typeof(DelegateTestClass))
            {
                return new DelegateTestClass();
            }
            
            throw new InvalidOperationException();
        }

        private static T CreateClass<T>()
        {
            throw new InvalidOperationException();
        }
    }
    
    public class DelegateTestClass
    {
        public string Text { get; set; }
    }
}