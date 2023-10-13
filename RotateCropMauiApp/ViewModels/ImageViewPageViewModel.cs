using CommunityToolkit.Mvvm.ComponentModel;

namespace RotateCropMauiApp.ViewModels
{
    public partial class ImageViewPageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private double angle;

        [ObservableProperty]
        private PathF path;
    }
}
