using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Palitra
{
    public class DigitalClockViewModel : ViewModel
    {
        private readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Dictionary<int, string> numPathCode = new Dictionary<int, string>()
        {
            { 1, "M 90 10 L 90 190" },
            { 2, "M 10 10 L 90 10 M 90 10 L 90 100 M 90 100 L 10 100 M 10 100 L 10 190 M 10 190 L 90 190" },
            { 3, "M 10 10 L 90 10 M 90 10 L 90 100 M 90 100 L 10 100 M 90 100 L 90 190 M 90 190 L 10 190" },
            { 4, "M 10 10 L 10 100 M 90 10 L 90 190 M 90 100 L 10 100" },
            { 5, "M 10 10 L 90 10 M 10 10 L 10 100 M 10 100 L 90 100 M 90 100 L 90 190 M 90 190 L 10 190" },
            { 6, "M 10 10 L 90 10 M 10 10 L 10 190 M 10 100 L 90 100 M 10 190 L 90 190 M 90 100 L 90 190" },
            { 7, "M 10 10 L 90 10 M 90 10 L 90 190" },
            { 8, "M 10 10 L 10 190 M 90 10 L 90 190 M 10 100 L 90 100 M 10 10 L 90 10 M 10 190 L 90 190" },
            { 9, "M 10 10 L 10 100 M 90 10 L 90 190 M 10 100 L 90 100 M 10 10 L 90 10 M 10 190 L 90 190" },
            { 0, "M 10 10 L 10 190 M 10 10 L 90 10 M 90 10 L 90 190 M 90 190 L 10 190" }
        };
        private string hourLeft, hourRight, minuteLeft, minuteRight, secondLeft, secondRight;

        public string HourLeft
        {
            get => hourLeft;
            set
            {
                hourLeft = value;
                OnPropertyChanged();
            }
        }

        public string HourRight
        {
            get => hourRight;
            set
            {
                hourRight = value;
                OnPropertyChanged();
            }
        }

        public string MinuteLeft
        {
            get => minuteLeft; 
            set
            {
                minuteLeft = value;
                OnPropertyChanged();
            }
        }

        public string MinuteRight
        {
            get => minuteRight; 
            set
            {
                minuteRight = value;
                OnPropertyChanged();
            }
        }

        public string SecondLeft
        {
            get => secondLeft; 
            set
            {
                secondLeft = value;
                OnPropertyChanged();
            }
        }

        public string SecondRight
        {
            get => secondRight; 
            set
            {
                secondRight = value;
                OnPropertyChanged();
            }
        }

        public DigitalClockViewModel()
        {
            var timerInterval = TimeSpan.FromMilliseconds(1);
            var timer = new Timer(UpdateClockPositions, null, timerInterval, timerInterval);
        }

        private void UpdateClockPositions(object state)
        {

            CancellationToken cancellationToken = cancellationTokenSource.Token;

            while (!cancellationToken.IsCancellationRequested)
            {
                DateTime currentTime = DateTime.Now;

                dispatcher.Invoke(() =>
                {
                    HourLeft = numPathCode[currentTime.Hour / 10];
                    HourRight = numPathCode[currentTime.Hour % 10];
                    MinuteLeft = numPathCode[currentTime.Minute / 10];
                    MinuteRight = numPathCode[currentTime.Minute % 10];
                    SecondLeft = numPathCode[currentTime.Second / 10];
                    SecondRight = numPathCode[currentTime.Second % 10];
                });

                Thread.Sleep(10);
            }
        }

    }
}
