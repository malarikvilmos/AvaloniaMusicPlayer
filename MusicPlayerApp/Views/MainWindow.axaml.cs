using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using MusicPlayerApp.Interfaces;
using MusicPlayerApp.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;

namespace MusicPlayerApp.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            Activated += MainWindow_Activated;
            this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
        }

        private void MainWindow_Activated(object? sender, System.EventArgs e)
        {
            if (ViewModel is null) return;

            if (DataContext is IHasWindowReference vm)
                vm.Window = this;

            slider.Tapped += ViewModel.SkipTo;   
        }

        private async Task DoShowDialogAsync(InteractionContext<MainWindowViewModel, MainWindowViewModel?> interaction)
        {
            var dialog = new VolumeWindow();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<MainWindowViewModel?>(this);
            interaction.SetOutput(result);
        }
    }
}
