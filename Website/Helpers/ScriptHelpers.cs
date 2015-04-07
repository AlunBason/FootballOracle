namespace System.Web.Mvc
{
    public static class ScriptHelpers
    {
        public static IHtmlString SelectTab(this HtmlHelper helper, string tabToSelect)
        {
            return new HtmlString(string.Format("<script>$(document).ready(function () {{$('#{0}').addClass('active');}});</script>", tabToSelect));
        }
    }
}