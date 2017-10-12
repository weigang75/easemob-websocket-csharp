using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Drawing;

namespace Easemob.PostedFile
{
    public class PostedFileResp 
    {
        /// <summary>
        ///  "action" : "post",
        /// </summary>
        public string action { get; set; }
        /// <summary>
        ///   "application" : "4d7e4ba0-dc4a-11e3-90d5-e1ffbaacdaf5",
        /// </summary>
        public string application { get; set; }
        /// <summary>
        /// "path" : "/chatfiles",
        /// </summary>
        public string path { get; set; }
        /// <summary>
        ///   "uri" : "https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles",
        /// </summary>
        public string uri { get; set; }

        public PostFileEntity[] entities { get; set; }

        /// <summary>
        /// "timestamp" : 1446533700821,
        /// </summary>
        public long timestamp { get; set; }
        /// <summary>
        /// "duration" : 40,
        /// </summary>
        public int duration { get; set; }
        /// <summary>
        /// "organization" : "easemob-demo",
        /// </summary>
        public string organization { get; set; }

        /// <summary>
        /// "applicationName" : "chatdemoui"
        /// </summary>
        public string applicationName { get; set; }
    }

    /// <summary>
    /// 参考：http://docs.easemob.com/doku.php?id=start:100serverintegration:30chatlog
    /// </summary>
    public class PostFileEntity
    {
        /// <summary>
        /// "uuid" : "cd6f8050-81f7-11e5-a16a-05187e341cb0",
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// "type" : "chatfile",
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// "share-secret" : "zW-AWoH3EeWmJevV5n4Fpkxnnu3e5okMLIhENE0QHaZbvqg5"
        /// </summary>
        [JsonProperty("share-secret")]
        public string share_secret { get; set; }

        /// <summary>
        /// 自建属性，用于文件名的信息
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// 自建属性，用于传递图片的大小信息
        /// </summary>
        public Size imageSize { get; set; }
        /*
         {  
    "from":"test2",
    "to":"test1",
    "bodies":[{
    	"type":"audio",//语音消息类型
    	"url":"https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/0637e55a-f606-11e3-ba23-51f25fd1215b",//上传语音远程地址，在上传语音后会返回uuid
    	"filename":"test1.amr",//语音名称
    	"length":10, //语音时间（单位秒）
    	"secret":"DRGM8OZrEeO1vafuJSo2IjHBeKlIhDp0GCnFu54xOF3M6KLr"//secret在上传文件后会返回
    }]
}
         */
        /// <summary>
        /// 语音、视频时间（秒）
        /// </summary>
        public int length { get; set; }
    }
}
/*
{
  "action" : "post",
  "application" : "4d7e4ba0-dc4a-11e3-90d5-e1ffbaacdaf5",
  "path" : "/chatfiles",
  "uri" : "https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles",
  "entities" : [ {
    "uuid" : "3ec2bb50-81f8-11e5-8e7c-1fa6315dec2d",
    "type" : "chatfile",
    "share-secret" : "PsK7WoH4EeWwmIkeyVsexnkK-Rmqu2X_N2qqK9FQYmUkko8W"
  } ],
  "timestamp" : 1446533890949,
  "duration" : 3,
  "organization" : "easemob-demo",
  "applicationName" : "chatdemoui"
}

 {
  "action" : "post",
  "application" : "4d7e4ba0-dc4a-11e3-90d5-e1ffbaacdaf5",
  "path" : "/chatfiles",
  "uri" : "https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles",
  "entities" : [ {
    "uuid" : "cd6f8050-81f7-11e5-a16a-05187e341cb0",
    "type" : "chatfile",
    "share-secret" : "zW-AWoH3EeWmJevV5n4Fpkxnnu3e5okMLIhENE0QHaZbvqg5"
  } ],
  "timestamp" : 1446533700821,
  "duration" : 40,
  "organization" : "easemob-demo",
  "applicationName" : "chatdemoui"
}
 
 */