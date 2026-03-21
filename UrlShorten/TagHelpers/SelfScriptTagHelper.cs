using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace UrlShorten.TagHelpers
{
    [HtmlTargetElement("self-script")]
    public class SelfScriptTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext{ get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "script";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("type", "text/javascript");

            string viewPath = ViewContext.View.Path;
            string scriptPath = $"{viewPath}.js";
            output.Attributes.SetAttribute("src", scriptPath);
        }
    }
}
