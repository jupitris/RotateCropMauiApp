using RotateCropMauiApp.ViewModels;

namespace RotateCropMauiApp.ContentPages
{
    public partial class CroppedImagePage : ContentPage
    {
        private CroppedImagePageViewModel BindingData;

        public CroppedImagePage()
        {
            InitializeComponent();
        }

        private async void Reset_Released(object sender, EventArgs e) => await Navigation.PopAsync();
    }
}