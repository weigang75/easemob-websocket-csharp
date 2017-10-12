using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOTO.Model.Model;
using MOTO.Comm.Utility;
using System.IO;

namespace MOTO.BLL.System
{
    public class FileUploadManager
    {
        private String QUOTATION_UPLOAD_PATH = "/dealer/quotation/";

        /// <summary>
        /// 传报价图片
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public PostFileResp UploadQuotationImage(string filePath)
        {
            return UploadImage(filePath, QUOTATION_UPLOAD_PATH + DateTime.Now.ToString("yyyyMMdd") + "/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="filename"></param>
        /// <param name="uploadDir"></param>
        /// <returns></returns>
        public PostFileResp UploadImage(byte[] fileBytes, string filename,string uploadDir)
        {
            return rmgr.PostFile(fileBytes,filename, filename, uploadDir);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="uploadDir"></param>
        /// <returns></returns>
        public PostFileResp UploadImage(string filePath, string uploadDir)
        {
            string tempPath = Path.Combine(Environment.CurrentDirectory,"temp");

            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            //string picExtName = Path.GetExtension(filePath);
            String filename = Guid.NewGuid().ToString();
            string thumbnailPath = Path.Combine(tempPath, "thumb_" + Path.GetFileNameWithoutExtension(filePath)+".jpg");

            try
            {
                Utility.MakeThumbnail(filePath, thumbnailPath, 800, 0, "W");
            }
            catch (Exception ex)
            {
                PostFileResp model = new PostFileResp();
                model.Success = false;
                model.Msg = "图片裁剪失败：" + ex.Message;
                return model;
            }

            return rmgr.PostFile(thumbnailPath, filename, uploadDir);
        }
    }
}
