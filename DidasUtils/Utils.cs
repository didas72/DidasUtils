using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace DidasUtils
{
    /// <summary>
    /// Class that contains several genereral-purpose methods.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Gets all files in the given directory and it's subdirectories.
        /// </summary>
        /// <param name="directory">The full path of the directory to be checked.</param>
        /// <returns>Array containing the full paths of every file found.</returns>
        public static string[] GetSubFiles(string directory) => GetSubFiles(directory, string.Empty);
        /// <summary>
        /// Gets all files in the given directory and it's subdirectories that match the given search pattern.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="searchPattern"></param>
        /// <returns>Array containing the full paths of every file found.</returns>
        public static string[] GetSubFiles(string directory, string searchPattern)
        {
            List<string> files = new List<string>();

            if (string.IsNullOrEmpty(searchPattern))
            {
                files.AddRange(Directory.GetFiles(directory));

                foreach (string d in Directory.GetDirectories(directory))
                    files.AddRange(GetSubFiles(d));
            }
            else
            {
                files.AddRange(Directory.GetFiles(directory, searchPattern));

                foreach (string d in Directory.GetDirectories(directory))
                    files.AddRange(GetSubFiles(d, searchPattern));
            }

            return files.ToArray();
        }
        /// <summary>
        /// Gets all files in the given directory and it's subdirectories that match the given search pattern, sorted by containing directory name.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="searchPattern"></param>
        /// <returns>Array containing the full paths of every file found.</returns>
        public static string[] GetSubFilesSorted(string directory, string searchPattern)
        {
            List<string> files = new List<string>();

            files.AddRange(Directory.GetFiles(directory, searchPattern));

            string[] dirs = Directory.GetDirectories(directory);
            dirs = SortDirs(dirs);

            foreach (string d in dirs)
                files.AddRange(GetSubFiles(d, searchPattern));

            return files.ToArray();
        }
        /// <summary>
        /// Sorts file paths by their numeric values. Values must be ulong compatible.
        /// </summary>
        /// <param name="filenames">The paths to sort by filename.</param>
        /// <returns>Sorted paths array.</returns>
        public static string[] SortFiles(string[] filenames)
        {
            Dictionary<ulong, string> files = new Dictionary<ulong, string>();
            List<string> sorted = new List<string>();

            for (int i = 0; i < filenames.Length; i++)
            {
                files.Add(ulong.Parse(Path.GetFileNameWithoutExtension(filenames[i])), filenames[i]);
            }

            while (files.Count > 0)
            {
                ulong lowest = ulong.MaxValue;

                foreach (KeyValuePair<ulong, string> pair in files)
                {
                    if (pair.Key < lowest)
                        lowest = pair.Key;
                }

                sorted.Add(files[lowest]);
                files.Remove(lowest);
            }

            return sorted.ToArray();
        }
        /// <summary>
        /// Sorts directory paths by their numeric values. Values must be ulong compatible.
        /// </summary>
        /// <param name="dirnames">The paths to sort by directory name.</param>
        /// <returns>Sorted paths array.</returns>
        public static string[] SortDirs(string[] dirnames)
        {
            Dictionary<ulong, string> dirs = new Dictionary<ulong, string>();
            List<string> sorted = new List<string>();

            for (int i = 0; i < dirnames.Length; i++)
            {
                dirs.Add(ulong.Parse(Path.GetFileName(dirnames[i])), dirnames[i]);
            }

            while (dirs.Count > 0)
            {
                ulong lowest = ulong.MaxValue;

                foreach (KeyValuePair<ulong, string> pair in dirs)
                {
                    if (pair.Key < lowest)
                        lowest = pair.Key;
                }

                sorted.Add(dirs[lowest]);
                dirs.Remove(lowest);
            }

            return sorted.ToArray();
        }



        /// <summary>
        /// Gets the path of the currently running .exe.
        /// </summary>
        /// <returns></returns>
        public static string GetExecutablePath()
        {
            return Assembly.GetEntryAssembly().Location;
        }



        /// <summary>
        /// Formats a size, in bytes, to a smaller, humanely readable value with two decimal places.
        /// </summary>
        /// <param name="size">The size, in bytes, to format.</param>
        public static string FormatSize(long size)
        {
            if (size < 1024) return $"{size}B";

            if (size < 1048576) return $"{(size / 1024f):N2}kB";

            if (size < 1073741824) return $"{(size / 1048576f):N2}MB";

            if (size < 1099511627776) return $"{(size / 1073741824f):N2}GB";

            return $"{(size / 1099511627776):N2}TB";
        }



        /// <summary>
        /// Waits for a key to be pressed while in console.
        /// </summary>
        public static void WaitForKey()
        {
            Console.ReadKey(true);
        }
    }
}
