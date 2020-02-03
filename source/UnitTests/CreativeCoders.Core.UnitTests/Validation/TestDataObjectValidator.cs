using System;
using CreativeCoders.Validation;

namespace CreativeCoders.Core.UnitTests.Validation
{
    public class TestDataObjectValidator : ValidatorBase<TestDataObject>
    {
        public TestDataObjectValidator(Action<TestDataObjectValidator> setupAction)
        {
            setupAction(this);
        }
    }
}