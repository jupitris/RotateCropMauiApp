using RotateCropMauiApp.ViewModels;

namespace RotateCropMauiApp.ContentPages
{
    public partial class ImageViewPage : ContentPage
    {
        private ImageViewPageViewModel BindingData;

        public ImageViewPage()
        {
            InitializeComponent();

            // 表示エリアの80%にした矩形パスをデフォルト値にする
            float x = Convert.ToSingle(Application.Current.MainPage.Window.Width / 2);
            float y = Convert.ToSingle(Application.Current.MainPage.Window.Height / 2);
            PointF p0 = new(x - (x * 0.8f), y - (y * 0.8f));
            PointF p1 = new(x + (x * 0.8f), y - (y * 0.8f));
            PointF p2 = new(x + (x * 0.8f), y + (y * 0.8f));
            PointF p3 = new(x - (x * 0.8f), y + (y * 0.8f));

            PathF path = new();
            path.MoveTo(p0.X, p0.Y);
            path.LineTo(p1.X, p1.Y);
            path.LineTo(p2.X, p2.Y);
            path.LineTo(p3.X, p3.Y);
            path.Close();

            BindingData = new()
            {
                Path = path
            };
            BindingContext = BindingData;
        }

        private void CropImage_Released(object sender, EventArgs e)
        {
            CroppedImagePageViewModel viewModel = new()
            {
                Angle = BindingData.Angle,
                Path = BindingData.Path
            };
            Navigation.PushAsync(new CroppedImagePage
            {
                BindingContext = viewModel
            });
        }

        private void MainImage_StartInteraction(object sender, TouchEventArgs e)
        {
            PointF point = e.Touches.FirstOrDefault();
            PathF path = new();
            path.MoveTo(point.X, point.Y);
            path.LineTo(point.X, point.Y);
            path.LineTo(point.X, point.Y);
            path.LineTo(point.X, point.Y);
            path.Close();
            BindingData.Path = path;
        }

        private void MainImage_EndInteraction(object sender, TouchEventArgs e)
        {
            RectF rect = BindingData.Path.Bounds;
            PointF point = e.Touches.LastOrDefault();
            PathF path = new();
            path.MoveTo(rect.X, rect.Y);
            path.LineTo(point.X, rect.Y);
            path.LineTo(point.X, point.Y);
            path.LineTo(rect.X, point.Y);
            path.Close();
            BindingData.Path = path;
        }

        private void MainImage_DragInteraction(object sender, TouchEventArgs e)
        {
            RectF rect = BindingData.Path.Bounds;
            PointF point = e.Touches.LastOrDefault();
            PathF path = new();
            path.MoveTo(rect.X, rect.Y);
            path.LineTo(point.X, rect.Y);
            path.LineTo(point.X, point.Y);
            path.LineTo(rect.X, point.Y);
            path.Close();
            BindingData.Path = path;
        }
    }
}