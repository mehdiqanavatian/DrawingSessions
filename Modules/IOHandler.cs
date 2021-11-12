using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace yarasoftDrawingSessions.Modules
{
    static class IOHandler
    {
        private static List<FileInfo> files = null;
        private static List<FileInfo> filesShuffled = null;
        private static ObservableCollection<string> libraries = null;
        private static List<string> validextensions = new() { ".jpg", ".bmp", ".png", ".gif", ".jpeg" };
        private static Random range = new Random();
        private static int currentIndex = 0;

        static IOHandler()
        {
            RefreshLibraries();
        }

        public static void RefreshLibraries()
        {
            libraries = SettingsHandler.LoadPathsFromStorage();
            LoadFilesFromLibraries();
        }

        public static bool CheckLibraries()
        {
            if (files != null)
            {
            return true;
            }
            else
            {
                return false;
            }
        }
        private static List<FileInfo> LoadFilesFromLibraries()
        {
            try
            {
                if (SettingsHandler.LoadIncludeSubFolders())
                {
                    foreach (String library in libraries)
                    {
                        DirectoryInfo directory = new(library);
                        if (files != null)
                        {
                            files = files.Concat(directory.GetFiles("*", SearchOption.AllDirectories).Where(file => validextensions.Contains(file.Extension.ToLower())).ToList()).ToList();
                        }
                        else
                        {
                            files = directory.GetFiles("*", SearchOption.AllDirectories).Where(file => validextensions.Contains(file.Extension.ToLower())).ToList();
                        }
                    }
                }
                else
                {
                    foreach (String library in libraries)
                    {
                        DirectoryInfo directory = new(library);
                        if (files != null)
                        {
                            files = files.Concat(directory.GetFiles().Where(file => validextensions.Contains(file.Extension.ToLower())).ToList()).ToList();
                        }
                        else
                        {
                            files = directory.GetFiles().Where(file => validextensions.Contains(file.Extension.ToLower())).ToList();
                        }
                    }
                }
                filesShuffled = files.ToList();
                filesShuffled.Shuffle();
                return files;
            }
            catch
            {
                return null;
            }
        }

        private static string LoadFileFromLibraries(List<FileInfo> files, int index)
        {
            if (files != null)
            {
                return files[index].FullName;
            }
            else
            {
                return null;
            }
        }
        private static string LoadFileFromLibraries(int index)
        {
            if (files != null)
            {
                return files[index].FullName;
            }
            else
            {
                return null;
            }
        }
        private static string LoadFileFromShuffledLibraries(int index)
        {
            return LoadFileFromLibraries(filesShuffled, index); ;
        }
        private static string LoadRandomFileFromLibraries()
        {
            Random rnd = new();
            return LoadFileFromLibraries(files, rnd.Next(files.Count - 1));
        }
        public static Bitmap LoadCurrentImageAsBitmap()
        {
            if (SettingsHandler.LoadShuffleState())
            {
                return new Bitmap(LoadFileFromShuffledLibraries(currentIndex));
            }
            else
            {

                return new Bitmap(LoadFileFromLibraries(currentIndex));
            }
        }
        public static BitmapImage LoadCurrentImageAsBitmapImage()
        {
            if (SettingsHandler.LoadShuffleState())
            {
                return new BitmapImage(new Uri(LoadFileFromShuffledLibraries(currentIndex)));
            }
            else
            {
                return new BitmapImage(new Uri(LoadFileFromLibraries(currentIndex)));
            }
        }
        public static Bitmap LoadNextImageAsBitmap()
        {
            if (currentIndex + 1 < files.Count)
            {
                ++currentIndex;

            }
            else
            {
                currentIndex = 0;
            }
            return LoadCurrentImageAsBitmap();
        }
        public static BitmapImage LoadNextImageAsBitmapImage()
        {
            if (currentIndex + 1 < files.Count)
            {
                ++currentIndex;

            }
            else
            {
                currentIndex = 0;
            }
            return LoadCurrentImageAsBitmapImage();
        }
        public static Bitmap LoadPreviousImageAsBitmap()
        {
            if (currentIndex - 1 >= 0)
            {
                --currentIndex;
            }
            else
            {
                currentIndex = files.Count - 1;
            }
            return LoadCurrentImageAsBitmap();
        }
        public static BitmapImage LoadPreviousImageAsBitmapImage()
        {
            if (currentIndex - 1 >= 0)
            {
                --currentIndex;
            }
            else
            {
                currentIndex = files.Count - 1;
            }
            return LoadCurrentImageAsBitmapImage();
        }
    }
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }

    static class MyExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}