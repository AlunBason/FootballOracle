using System.IO;
using ImageResizer;

namespace System.Web
{
    public static class WebExtensions
    {
        public static byte[] ToBytes(this HttpPostedFileBase fileBase)
        {
            var outStream = new MemoryStream();
            var settings = new ResizeSettings("maxwidth=100&maxheight=100");
            
            ImageResizer.ImageBuilder.Current.Build(fileBase.InputStream, outStream, settings);

            return outStream.ToArray();
        }
    }
}
