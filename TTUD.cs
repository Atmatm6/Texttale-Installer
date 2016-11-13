using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading;
//If anyone sees this, understand that I am only half sure what I am writing is correct.
namespace Texttale_Installer
{
    public partial class TexttaleUpdater : Form
    {
        private WebClient wc = new WebClient();
        private string downloadLocation = Path.Combine(Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)), "Roaming");
        public TexttaleUpdater()
        {
            InitializeComponent();
            this.Visible = false;
            this.Icon = (Icon)new IconConverter().ConvertFrom(wc.DownloadData("http://arbituaryotter.bitballoon.com/def.ico"));
            pictureBox1.Image = Program.ScaleImage(
                (Image)new ImageConverter().ConvertFrom(wc.DownloadData("http://arbituaryotter.bitballoon.com/def.ico")),
                pictureBox1.Width,
                pictureBox1.Height);
            if (checkBox1.Checked) if (!Directory.Exists(DownloadLocation)) Directory.CreateDirectory(DownloadLocation);
            Thread thr = new Thread(new ThreadStart(UpdatingThread));
            if (TexttaleInstalled()) {
                button1.Enabled = false;
                button3.Enabled = true;
                if (CheckForUpdates()) button2.Enabled = true;
                else button2.Enabled = false;
            }
            else {
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
            }
            
            this.Visible = true;
        }

        public void DeleteFolder(bool isUninstalling) {
            DirectoryInfo di = new DirectoryInfo(new Uri(DownloadLocation).AbsolutePath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            if (isUninstalling) {
                Directory.Delete(new Uri(DownloadLocation).AbsolutePath);
            }
        }

        public void UpdatingThread() {
            bool sentUpdateBalloon = false;
            while (!sentUpdateBalloon) {
                if (CheckForUpdates())
                {
                    using (var no = new NotifyIcon())
                    {
                        no.BalloonTipTitle = "Texttale Updater";
                        no.BalloonTipText = "A new build of Texttale has come up:\nbuild " + Convert.ToString(Convert.ToInt32(wc.DownloadString("http://texttale.ga/upload/buildnum")));
                        no.BalloonTipIcon = ToolTipIcon.Info;
                        no.ShowBalloonTip(30000);
                        sentUpdateBalloon = true;
                    }
                } else
                {
                    Thread.Sleep(240000);
                }
            }
        }

        private bool TexttaleInstalled()
        {
            if (File.Exists(Path.Combine(DownloadLocation, "Texttale.exe"))) {
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
            checkBox1.Enabled = false;
            if (TexttaleInstalled())
            {
                if (Convert.ToInt32(File.ReadAllText(Path.Combine(DownloadLocation, "buildnum"))).Equals(Convert.ToInt32(wc.DownloadString("http://texttale.ga/upload/buildnum"))))
                {
                    checkBox1.Enabled = true;
                    button2.Enabled = false;
                    return false;
                }
                else
                {
                    checkBox1.Enabled = true;
                    button2.Enabled = true;
                    return true;
                }
            } else {
                checkBox1.Enabled = true;
                button2.Enabled = false;
                return false;
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
                DownloadLocation = Path.Combine(Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)),"Roaming","Texttale");
            }
            if (TexttaleInstalled())
            {
                button1.Enabled = false;
                button3.Enabled = true;
                if (CheckForUpdates()) button2.Enabled = true;
                else button2.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void DownloadOrUpdate(bool isUpdate)
        {
            string buildnum = Convert.ToString(Convert.ToInt32(Encoding.UTF8.GetString(wc.DownloadData("http://texttale.ga/upload/buildnum"))));
            Console.WriteLine(String.Concat("http://texttale.ga/upload/", buildnum, "/Texttale.exe ", Path.Combine(DownloadLocation, "Texttale.exe")));
            if (isUpdate)
            {
                DeleteFolder(false);
                File.WriteAllBytes(new Uri(Path.Combine(DownloadLocation, "Texttale.exe")).AbsolutePath, wc.DownloadData(String.Concat("http://texttale.ga/upload/" + buildnum + "/Texttale.exe")));
                File.WriteAllBytes(new Uri(Path.Combine(DownloadLocation, "SlimDX.dll")).AbsolutePath, wc.DownloadData(String.Concat("http://texttale.ga/upload/" + buildnum + "/SlimDX.dll")));
                File.WriteAllBytes(new Uri(Path.Combine(DownloadLocation, "buildnum")).AbsolutePath, wc.DownloadData(String.Concat("http://texttale.ga/upload/buildnum")));
                MessageBox.Show("The update to build number " + buildnum + " is complete.","Done Updating.", MessageBoxButtons.OK);
            } else {
                Directory.CreateDirectory(new Uri(Path.Combine(DownloadLocation, "Texttale")).AbsolutePath);
                File.WriteAllBytes(new Uri(Path.Combine(DownloadLocation, "Texttale.exe")).AbsolutePath, wc.DownloadData(String.Concat("http://texttale.ga/upload/" + buildnum + "/Texttale.exe")));
                File.WriteAllBytes(new Uri(Path.Combine(DownloadLocation, "SlimDX.dll")).AbsolutePath, wc.DownloadData(String.Concat("http://texttale.ga/upload/" + buildnum + "/SlimDX.dll")));
                File.WriteAllBytes(new Uri(Path.Combine(DownloadLocation, "buildnum")).AbsolutePath, wc.DownloadData(String.Concat("http://texttale.ga/upload/buildnum")));
                button1.Enabled = false;
                MessageBox.Show(String.Concat("The download of Texttale build no. ", buildnum, " is complete"), "Done Downloading", MessageBoxButtons.OK);
            }
            button3.Enabled = true;
            button4.Enabled = true;
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to uninstall?", "Uninstall?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DeleteFolder(true);
                MessageBox.Show("Texttale is uninstalled.", "Uninstall Completed", MessageBoxButtons.OK);
                button3.Enabled = false;
                button1.Enabled = true;
            }            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process tt = new Process();
            tt.StartInfo.FileName = Path.Combine(DownloadLocation, "Texttale.exe");
            bool[] enabledArray = new bool[] { button1.Enabled, button2.Enabled, button3.Enabled, button4.Enabled }; ;
            try
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                this.Visible = false;
                tt.Start();
                tt.WaitForExit();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("FileNotFoundException: File \"" + ex.FileName + "not found.");
            }
            finally
            {
                button1.Enabled = enabledArray[0];
                button2.Enabled = enabledArray[1];
                button3.Enabled = enabledArray[2];
                button4.Enabled = enabledArray[3];
                this.Visible = true;
            }
        }
    }
}
