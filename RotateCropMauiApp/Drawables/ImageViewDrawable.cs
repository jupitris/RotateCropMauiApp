using RotateCropMauiApp.Helpers;
using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace RotateCropMauiApp.Drawables
{
    internal class ImageViewDrawable : GraphicsView, IDrawable
    {
        private double _currentAngle = 0;
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
            if (bindable is not ImageViewDrawable { Drawable: ImageViewDrawable drawable } view)
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

        public ImageViewDrawable()
        {
            // 切り抜き対象画像(この段階では表示のみ)を読み込む
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using Stream stream = assembly.GetManifestResourceStream("RotateCropMauiApp.Resources.Images.undou_zenpou_chugaeri.png");
            _image = CrossPlatformImageHelper.GetImageFromStream(stream);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_image is not null)
            {
                // 画像を描画
                canvas.DrawImage(_image, 0, 0, _image.Width, _image.Height);

                // ICanvas.Rotateは設定した角度がそのまま反映されるので、
                // 差分を設定するためにスライダーの値から現在の設定値を引く
                _currentAngle = Angle - _currentAngle;

                if (Path is not null)
                {
                    // 矩形の中心座標を取得する
                    RectF rect = Path.Bounds;
                    float centerX = rect.Left + (rect.Width / 2);
                    float centerY = rect.Top + (rect.Height / 2);

                    // ICanvasを回転したあとに矩形を描画
                    canvas.Rotate(Convert.ToSingle(_currentAngle), centerX, centerY);
                    canvas.StrokeSize = 2;
                    canvas.StrokeColor = Colors.Red;
                    canvas.FillColor = Color.FromRgba(255, 0, 0, 25);
                    canvas.FillPath(Path);
                    canvas.DrawPath(Path);
                }

                // スライダー値を保存
                _currentAngle = Angle;
            }
        }
    }
}
