using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace CreativeCoders.UnitTests.Net.Http;

///<inheritdoc/>
public class RecordedRequestVerifier : IRecordedRequestVerifier
{
    private IList<RecordedHttpRequest> _recordedHttpRequests;

    internal RecordedRequestVerifier(IList<RecordedHttpRequest> recordedHttpRequests)
    {
        _recordedHttpRequests = recordedHttpRequests;

        CheckRecordedRequests("No recorded requests found.");
    }

    private void CheckRecordedRequests(string verificationInfoText)
    {
        if (!_recordedHttpRequests.Any())
        {
            throw new RecordedRequestVerificationFailedException(verificationInfoText);
        }
    }

    public IRecordedRequestVerifier WithVerb(HttpMethod httpMethod)
    {
        return RequestMeets(requestMessage => requestMessage.Method == httpMethod,
            $"Verb '{httpMethod}' not found.");
    }

    public IRecordedRequestVerifier WithContentType(string contentType)
    {
        return RequestMeets(requestMessage => requestMessage.Content?.Headers.ContentType?.MediaType == contentType,
            $"Content type '{contentType}' not found");
    }

    public IRecordedRequestVerifier WithContentText(string content)
    {
        return RequestMeetsCore(request => request.Content.ReadAsStringAsync().Result == content,
            $"Content text '{content}' not found.");
    }

    private IRecordedRequestVerifier RequestMeetsCore(Func<RecordedHttpRequest, bool> verifyRequest, string verificationInfoText)
    {
        _recordedHttpRequests = _recordedHttpRequests
            .Where(verifyRequest)
            .ToList();

        CheckRecordedRequests(verificationInfoText);

        return this;
    }

    public IRecordedRequestVerifier RequestMeets(Func<HttpRequestMessage, bool> verifyRequestMessage, string verificationInfoText)
    {
        return RequestMeetsCore(request => verifyRequestMessage(request.RequestMessage), verificationInfoText);
    }

    public IRecordedRequestVerifier RequestMeets(
        Func<HttpRequestMessage, CancellationToken, bool> verifyRequestMessage, string verificationInfoText)
    {
        return RequestMeetsCore(request => verifyRequestMessage(request.RequestMessage, request.CancellationToken), verificationInfoText);
    }

    public IRecordedRequestVerifier RequestCount(int count)
    {
        if (_recordedHttpRequests.Count != count)
        {
            throw new RecordedRequestVerificationFailedException(
                $"Count does not match. Expected: {count}. Actual: {_recordedHttpRequests.Count}.");
        }

        return this;
    }
}