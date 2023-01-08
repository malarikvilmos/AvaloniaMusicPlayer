using NAudio.Wave;
using System;
using System.Timers;

namespace MusicPlayerApp.Models
{
    public class Song
    {
        public string? Name { get; set; }
        public int Length { get; set; }
        public int TimePassed { get; set; }

        private static WaveOutEvent outputDevice = new();

        private static AudioFileReader audioFile;

        private string path;

        private Timer? timer;

        private static int volume = 100;

        public Song(string path) => Reset(path);

        ~Song() => timer?.Dispose();

        public void Play(ref bool isSpinning)
        {
            if (outputDevice.PlaybackState != PlaybackState.Playing)
            {
                if (outputDevice.PlaybackState == PlaybackState.Stopped)
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Volume = 1.00f;
                } 
                outputDevice.Play();
                isSpinning = true;

                if (timer == null)
                {
                    timer = new();
                    timer.Interval = 1000;
                    timer.Elapsed += (source, args) => {
                        TimePassed = (int)audioFile.CurrentTime.TotalSeconds;
                        TimeChanged?.Invoke();
                    };
                }
                timer?.Start();
                outputDevice.PlaybackStopped += (source, args) => { Reset(path); };
            }
            else if (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Pause();
                isSpinning = false;
                if (timer is not null)
                    timer.Enabled = false;
            }
        }

        public void SkipTo(double? value)
        {
            long position = (long)(audioFile.Length * value);
            audioFile.Position = position / 100;
        }

        public static void SetVolume(int value)
        {
            volume = value;
            if (audioFile != null)
                audioFile.Volume = value * 0.01f;
        }

        private void Reset(string path)
        {
            outputDevice.Dispose();
            outputDevice = new();

            audioFile = new AudioFileReader(path);
            int index = audioFile.FileName.LastIndexOfAny(new char[] { '\\', '/' });
            this.path = path;
            Name = audioFile.FileName[(index + 1)..];
            Length = (int)audioFile.TotalTime.TotalSeconds;
            TimePassed = audioFile.CurrentTime.Seconds;

            SetVolume(volume);
        }

        public event Action? TimeChanged; 
    }
}
