using System.Reflection;
#if IOS || ANDROID || MACCATALYST
using Microsoft.Maui.Graphics.Platform;
#elif WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif
using IImage = Microsoft.Maui.Graphics.IImage;

namespace RotateCropMauiApp.Interfaces
{
    internal class GraphicsDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // 切り抜き対象画像(この段階では表示のみ)を読み込む
            IImage image;
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using Stream stream = assembly.GetManifestResourceStream("RotateCropMauiApp.Resources.Images.undou_zenpou_chugaeri.png");

#if IOS || ANDROID || MACCATALYST
            image = PlatformImage.FromStream(stream);
#elif WINDOWS
            image = new W2DImageLoadingService().FromStream(stream);
#endif

            if (image is not null)
            {
                canvas.DrawImage(image, 0, 0, image.Width, image.Height);
            }
        }
    }
}
