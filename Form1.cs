using System;
using System.Windows.Forms;

namespace Texttale_Installer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = (System.Drawing.Image)new System.Drawing.ImageConverter().ConvertFrom(new System.Net.WebClient().DownloadData("arbituaryotter.bitballoon.com/def.ico"));
        }
    }
}
