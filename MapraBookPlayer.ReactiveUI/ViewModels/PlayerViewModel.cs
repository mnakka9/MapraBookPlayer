using System;
using System.Collections.ObjectModel;
using System.Reactive;

using MapraBookPlayer.Domain;

using NAudio.Wave;

using ReactiveUI;

namespace MapraBookPlayer.ReactiveUI.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        public AudioBookViewModel? AudioBook { get; set; }

        private int selectedIndex = 0;
        public int SelectedIndex
        {
            get => selectedIndex;
            set => this.RaiseAndSetIfChanged(ref selectedIndex, value);
        }

        private ObservableCollection<Bookmark> _bookmarks = [];
        public ObservableCollection<Bookmark> Bookmarks
        {
            get => _bookmarks;
            set => this.RaiseAndSetIfChanged(ref _bookmarks, value);
        }

        private WaveOutEvent? _waveOut;
        private AudioFileReader? _audioFileReader;

        private bool _playFilled;
        private bool _pauseFilled;
        private bool _stopFilled;

        public bool PlayFilled
        {
            get => _playFilled;
            set => this.RaiseAndSetIfChanged(ref _playFilled, value);
        }

        public bool PauseFilled
        {
            get => _pauseFilled;
            set => this.RaiseAndSetIfChanged(ref _pauseFilled, value);
        }

        public bool StopFilled
        {
            get => _stopFilled;
            set => this.RaiseAndSetIfChanged(ref _stopFilled, value);
        }

        private double _position;
        public double Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    this.RaiseAndSetIfChanged(ref _position, value);

                    if (_audioFileReader != null)
                    {
                        _audioFileReader.CurrentTime = TimeSpan.FromSeconds(Position);
                    }
                }
            }
        }

        public ReactiveCommand<double, Unit> SeekCommand { get; }

        private void Seek (double newPosition)
        {
            Position = newPosition;
            _audioFileReader!.CurrentTime = TimeSpan.FromSeconds(Position);
        }

        public Chapter? CurrentChapter { get; set; }

        public PlayerViewModel ()
        {
            PlayCommand = ReactiveCommand.Create(Play);
            StopCommand = ReactiveCommand.Create(Stop);
            PauseCommand = ReactiveCommand.Create(Pause);
            SeekCommand = ReactiveCommand.Create<double>(Seek);
        }

        private double _audioFileLength;
        public double AudioFileLength
        {
            get => _audioFileLength;
            set
            {
                if (_audioFileLength != value)
                {
                    this.RaiseAndSetIfChanged(ref _audioFileLength, value);
                }
            }
        }

        public ReactiveCommand<Unit, Unit> PlayCommand { get; }
        public ReactiveCommand<Unit, Unit> StopCommand { get; }
        public ReactiveCommand<Unit, Unit> PauseCommand { get; }

        private void Play ()
        {
            if (CurrentChapter == null)
            {
                return;
            }

            PauseFilled = false;
            PlayFilled = true;
            StopFilled = false;

            _waveOut ??= new WaveOutEvent();

            if (_audioFileReader == null)
            {
                _audioFileReader = new AudioFileReader(CurrentChapter.Path)
                {
                    Position = (long)Position
                };
                AudioFileLength = _audioFileReader.TotalTime.TotalSeconds;
                _waveOut.Init(_audioFileReader);
                _waveOut.PlaybackStopped += (sender, e) => PlayNextChapter();
                _waveOut.Play();
            }
            else
            {
                _waveOut.Play();
            }
        }

        private void PlayNextChapter ()
        {
            //var chapterList = AudioBook!.Chapters.ToList();
            //var currentIndex = chapterList.IndexOf(CurrentChapter!);
            //if (currentIndex > -1 && currentIndex < chapterList.Count)
            //{
            //    SelectedIndex = currentIndex + 1;
            //}
            //else
            //{
            //    SelectedIndex = 0;
            //}
        }

        public void StopPlaying ()
        {
            if (_waveOut != null)
            {
                this.Stop();
            }
        }

        public void StartPlaying ()
        {
            this.Play();
        }

        private void Stop ()
        {
            if (_waveOut != null && _waveOut.PlaybackState == PlaybackState.Playing)
            {
                PauseFilled = false;
                PlayFilled = false;
                StopFilled = true;
                if (_waveOut != null)
                {
                    _waveOut.Stop();
                    _waveOut.Dispose();
                    _waveOut = null;
                }
                if (_audioFileReader != null)
                {
                    _audioFileReader.Dispose();
                    _audioFileReader = null;
                }
                Position = 0;
                PauseFilled = false;
                PlayFilled = false;
                StopFilled = false;
            }
        }

        private void Pause ()
        {
            PauseFilled = true;
            PlayFilled = false;
            StopFilled = false;
            _waveOut?.Pause();
        }
    }
}