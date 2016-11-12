using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Text;
//If anyone sees this, understand that I am only half sure what I am writing is correct.
namespace Texttale_Installer
{
    public partial class TexttaleUpdater : Form
    {
        private WebClient wc = new WebClient();
        private string downloadLocation = Path.Combine(Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)), "Texttale");
        public TexttaleUpdater()
        {
            InitializeComponent();
            pictureBox1.Image = Program.ScaleImage(
                (Image) new ImageConverter().ConvertFrom(wc.DownloadData("http://arbituaryotter.bitballoon.com/def.ico")),
                pictureBox1.Width,
                pictureBox1.Height);
            CheckForUpdates();
        }

        private bool InDL()
        {
            if (File.Exists(Path.Combine(downloadLocation, "Texttale.exe")) && InDL()) {
                return true;
            } else {
                return false;
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

        public bool CheckForUpdates()
        {
            if (Convert.ToInt32(File.ReadAllText(Path.Combine(downloadLocation, "buildnum"))).Equals(Convert.ToInt32(wc.DownloadString("http://texttale.ga/upload/buildnum")))){
                return false;
            }
            else {
                return true;
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
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
            CheckForUpdates();
        }

        private void DownloadOrUpdate(bool isUpdate)
        {
            string buildnum = wc.DownloadString("http://texttale.ga/upload/buildnum");
            if (isUpdate)
            {
                File.Delete(Path.Combine(downloadLocation, "Texttale.exe"));
                File.Delete(Path.Combine(downloadLocation, "SlimDX.dll"));
                File.Delete(Path.Combine(downloadLocation, "buildnum"));
                wc.DownloadFile("http://texttale.ga/upload/" + buildnum + "/Texttale.exe", Path.Combine(downloadLocation, "Texttale.exe"));
                wc.DownloadFile("http://texttale.ga/upload/" + buildnum + "/SlimDX.dll", Path.Combine(downloadLocation, "SlimDX.dll"));
                wc.DownloadFile("http://texttale.ga/upload/buildnum", Path.Combine(downloadLocation, "buildnum"));
                MessageBox.Show("The update to build number " + buildnum + " is complete.","Done Updating.", MessageBoxButtons.OK);
            } else {
                wc.DownloadFile("http://texttale.ga/upload/" + buildnum + "/Texttale.exe", Path.Combine(downloadLocation, "Texttale.exe"));
                wc.DownloadFile("http://texttale.ga/upload/" + buildnum + "/SlimDX.dll", Path.Combine(downloadLocation, "SlimDX.dll"));
                wc.DownloadFile("http://texttale.ga/upload/buildnum", Path.Combine(downloadLocation, "buildnum"));
                button1.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DownloadOrUpdate(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CheckForUpdates())
            {
                DownloadOrUpdate(true);
            }
        }
    }
}
