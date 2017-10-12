using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;

namespace Easemob.PostedFile
{
    /// <summary>
    /// http post请求客户端
    /// 主要用于上传文件
    /// </summary>
    public class HttpRequestClient
    {
        #region //字段
        private ArrayList bytesArray;
        private Encoding encoding = Encoding.UTF8;
        private string boundary = String.Empty;
        #endregion

        #region //构造方法
        public HttpRequestClient()
        {
            bytesArray = new ArrayList();
            string flag = "JiaPartsBoundary" + DateTime.Now.Ticks.ToString("x");
            boundary = "------" + flag;
        }
        #endregion

        #region //方法
        /// <summary>
        /// 合并请求数据
        /// </summary>
        /// <returns></returns>
        private byte[] MergeContent()
        {
            int length = 0;
            int readLength = 0;
            string endBoundary = "--" + boundary + "--\r\n";
            byte[] endBoundaryBytes = encoding.GetBytes(endBoundary);

            bytesArray.Add(endBoundaryBytes);

            foreach (byte[] b in bytesArray)
            {
                length += b.Length;
            }

            byte[] bytes = new byte[length];

            foreach (byte[] b in bytesArray)
            {
                b.CopyTo(bytes, readLength);
                readLength += b.Length;
            }

            return bytes;
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="requestUrl">请求url</param>
        /// <param name="responseText">响应</param>
        /// <returns></returns>
        public bool Upload(String requestUrl, string token, out String responseText)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);
            webClient.Headers.Add("restrict-access", "true");
            webClient.Headers.Add("Authorization", "Bearer " + token);
           
            /*
             POST /easemob-demo/chatdemoui/chatfiles HTTP/1.1
Host: a1.easemob.com
Connection: keep-alive
Content-Length: 31220
User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.89 Safari/537.36
Origin: null
restrict-access: true
Authorization: Bearer YWMtdmWpqoD-EeWqenXdr9KU7QAAAVH6vBm9GLTApe93jOXncT8-I4S1aDU7rRI
Content-Type: multipart/form-data; boundary=----WebKitFormBoundaryCOp4hD38PTMYCUin
Accept: * / *
Accept-Encoding: gzip, deflate
Accept-Language: zh-CN,zh;q=0.8
             */
            byte[] responseBytes;
            byte[] bytes = MergeContent();

            try
            {
                responseBytes = webClient.UploadData(requestUrl, bytes);
                responseText = System.Text.Encoding.UTF8.GetString(responseBytes);
                return true;
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    responseText = ex.Message;
                    return false;
                }
                else
                {
                    Stream responseStream = ex.Response.GetResponseStream();
                    responseBytes = new byte[ex.Response.ContentLength];
                    responseStream.Read(responseBytes, 0, responseBytes.Length);
                }
            }
            responseText = System.Text.Encoding.UTF8.GetString(responseBytes);
            return false;
        }

        /// <summary>
        /// 设置表单数据字段
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <returns></returns>
        public void SetFieldValue(String fieldName, String fieldValue)
        {
            string httpRow = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
            string httpRowData = String.Format(httpRow, fieldName, fieldValue);

            bytesArray.Add(encoding.GetBytes(httpRowData));
        }

        /// <summary>
        /// 设置表单文件数据
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="filename">字段值</param>
        /// <param name="contentType">内容内型</param>
        /// <param name="fileBytes">文件字节流</param>
        /// <returns></returns>
        public void SetFieldValue(String fieldName, String filename, String contentType, Byte[] fileBytes)
        {
            string end = "\r\n";
            string httpRow = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string httpRowData = String.Format(httpRow, fieldName, filename, contentType);

            byte[] headerBytes = encoding.GetBytes(httpRowData);
            byte[] endBytes = encoding.GetBytes(end);
            byte[] fileDataBytes = new byte[headerBytes.Length + fileBytes.Length + endBytes.Length];

            headerBytes.CopyTo(fileDataBytes, 0);
            fileBytes.CopyTo(fileDataBytes, headerBytes.Length);
            endBytes.CopyTo(fileDataBytes, headerBytes.Length + fileBytes.Length);

            bytesArray.Add(fileDataBytes);
        }
        #endregion
    }
}
