using System.Linq;

namespace System.Web.Mvc
{
    public static class ModelMetadataHelper
    {
        public static string DisabledCheck(this ModelMetadata modelMetadata)
        {
            return modelMetadata.IsReadOnly ? string.Format("disabled=\"disabled\"", null) : string.Empty;
        }
    }
}