using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace yarasoftDrawingSessions.UserControls
{
    /// <summary>
    /// Interaction logic for TimeSpanUpDown.xaml
    /// </summary>
    public partial class TimeSpanUpDown : UserControl
    {
        private const int constantHour = 2;
        private const int constantMinute = 1;
        private const int constantSecond = 0;

        private int lastText;

        private TimeSpan duration;
        private double totalSeconds;

        public TimeSpan Duration { get => duration; }

        public TimeSpanUpDown()
        {
            InitializeComponent();
        }

        public void InitTimeSpan(TimeSpan timeSpan)
        {
            duration = timeSpan;
            totalSeconds = Duration.TotalSeconds;
            txtHour.Text = Duration.Hours.ToString();
            txtMinute.Text = Duration.Minutes.ToString();
            txtSecond.Text = Duration.Seconds.ToString();
        }
        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            switch (lastText)
            {
                case constantHour:
                    IncreaseHour();
                    break;

                case constantMinute:
                    IncreaseMinute();
                    break;

                case constantSecond:
                    IncreaseSecond();
                    break;
            }
        }

        private void IncreaseHour()
        {

            totalSeconds += 3600;
            if (totalSeconds > 43200) totalSeconds = 43200;
            CalculateTime();
        }
        private void DecreaseHour()
        {
            if (totalSeconds > 3600)
            {
                totalSeconds -= 3600;
            }
            CalculateTime();
        }
        private void IncreaseMinute()
        {
            totalSeconds += 60;
            if (totalSeconds > 43200) totalSeconds = 43200;

            CalculateTime();
        }
        private void DecreaseMinute()
        {
            if (totalSeconds > 60)
            {
                totalSeconds -= 60;
            }
            CalculateTime();
        }
        private void IncreaseSecond()
        {
            totalSeconds++;
            if (totalSeconds > 43200) totalSeconds = 43200;
            CalculateTime();
        }
        private void DecreaseSecond()
        {
            if (totalSeconds > 0)
            {
                totalSeconds--;
            }
            CalculateTime();
        }
        private void CalculateTime()
        {
            duration = new TimeSpan(ticks: (long)(totalSeconds*10000000));
            txtHour.Text = Duration.Hours.ToString();
            txtMinute.Text = Duration.Minutes.ToString();
            txtSecond.Text = Duration.Seconds.ToString();
        }


        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            switch (lastText)
            {
                case constantHour:
                    DecreaseHour();
                    break;

                case constantMinute:
                    DecreaseMinute();                    
                    break;

                case constantSecond:
                    DecreaseSecond();
                    break;

            }
        }

        private void TxtSecond_GotFocus(object sender, RoutedEventArgs e)
        {
            lastText = constantSecond;
        }

        private void TxtMinute_GotFocus(object sender, RoutedEventArgs e)
        {
            lastText = constantMinute;
        }

        private void TxtHour_GotFocus(object sender, RoutedEventArgs e)
        {
            lastText = constantHour;
        }
    }
}
