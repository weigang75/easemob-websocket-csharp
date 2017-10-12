using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Easemob.Audio
{
    /// <summary>
    /// 音频转换
    /// </summary>
    public class AudioConverter
    {

        /// <summary>
        /// 将Wav音频转成Amr手机音频
        /// </summary>
        /// <param name="applicationPath">ffmeg.exe文件路径</param>
        /// <param name="fileName">WAV文件的路径(带文件名)</param>
        /// <param name="targetFileName">生成目前amr文件路径（带文件名）</param>
        public static string ConvertToAmr(string applicationPath, string fileName, string targetFileName)
        {
            string filename = Path.Combine(applicationPath, fileName);
            string targetfilename = Path.Combine(applicationPath, targetFileName);

            string c = GetConvertFilePath() + @" -y -i " + "\"" + filename + "\"" + " -ar 8000 -ab 12.2k -ac 1 " + "\"" + targetfilename + "\"";
            Cmd(c);

            return targetfilename;
        }

        private static string GetConvertFilePath()
        {
            string exeFile = System.Environment.CurrentDirectory + @"\conv\ffmpeg.exe";
            return exeFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationPath"></param>
        /// <param name="fileName"></param>
        /// <param name="targetFileName"></param>
        public static string ConvertToWave(string applicationPath, string fileName, string targetFileName)
        {
            string filename = Path.Combine(applicationPath, fileName);
            string targetfilename = Path.Combine(applicationPath, targetFileName);

            string c = GetConvertFilePath() + @" -y -i " + "\"" + filename + "\"" + " -ar 8000 -ab 12.2k -ac 1 " + "\"" + targetfilename + "\"";
            Cmd(c);

            return targetfilename;
        }

        /// <summary>
        /// 执行Cmd命令
        /// </summary>
        private static void Cmd(string c)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.Start();

                process.StandardInput.WriteLine(c);
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine("exit");

                StreamReader reader = process.StandardOutput;//截取输出流           

                process.WaitForExit();
            }
            catch
            { }
        }

        /// <summary>
        /// 获取文件的byte[]
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] GetFileByte(string fileName)
        {
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(pFileStream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                pReadByte = r.ReadBytes((int)r.BaseStream.Length);
                return pReadByte;
            }
            catch
            {
                return pReadByte;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }

        /// <summary>
        /// 将文件的byte[]生成文件
        /// </summary>
        /// <param name="pReadByte"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool writeFile(byte[] pReadByte, string fileName)
        {
            FileStream pFileStream = null;
            try
            {
                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                pFileStream.Write(pReadByte, 0, pReadByte.Length);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();          
            }
            return true;

        }
    }     
}
