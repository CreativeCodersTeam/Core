using System.IO;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Net;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace CreativeCoders.AspNetCore;

[PublicAPI]
public class RawRequestBodyFormatter : InputFormatter
{
    public RawRequestBodyFormatter()
    {
        SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentMediaTypes.Text.Plain));
        SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentMediaTypes.Application.OctetStream));
    }

    public override bool CanRead(InputFormatterContext context)
    {
        Ensure.IsNotNull(context, nameof(context));

        var contentType = context.HttpContext.Request.ContentType;
        return string.IsNullOrEmpty(contentType) ||
               ContentMediaTypes.IsContentType(contentType, ContentMediaTypes.Text.Plain) ||
               ContentMediaTypes.IsContentType(contentType, ContentMediaTypes.Application.OctetStream);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
    {
        var request = context.HttpContext.Request;
        var contentType = context.HttpContext.Request.ContentType;

        if (string.IsNullOrEmpty(contentType) ||
            ContentMediaTypes.IsContentType(contentType, ContentMediaTypes.Text.Plain))
        {
            using (var reader = new StreamReader(request.Body))
            {
                var content = await reader.ReadToEndAsync().ConfigureAwait(false);
                return await InputFormatterResult.SuccessAsync(content).ConfigureAwait(false);
            }
        }

        if (!ContentMediaTypes.IsContentType(contentType, ContentMediaTypes.Application.OctetStream))
        {
            return await InputFormatterResult.FailureAsync().ConfigureAwait(false);
        }

        await using (var ms = new MemoryStream(2048))
        {
            await request.Body.CopyToAsync(ms).ConfigureAwait(false);
            var content = ms.ToArray();
            return await InputFormatterResult.SuccessAsync(content).ConfigureAwait(false);
        }
    }
}
