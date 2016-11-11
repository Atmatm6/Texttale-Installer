using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Texttale_Installer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TexttaleUpdater());
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public static void WebStart(string link)
        {
            string browserPath = GetBrowserPath();
            if (browserPath == string.Empty)
                browserPath = "iexplore";
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(browserPath);
            process.StartInfo.Arguments = "\"" + link + "\"";
            process.Start();
        }

        private static string GetBrowserPath()
        {
            string browser = string.Empty;
            RegistryKey key = null;

            try
            {
                // try location of default browser path in XP
                key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                // try location of default browser path in Vista
                if (key == null)
                {
                    key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http", false); ;
                }

                if (key != null)
                {
                    //trim off quotes
                    browser = key.GetValue(null).ToString().ToLower().Replace("\"", "");
                    if (!browser.EndsWith("exe"))
                    {
                        //get rid of everything after the ".exe"
                        browser = browser.Substring(0, browser.LastIndexOf(".exe") + 4);
                    }

                    key.Close();
                }
            }
            catch
            {
                return string.Empty;
            }

            return browser;
        }
    }
}
