using System;

namespace CreativeCoders.UnitTests.Net.Http
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Exception for signalling that verification for recorded requests failed. </summary>
    ///
    /// <seealso cref="Exception"/>
    ///-------------------------------------------------------------------------------------------------
    public class RecordedRequestVerificationFailedException : Exception
    {
        internal RecordedRequestVerificationFailedException(string failedVerification)
            : base($"Verification for recorded requests failed. {failedVerification}")
        {
        }
    }
}