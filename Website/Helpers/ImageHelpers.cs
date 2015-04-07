using System;
using System.Web;

namespace System.Web.Mvc
{
    public static class ImageHelpers
    {
        public static IHtmlString BytesToImage(this HtmlHelper helper, byte[] bytes, int width, int height, string title)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", string.Format("data:image/gif;base64,{0}", bytes.ToBase64Image(width, height)));
            builder.MergeAttribute("title", title);
            builder.MergeAttribute("alt", title);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));

        }
    }
}