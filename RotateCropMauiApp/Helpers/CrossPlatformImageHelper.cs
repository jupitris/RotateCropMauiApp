#if IOS || ANDROID || MACCATALYST
using Microsoft.Maui.Graphics.Platform;
#elif WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif
using IImage = Microsoft.Maui.Graphics.IImage;

namespace RotateCropMauiApp.Helpers
{
    public class CrossPlatformImageHelper
    {
        public static IImage GetImageFromStream(Stream stream)
        {
            IImage image = null;
#if IOS || ANDROID || MACCATALYST
            image = PlatformImage.FromStream(stream);
#elif WINDOWS
            image = new W2DImageLoadingService().FromStream(stream);
#endif
            return image;
        }
    }
}
