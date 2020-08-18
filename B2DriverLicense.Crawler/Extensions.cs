using B2DriverLicense.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace B2DriverLicense.AppCrawler
{
    public static class Extensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
                => self?.Select((item, index) => (item, index)) ?? new List<(T, int)>();
        public static string DownloadImg(string url, string path)
        {
            using (var client = new WebClient())
            {
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var fileName = Path.GetFileName(url);
                    var filePath = path + fileName;
                    client.DownloadFile(url, filePath);
                    Console.WriteLine($"Image: {fileName} has been downloaded.");
                    return filePath;
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to download image.");
                    return null;
                }
            }
        }



    }
}
