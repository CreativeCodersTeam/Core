using System;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.AspNetCore.Blazor.Components.Base;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace CreativeCoders.AspNetCore.Blazor.Components
{
    [PublicAPI]
    public class HtmlInclude : ControlBase
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        private MarkupString Content { get; set; }
        
        private bool ContentLoadFailed { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                var html = ContentLoader != null ? await ContentLoader() : await HttpClient.GetStringAsync(Url);
        
                Content = new MarkupString(html);

                ContentLoadFailed = false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"HtmlInclude load data failed. {e}");
                
                ContentLoadFailed = true;
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            var sequence = 0;

            if (UseEnclosingDiv)
            {
                builder.OpenElement(sequence++, "div");
            }
            
            if (ContentLoadFailed)
            {
                builder.AddContent(sequence, FailedContent);
            }
            else
            {
                builder.AddContent(sequence, Content);
            }

            if (UseEnclosingDiv)
            {
                builder.CloseElement();
            }
        }

        [Parameter]
        public string Url { get; set; }
    
        [Parameter]
        public RenderFragment FailedContent { get; set; }
    
        [Parameter]
        public bool UseEnclosingDiv { get; set; }
        
        [Parameter]
        public Func<Task<string>> ContentLoader { get; set; }
    }
}