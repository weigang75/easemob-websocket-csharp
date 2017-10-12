using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Easemob
{
    public class SdkUtils
    {
        /// <summary>
        /// 根据文件的名称获取类型，必须包含扩展名
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFileType(string filename)
        {
            string extName = Path.GetExtension(filename).ToLower();

            switch (extName)
            {
                case ".jpg":
                case ".png":
                case ".gif":
                case ".bmp":
                case ".jpeg":
                    return "img";
                case ".mp3":
                case ".mid":
                case ".amr":
                    return "audio";
                case ".mp4":
                    return "video";
                default:
                    break;
            }
            return "file";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDirectory()
        {
            return System.Environment.CurrentDirectory;
        }
        /// <summary>
        /// 获取接收文件的临时目录
        /// </summary>
        /// <returns></returns>
        public static string GetRevFileTempDir()
        {
            return GetRevFileTempDir(null);
        }
        /// <summary>
        /// 获取接收文件的临时目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRevFileTempDir(string path)
        {
            string tempDir = DateTime.Now.ToString("yyyyMM");
            if (!string.IsNullOrEmpty(path))
                tempDir = tempDir + "\\" + path.Trim('\\');

            return GetFileTempDir(tempDir);
        }

        /// <summary>
        /// 获取文件的临时目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileTempDir(string path)
        {
            string tempDir = System.Environment.CurrentDirectory + @"\temp";

            if (!string.IsNullOrEmpty(path))
                tempDir = tempDir + "\\" + path.Trim('\\');

            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            return tempDir;
        }

        /// <summary>
        /// 下载缩略图
        /// curl -O -H "thumbnail: true" -H "share-secret: DRGM8OZrEeO1vafuJSo2IjHBeKlIhDp0GCnFu54xOF3M6KLr" -H "Authorization: Bearer YWMtz1hFWOZpEeOPpcmw1FB0RwAAAUZnAv0D7y9-i4c9_c4rcx1qJDduwylRe7Y" -H "Accept: application/octet-stream" https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/0c0f5f3a-e66b-11e3-8863-f1c202c2b3ae
        /// </summary>
        /// <param name="url"></param>
        /// <param name="secret"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static byte[] DownloadThumbnail(string url,string secret, string access_token)
        {
            WebClient client = new WebClient();
            client.Headers.Add("thumbnail", "true");
            client.Headers.Add("share-secret", secret);
            client.Headers.Add("Authorization", "Bearer " + access_token);
            client.Headers.Add("Accept", "application/octet-stream");
            byte[] bytes = new byte[0];

            try
            {
                bytes = client.DownloadData(url);
                return bytes;
            }
            catch (Exception)
            {
            }

            return new byte[0];        
        }
    }
}
