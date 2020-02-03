using System;

namespace CreativeCoders.Core.UnitTests.Reflection
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DummyTestAttribute : Attribute
    {
        public int Value { get; set; }
    }
}