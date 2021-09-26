using System;

namespace CreativeCoders.NukeBuild.Exceptions
{
    public class UnitTestFailedException : Exception
    {
        public UnitTestFailedException(string unitTestProject, Exception innerException)
            : base($"Unit test project failed: {unitTestProject}", innerException)
        {
            
        }
    }
}
