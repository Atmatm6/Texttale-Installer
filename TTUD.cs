using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace Texttale_Installer
{
    public partial class TexttaleUpdater : Form
    {
        private string downloadLocation = Path.Combine(Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)), "Texttale");
        public TexttaleUpdater()
        {
            InitializeComponent();
            pictureBox1.Image = Program.ScaleImage(
                (Image) new ImageConverter().ConvertFrom(new WebClient().DownloadData("http://arbituaryotter.bitballoon.com/def.ico")),
                pictureBox1.Width,
                pictureBox1.Height);
            if (File.Exists(Path.Combine(downloadLocation, "Texttale.exe")))
            {
                new NotImplementedException();
            }
            else
            {
                new NotImplementedException();
            }
        }

        public string DownloadLocation
        {
            get
            {
                return downloadLocation;
            }

            set
            {
                downloadLocation = value;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Program.WebStart("http://texttale.ga");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                DownloadLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            }
            else
            {
                DownloadLocation = Path.Combine(Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)),"Texttale");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new NotImplementedException();
        }
    }
}
