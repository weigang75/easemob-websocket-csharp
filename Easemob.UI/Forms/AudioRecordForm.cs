using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Easemob.Audio;
using System.IO;

namespace Easemob.UI.Forms
{
    public partial class AudioRecordForm : Form
    {
        public AudioRecordForm()
        {
            InitializeComponent();
        }

        public string FileNamePrefix { get; set; }

        private AudioRecorder recorder = new AudioRecorder();

        public string AudioFileName { get; private set; }

        private void btnRecord_MouseDown(object sender, MouseEventArgs e)
        {
            recorder.RecordSound();
        }

        private void btnRecord_MouseUp(object sender, MouseEventArgs e)
        {
            DateTime now = DateTime.Now;

            string fileTempDir = SdkUtils.GetRevFileTempDir();
            string filename = String.Format("{0}_{1}", FileNamePrefix, now.ToString("MMddHHmmss"));
            string wavFile = filename + ".wav";
            string amrFile = filename + ".amr";
            string wavFilePath = Path.Combine(fileTempDir, wavFile);
            // 保存为wav格式
            recorder.EndRecordSound(wavFilePath);

            if (File.Exists(wavFilePath))
            {
                AudioConverter.ConvertToAmr(fileTempDir, wavFile, amrFile);
                AudioFileName = Path.Combine(fileTempDir, amrFile);
                this.Close();
            }
            else
            { 
                //错误
            }
        }
    }
}
