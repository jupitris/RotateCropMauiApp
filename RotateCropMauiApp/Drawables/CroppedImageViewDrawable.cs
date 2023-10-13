using RotateCropMauiApp.Helpers;
using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace RotateCropMauiApp.Drawables
{
    class CroppedImageViewDrawable : GraphicsView, IDrawable
    {
        private readonly IImage _image;

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly BindableProperty AngleProperty = BindableProperty.Create(nameof(Angle), typeof(double), typeof(ImageViewDrawable), propertyChanged: AngleChanged);

        public PathF Path
        {
            get => (PathF)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        public static readonly BindableProperty PathProperty = BindableProperty.Create(nameof(Path), typeof(PathF), typeof(ImageViewDrawable), propertyChanged: AngleChanged);

        /// <summary>
        /// Angleを変更したときに呼び出すメソッド。
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void AngleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not CroppedImageViewDrawable { Drawable: CroppedImageViewDrawable drawable } view)
            {
                return;
            }

            if (newValue is double angle)
            {
                drawable.Angle = angle;
            }
            else if (newValue is PathF path)
            {
                drawable.Path = path;
            }
            else
            {
                return;
            }

            view.Invalidate();  // 画面更新
        }

        public CroppedImageViewDrawable()
        {
            // 切り抜き対象画像を読み込む
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using Stream stream = assembly.GetManifestResourceStream("RotateCropMauiApp.Resources.Images.undou_zenpou_chugaeri.png");
            _image = CrossPlatformImageHelper.GetImageFromStream(stream);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_image is not null && Path is not null)
            {

                // 矩形の中心座標を取得する
                RectF rect = Path.Bounds;
                float centerX = rect.Left + (rect.Width / 2);
                float centerY = rect.Top + (rect.Height / 2);

                // 切り抜く位置を指定
                canvas.ClipRectangle(rect);

                // 画像を逆回転して描画 
                canvas.Rotate(Convert.ToSingle(-Angle), centerX, centerY);
                canvas.DrawImage(_image, 0, 0, _image.Width, _image.Height);

                // 以下のエラーが発生するので指定
                // "After calling CanvasDrawingSession.CreateLayer, you must close the resulting CanvasActiveLayer before ending the CanvasDrawingSession."
                // グラフィック状態をリセット
                canvas.ResetState();
            }
        }
    }
}
