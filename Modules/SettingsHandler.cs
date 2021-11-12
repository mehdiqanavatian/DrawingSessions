using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace yarasoftDrawingSessions.Modules
{
    static class SettingsHandler
    {
        public struct Coordinate
        {
            public double top;
            public double left;

            public Coordinate(double top, double left)
            {
                this.top = top;
                this.left = left;
            }
        }
        public struct WindowSize
        {
            public double width;
            public double height;
            public bool windowMaximized;
            public WindowSize(double width, double height,bool windowMaximized)
            {
                this.width = width;
                this.height = height;
                this.windowMaximized = windowMaximized;
            }
        }
        internal static ObservableCollection<string> LoadPathsFromStorage()
        {
            try
            {
                ObservableCollection<string> libraries = new(Properties.Settings.Default.Paths.Split(new char[] { ';' }));
                libraries = new ObservableCollection<string>(libraries.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList());
                return libraries;
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static void SavePathsToStorage(ObservableCollection<string> libraries)
        {
            string savelibraries = "";
            foreach (string library in libraries)
            {
                savelibraries += library + ";";

            }
            Properties.Settings.Default.Paths = savelibraries;
        }

        internal static void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        internal static void SaveRememberLastWindowPosition(bool data)
        {
            Properties.Settings.Default.RememberLastWindowPosition = data;
        }
        internal static void SaveRememberLastWindowSize(bool data)
        {
            Properties.Settings.Default.RememberLastWindowSize = data;
        }

        internal static void SaveSlideIntervals()
        {
            throw new NotImplementedException();
        }

        internal static void SaveAlwaysOnTop(bool data)
        {
            Properties.Settings.Default.isTopMost = data;
        }
        internal static void SaveIncludeSubFolders(bool data)
        {
            Properties.Settings.Default.IncludeSubFolders = data;
        }

        internal static bool LoadRememberLastWindowPosition()
        {
            return Properties.Settings.Default.RememberLastWindowPosition;
        }
        internal static bool LoadRememberLastWindowSize()
        {
            return Properties.Settings.Default.RememberLastWindowSize;
        }
        internal static bool LoadAlwaysOnTop()
        {
            return Properties.Settings.Default.isTopMost;
        }
        internal static bool LoadIncludeSubFolders()
        {
            return Properties.Settings.Default.IncludeSubFolders;
        }

        internal static TimeSpan LoadSlideIntervals()
        {
            return Properties.Settings.Default.slideInterval;
        }

        internal static void SaveSlideIntervals(TimeSpan interval)
        {
            Properties.Settings.Default.slideInterval = interval;
        }

        internal static TimeSpan LoadSlideGapIntervals()
        {
            return Properties.Settings.Default.SlideGapInterval;
        }

        internal static void SaveSlideGapIntervals(TimeSpan interval)
        {
            Properties.Settings.Default.SlideGapInterval = interval;
        }


        internal static Coordinate LoadLastWindowPosition()
        {
            Coordinate coordinates = new();

            coordinates.top = Properties.Settings.Default.WindowTop;
            coordinates.left = Properties.Settings.Default.WindowLeft;

            return coordinates;
        }
        internal static void SaveLastWindowPosition(Coordinate coordinates)
        {
            Properties.Settings.Default.WindowTop = coordinates.top;
            Properties.Settings.Default.WindowLeft = coordinates.left;
        }

        internal static WindowSize LoadLastWindowSize()
        {
            WindowSize windowSize = new();
            windowSize.height = Properties.Settings.Default.WindowHeight;
            windowSize.width = Properties.Settings.Default.WindowWidth;
            windowSize.windowMaximized = Properties.Settings.Default.WindowMaximized;
            return windowSize;
        }
        internal static void SaveLastWindowSize(WindowSize windowSize)
        {
            Properties.Settings.Default.WindowHeight = windowSize.height;
            Properties.Settings.Default.WindowWidth = windowSize.width;
            Properties.Settings.Default.WindowMaximized = windowSize.windowMaximized;
        }

        internal static void SaveSlideshowState(bool data)
        {
            Properties.Settings.Default.isSliding = data;
        }

        internal static bool SlideshowPlayback()
        {
            return Properties.Settings.Default.isSliding;
        }

        internal static void SaveShuffleState(bool data)
        {
            Properties.Settings.Default.useShuffledList = data;
        }
        internal static bool LoadShuffleState()
        {
            return Properties.Settings.Default.useShuffledList;
        }
    }
}
