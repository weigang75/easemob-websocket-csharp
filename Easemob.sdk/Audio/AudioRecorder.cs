using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Easemob.Audio
{
    /// <summary>
    /// 音频录音
    /// </summary>
    public class AudioRecorder
    {
        /// <summary>
        /// 录音注册的DLL
        /// </summary>
        /// <param name="lpstrCommand"></param>
        /// <param name="lpstrReturnString"></param>
        /// <param name="uReturnLength"></param>
        /// <param name="hwndCallback"></param>
        /// <returns></returns>
        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        public static extern int mciSendString(
         string lpstrCommand,
         string lpstrReturnString,
         int uReturnLength,
         int hwndCallback
        );

        /// <summary>
        /// 开始录音
        /// </summary>
        public void RecordSound()
        {
            mciSendString("set wave bitpersample 8", "", 0, 0);
            mciSendString("set wave samplespersec 20000", "", 0, 0);
            mciSendString("set wave channels 2", "", 0, 0);
            mciSendString("set wave format tag pcm", "", 0, 0);
            mciSendString("open new type WAVEAudio alias movie", "", 0, 0);
            mciSendString("record movie", "", 0, 0);
        }

        /// <summary>
        /// 结束录音，保存文件生成Wav
        /// </summary>
        /// <param name="fileName">Wav文件名</param>
        public void EndRecordSound(string fileName)
        {
            mciSendString("stop movie", "", 0, 0);
            mciSendString("save movie " + fileName, "", 0, 0);
            mciSendString("close movie", "", 0, 0);
        }

    }
}
