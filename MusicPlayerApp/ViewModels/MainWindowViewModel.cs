using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MusicPlayerApp.Interfaces;
using MusicPlayerApp.Models;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicPlayerApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IHasWindowReference
    {
        public Window? Window { get; set; }

        public Interaction<MainWindowViewModel, MainWindowViewModel> ShowDialog { get; }

        public MainWindowViewModel()
        {
            ShowDialog = new();
        }

        private string volumeValue = "100";
        public string VolumeValue
        {
            get => volumeValue;
            set
            {
                this.RaiseAndSetIfChanged(ref volumeValue, value);
                Song.SetVolume(Int32.Parse(volumeValue));
            }
        }

        private float volumeSlider = 100.0f;
        public float VolumeSlider
        {
            get => volumeSlider;
            set
            {
                VolumeValue = ((int)value).ToString();
                this.RaiseAndSetIfChanged(ref volumeSlider, value);
            }
        }

        private bool isSpinning;
        public bool IsSpinning
        {
            get => isSpinning;
            set => this.RaiseAndSetIfChanged(ref isSpinning, value);
        }

        public int Length { get; set; } = 0;

        private string? name;
        public string? Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }

        private int timePassed;
        public int TimePassed
        {
            get => timePassed;
            set => this.RaiseAndSetIfChanged(ref timePassed, value);
        }

        private int timeRemaining;
        public int TimeRemaining
        {
            get => timeRemaining;
            set => this.RaiseAndSetIfChanged(ref timeRemaining, value);
        }

        private float slider;
        public float Slider
        {
            get => slider;
            set => this.RaiseAndSetIfChanged(ref slider, value);
        }

        private Song? song;

        public void SkipTo(object? sender, RoutedEventArgs e)
        {
            song?.SkipTo(((Slider)sender).Value);
        }

        public ReactiveCommand<Unit, Task> Open => ReactiveCommand.Create(OpenCommand);
        private async Task OpenCommand()
        {
            var dialog = new OpenFileDialog();
            if (dialog == null || Window == null) return;

            dialog.Filters?.Add(new FileDialogFilter() { Name = "Audio Files", Extensions = { "wav", "mp3", "flac", "webm" } });
            dialog.Filters?.Add(new FileDialogFilter() { Name = "All Files", Extensions = { "*" } });
            dialog.AllowMultiple = true;

            var filenames = await dialog.ShowAsync(Window);
            if (filenames != null)
            {
                song = new Song(filenames[0]);
                Name = song.Name;
                Slider = 0.0f;
                TimePassed = 0;
                TimeRemaining = Length = song.Length;
                song.TimeChanged += () => { 
                    TimePassed = song.TimePassed; 
                    TimeRemaining = Length - TimePassed;
                    Slider = (float)TimePassed / Length * 100;
                };
                IsSpinning = false;
            }
        }

        public ReactiveCommand<Unit, Unit> Exit => ReactiveCommand.Create(ExitCommand);
        private void ExitCommand()
        {
            Environment.Exit(0);
        }

        public ReactiveCommand<Unit, Unit> Play => ReactiveCommand.Create(PlayCommand);
        private void PlayCommand()
        {
            song?.Play(ref isSpinning);
            this.RaisePropertyChanged("IsSpinning");
        }

        public ReactiveCommand<Unit, Unit> Volume => ReactiveCommand.CreateFromTask(async () =>
        {
            var volume = this;

            var result = await ShowDialog.Handle(volume);
        });
    }
}
