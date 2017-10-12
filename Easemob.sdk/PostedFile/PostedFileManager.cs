using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;


namespace Easemob.PostedFile
{
    /// <summary>
    /// 文件上传管理器
    /// </summary>
    public class PostedFileManager
    {
        
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url">上传的URL地址</param>
        /// <param name="token">上传需要的 token</param>
        /// <param name="filePath">要上传的文件本地路径</param>
        /// <returns></returns>
        public static PostedFileResp PostFile(string url, string token, string filePath)
        {
            PostedFileResp resp = new PostedFileResp();
            // 参数不能为空
            if (String.IsNullOrEmpty(url) || 
                String.IsNullOrEmpty(token) || 
                String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("url,token,filePath");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("文件读取失败！", filePath);
            }

            // 读取文件字节
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] fileBytes = new byte[fs.Length];
            fs.Read(fileBytes, 0, fileBytes.Length);
            fs.Close();
            fs.Dispose();

            return PostFile(url,token, fileBytes, filePath);
        }

        
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url">上传的URL地址</param>
        /// <param name="token">上传需要的 token</param>
        /// <param name="fileBytes">文件字节</param>
        /// <param name="filePath">要上传的文件本地路径</param>
        /// <returns></returns>
        public static PostedFileResp PostFile(string url, string token, byte[] fileBytes, string filePath)
        {
            string responseText = "";

            HttpRequestClient httpRequestClient = new HttpRequestClient();
            httpRequestClient.SetFieldValue("file", filePath, "application/octet-stream", fileBytes);

            // 开始上传文件
            httpRequestClient.Upload(url, token,out responseText);
            
            // 上传返回的参数
            PostedFileResp resp = JsonConvert.DeserializeObject<PostedFileResp>(responseText);

            if (resp.entities == null || resp.entities.Length == 0)
            { 
                // 可能上传失败，原因是？？？？？
                throw new ApplicationException("上传失败，原因是？？？？？");
            }

            // 传入本地文件路径到 PostedFileResp.entities; entities[0]为第一文件，目前只支持1个文件的上传
            resp.entities[0].filename = Path.GetFileName(filePath);

            string fileType = SdkUtils.GetFileType(resp.entities[0].filename);
            // 如果文件为图片，则获取图片的大小，并传入到PostedFileResp.entities
            if ("img".Equals(fileType))
            {
                Image img = Bitmap.FromFile(filePath);
                resp.entities[0].imageSize = new Size(img.Width, img.Height);
                img.Dispose();
            }
            return resp;
        }
    }
}
