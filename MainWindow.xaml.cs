using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
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
using System.Windows.Threading;

namespace yarasoftDrawingSessions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Settings settings = new();
        TimeSpan slideInterval = TimeSpan.Zero;
        TimeSpan slideGapInterval = TimeSpan.Zero;

        public MainWindow()
        {
            InitializeComponent();
            LoadLastWindowSize();
            LoadLastWindowPosition();

            buttonPause.Content = Modules.SettingsHandler.SlideshowPlayback() ? "Pause" : "Play";
            InitSlideInterval();
        }
        private void InitSlideInterval()
        {
            slideInterval = Modules.SettingsHandler.LoadSlideIntervals();
            slideGapInterval = Modules.SettingsHandler.LoadSlideGapIntervals();

            UpdateLabelTimerOverlay(slideInterval);
        }
        private void UpdateLabelTimerOverlay(TimeSpan interval)
        {
            labelTimerOverlay.Content = interval.ToString("c");
        }
        private void LoadLastWindowPosition()
        {
            if (Modules.SettingsHandler.LoadRememberLastWindowPosition())
            {
                Modules.SettingsHandler.Coordinate coordinates = Modules.SettingsHandler.LoadLastWindowPosition();
                this.Top = coordinates.top;
                this.Left = coordinates.left;
            }
        }

        private void LoadLastWindowSize()
        {
            if (Modules.SettingsHandler.LoadRememberLastWindowSize())
            {
                Modules.SettingsHandler.WindowSize windowSize = Modules.SettingsHandler.LoadLastWindowSize();
                this.Height = windowSize.height;
                this.Width = windowSize.width;
                if (windowSize.windowMaximized)
                {
                    WindowState = WindowState.Maximized;
                }
            }
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (Modules.IOHandler.CheckLibraries())
            {
                buttonPlay.Visibility = Visibility.Hidden;
                PrepTimer();
                UpdateImage();
            }
            else
            {
                MessageBox.Show("Add a directory with images first");
                ShowSettings();
            }
        }

        private void PrepTimer()
        {
            DispatcherTimer dispatcherTimer = new();

            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);

            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Modules.SettingsHandler.SlideshowPlayback())
            {
                if (slideInterval != TimeSpan.Zero)
                {
                    slideInterval = slideInterval.Add(new TimeSpan(0, 0, -1));
                    UpdateLabelTimerOverlay(slideInterval);
                }
                else
                {
                    if (slideGapInterval != TimeSpan.Zero)
                    {
                        slideGapInterval = slideGapInterval.Add(new TimeSpan(0, 0, -1));
                        UpdateLabelTimerOverlay(slideGapInterval);
                    }
                    else
                    {
                        NextImage();
                    }
                }
            }
        }
        private void UpdateImage()
        {
            if (checkBox.IsChecked.Value)
            {
                Bitmap bmp = Modules.ColorFilters.DrawAsGrayscale(Modules.IOHandler.LoadCurrentImageAsBitmap());
                image.Source = Modules.ColorFilters.ToBitmapImage(bmp);
            }
            else
            {
                image.Source = Modules.IOHandler.LoadCurrentImageAsBitmapImage();
            }
        }

        private void CurrentImage()
        {
            Modules.IOHandler.LoadCurrentImageAsBitmapImage();
            UpdateImage();
        }

        private void NextImage()
        {
            Modules.IOHandler.LoadNextImageAsBitmapImage();
            UpdateImage();
            InitSlideInterval();
        }
        private void PreviousImage()
        {
            Modules.IOHandler.LoadPreviousImageAsBitmapImage();
            UpdateImage();
        }


        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            NextImage();
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            PreviousImage();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = Modules.SettingsHandler.LoadAlwaysOnTop();
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            if (buttonPause.Content.ToString() != "Pause")
            {
                buttonPause.Content = "Pause";
                Modules.SettingsHandler.SaveSlideshowState(true);
            }
            else
            {
                buttonPause.Content = "Play";
                Modules.SettingsHandler.SaveSlideshowState(false);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveLastWindowProps();
            settings.Close();
        }

        private void SaveLastWindowProps()
        {
            Modules.SettingsHandler.Coordinate coords = new();
            Modules.SettingsHandler.WindowSize windowSize = new();

            if (WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen                
                coords.top = RestoreBounds.Top;
                coords.left = RestoreBounds.Left;
                windowSize.height = RestoreBounds.Height;
                windowSize.width = RestoreBounds.Width;
                windowSize.windowMaximized = true;
            }
            else
            {
                coords.top = this.Top;
                coords.left = this.Left;
                windowSize.height = this.Height;
                windowSize.width = this.Width;
                windowSize.windowMaximized = false;
            }
            Modules.SettingsHandler.SaveLastWindowPosition(coords);
            Modules.SettingsHandler.SaveLastWindowSize(windowSize);
            Modules.SettingsHandler.SaveSettings();
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            ShowSettings();
        }

        private void ShowSettings()
        {
            try
            {
                settings.Show();
            }
            catch
            {
                settings = new Settings(); settings.Show();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateImage();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateImage();
        }

        private void ShowControlPanel()
        {
            if (GridControlPanel.Height != 70)
                GridControlPanel.Height = 70;
        }
        private void HideControlPanel()
        {
            if (GridControlPanel.Height != 0)
                GridControlPanel.Height = 0;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.GetPosition(GridControlPanel).Y > -50)
            {
                ShowControlPanel();
            }
            else
            {
                HideControlPanel();
            }
        }
    }
}

