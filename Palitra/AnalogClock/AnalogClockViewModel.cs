using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Palitra
{
    public class AnalogClockViewModel : ViewModel
    {
        private readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private double hourX, hourY, minuteX, minuteY, secondX, secondY;


        public double HourX
        {
            get => hourX;
            set
            {
                hourX = value;
                OnPropertyChanged();
            }
        }

        public double HourY
        {
            get => hourY;
            set
            {
                hourY = value;
                OnPropertyChanged();
            }
        }

        public double MinuteX
        {
            get => minuteX;
            set
            {
                minuteX = value;
                OnPropertyChanged();
            }
        }

        public double MinuteY
        {
            get => minuteY;
            set
            {
                minuteY = value;
                OnPropertyChanged();
            }
        }

        public double SecondX
        {
            get => secondX;
            set
            {
                secondX = value;
                OnPropertyChanged(); 
            }
        }
        public double SecondY
        {
            get => secondY;
            set
            {
                secondY = value;
                OnPropertyChanged();
            }
        }

        public AnalogClockViewModel()
        {
            TimeSpan timerInterval = TimeSpan.FromMilliseconds(1);
            Timer timer = new Timer(UpdateClockPositions, null, timerInterval, timerInterval);
        }

        private void UpdateClockPositions(object state)
        {

            CancellationToken cancellationToken = cancellationTokenSource.Token;

            while (!cancellationToken.IsCancellationRequested)
            {
                DateTime currentTime = DateTime.Now;

                double hourAngle = Math.PI / 6 * (currentTime.Hour % 12 * (-1) - (double)currentTime.Minute / 60 - (double)currentTime.Second / 3600 + 3);
                double minuteAngle = Math.PI / 30 * (currentTime.Minute * (-1) - (double)currentTime.Second / 60 + 15);
                double secondAngle = Math.PI / 30 * (currentTime.Second * (-1) - (double)currentTime.Millisecond / 1000 + 15);

                dispatcher.Invoke(() =>
                {
                    HourX = 205 + 100 * Math.Cos(hourAngle);
                    HourY = 205 - 100 * Math.Sin(hourAngle);
                    MinuteX = 205 + 120 * Math.Cos(minuteAngle);
                    MinuteY = 205 - 120 * Math.Sin(minuteAngle);
                    SecondX = 205 + 140 * Math.Cos(secondAngle);
                    SecondY = 205 - 140 * Math.Sin(secondAngle);
                });

                Thread.Sleep(10);
            }
        }
    }
}

