using Avalonia;
using Avalonia.Controls;

namespace MusicPlayerApp.Views
{
    public partial class SpinningImage : Image
    {
        public static readonly StyledProperty<bool> IsSpinningProperty = AvaloniaProperty.Register<SpinningImage, bool>(nameof(IsSpinning));

        public bool IsSpinning
        {
            get { return GetValue(IsSpinningProperty); }
            set { SetValue(IsSpinningProperty, value); }
        }

        public SpinningImage()
        {
            InitializeComponent();
        }
    }
}
