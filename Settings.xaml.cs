using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace yarasoftDrawingSessions
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        ObservableCollection<string> libraries = new();
        public Settings()
        {
            InitializeComponent();
            //AddIntervalComponents();
            if (Modules.SettingsHandler.LoadPathsFromStorage() != null)
            {
                libraries = Modules.SettingsHandler.LoadPathsFromStorage();
                PopulateFolderLibraryList();
            }
            else
            {
                MessageBox.Show("Failed To Load Libraries");
            }

            CheckBoxLastWindowPosition.IsChecked = Modules.SettingsHandler.LoadRememberLastWindowPosition();
            CheckBoxLastWindowSize.IsChecked = Modules.SettingsHandler.LoadRememberLastWindowSize();
            CheckBoxStayOnTop.IsChecked = Modules.SettingsHandler.LoadAlwaysOnTop();
            CheckBoxIncludeSubfolders.IsChecked = Modules.SettingsHandler.LoadIncludeSubFolders();
            CheckBoxShuffleState.IsChecked = Modules.SettingsHandler.LoadShuffleState();

            this.Topmost = Modules.SettingsHandler.LoadAlwaysOnTop();
        }

        private void ButtonAddPhotoLibrary_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new();

            System.Windows.Forms.DialogResult result = fbd.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("Failed to select a folder");
            }
            else
            {
                libraries.Add(fbd.SelectedPath);
            }
        }

        private void ButtonSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            Modules.SettingsHandler.SavePathsToStorage(libraries);
            Modules.SettingsHandler.SaveSlideIntervals(TimeSpanUpDownSlideDuration.Duration);
            Modules.SettingsHandler.SaveSlideGapIntervals(TimeSpanUpDownSlideGapDuration.Duration);
            Modules.SettingsHandler.SaveSettings();
            Modules.IOHandler.RefreshLibraries();
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private bool PopulateFolderLibraryList()
        {
            try
            {
                listViewPhotoLibrary.ItemsSource = libraries;
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        private void RemoveFolder_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string context = button.DataContext as string;
            libraries.Remove(context);
        }


        private void CheckBoxLastWindowPosition_Checked(object sender, RoutedEventArgs e)
        {
            Modules.SettingsHandler.SaveRememberLastWindowPosition(true);
        }

        private void CheckBoxLastWindowSize_Checked(object sender, RoutedEventArgs e)
        {
            Modules.SettingsHandler.SaveRememberLastWindowSize(true);
        }

        private void CheckBoxStayOnTop_Checked(object sender, RoutedEventArgs e)
        {
            Modules.SettingsHandler.SaveAlwaysOnTop(true);
        }

        private void CheckBoxIncludeSubfolders_Checked(object sender, RoutedEventArgs e)
        {
            Modules.SettingsHandler.SaveIncludeSubFolders(true);
        }

        private void CheckBoxStayOnTop_Unchecked(object sender, RoutedEventArgs e)
        {
            Modules.SettingsHandler.SaveAlwaysOnTop(false);
        }

        private void CheckBoxIncludeSubfolders_Unchecked(object sender, RoutedEventArgs e)
        {
            Modules.SettingsHandler.SaveIncludeSubFolders(false);
        }

        private void CheckBoxLastWindowSize_Unchecked(object sender, RoutedEventArgs e)
        {
            Modules.SettingsHandler.SaveRememberLastWindowSize(false);
        }

        private void CheckBoxLastWindowPosition_Unchecked(object sender, RoutedEventArgs e)
        {
            Modules.SettingsHandler.SaveRememberLastWindowPosition(false);
        }


        private void TimeSpanUpDownSlideDuration_Loaded(object sender, RoutedEventArgs e)
        {
            TimeSpanUpDownSlideDuration.InitTimeSpan(Modules.SettingsHandler.LoadSlideIntervals());
        }

        private void CheckBoxShuffleState_Checked(object sender, RoutedEventArgs e)
        {
            Modules.SettingsHandler.SaveShuffleState(true);

        }

        private void CheckBoxShuffleState_Unchecked(object sender, RoutedEventArgs e)
        {
            Modules.SettingsHandler.SaveShuffleState(false);
        }

        private void TimeSpanUpDownSlideGapDuration_Loaded(object sender, RoutedEventArgs e)
        {
            TimeSpanUpDownSlideGapDuration.InitTimeSpan(Modules.SettingsHandler.LoadSlideGapIntervals());
        }
    }
}
