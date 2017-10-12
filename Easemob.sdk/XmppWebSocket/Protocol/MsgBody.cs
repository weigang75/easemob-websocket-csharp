using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Easemob.XmppWebSocket.Protocol
{
    /// <summary>
    /// {"from":"weigang75","to":"march3","bodies":[{"type":"txt","msg":"?"}],"ext":{}};
    /// </summary>
    public class MsgData
    {
        public MsgData()
        {
            ext = new { };
        }
        ///// <summary>
        ///// "target_type" : "users",   //users 给用户发消息, chatgroups 给群发消息
        ///// </summary>
        //public string target_type { get; set; }
        ///// <summary>
        /////  "target" : ["u1", "u2", "u3"],// 注意这里需要用数组,数组长度建议不大于20, 即使只有一个用户,   
        ///// 也要用数组 ['u1'], 给用户发送时数组元素是用户名,给群组发送时  
        ///// 数组元素是groupid
        ///// </summary>
        //public string[] target { get; set; }
        /// <summary>
        /// "from" : "jma2", //表示这个消息是谁发出来的, 可以没有这个属性, 那么就会显示是admin, 如果有的话, 则会显示是这个用户发出的    
        /// </summary>
        public string from { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string to { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public BodyBase[] bodies { get; set; }
        /// <summary>
        /// 扩展属性, 由app自己定义.可以没有这个字段，但是如果有，值不能是“ext:null“这种形式，否则出错
        /// </summary>
        public object ext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                return "{}";

            return JsonConvert.SerializeObject(this);
        }
    }
    /*
     
    "target" : ["u1", "u2", "u3"],// 注意这里需要用数组,数组长度建议不大于20, 即使只有一个用户,   
                                  // 也要用数组 ['u1'], 给用户发送时数组元素是用户名,给群组发送时  
                                  // 数组元素是groupid
     */

    /// <summary>
    /// 消息包基类
    /// </summary>
    public abstract class BodyBase
    {
        /// <summary>
        /// 消息包的类型
        /// </summary>
        public virtual string type { get; set; }
    }

    /// <summary>
    /// 文本消息包
    /// </summary>
    public class TextBody : BodyBase
    {
        public override string type 
        {
            get { return "txt"; }
            set { }
        }

        public string msg { get; set; }
    }

    /// <summary>
    /// 定位消息包
    /// </summary>
    public class LocationBody : BodyBase
    {
        public override string type
        {
            get { return "loc"; }
            set { }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public string addr { get; set; }
        /// <summary>
        /// lat(经度)
        /// </summary>
        public double lat { get; set; }
        /// <summary>
        /// lng (纬度)
        /// </summary>
        public double lng { get; set; }
        /*
        <body>{"from":"weigang75","to":"march3","bodies":
         * [{"type":"loc","addr":"湖北省武汉市洪山区珞瑜路881","lat":30.50826,"lng":114.3957}],"ext":{}}
         * </body>

         */
    }

    /// <summary>
    /// 文件消息包
    /// </summary>
    public class FileBody : BodyBase
    {
        public override string type
        {
            get { return "file"; }
            set { }
        }
        public string url { get; set; }
        /// <summary>
        /// 成功上传文件后返回的secret
        /// </summary>
        public string secret { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// 视频文件大小
        /// </summary>
        public long file_length { get; set; }
        /*
        <body>{"from":"weigang75","to":"march3","bodies":
         * [{"type":"file","url":"http://182.92.228.160:80/easemob-demo/chatdemoui/chatfiles/91d5e6f0-881a-11e5-9742-9b38b462a4a4",
         * "filename":"gstrings.trace",
         * "file_length":2511,"secret":"kdXm-ogaEeWhzcUWXuHshOs0Wv7sx4yMQ8orRXiReRO6sk1T"}],"ext":{}}</body><thread>6z9490</thread></message>
         */
    }

    /// <summary>
    /// 音频消息包
    /// </summary>
    public class AudioBody : FileBody
    {  
        public override string type 
        {
            get { return "audio"; }
            set { }
        }
        /// <summary>
        /// 播放长度（秒）
        /// </summary>
        public int length { get; set; }
    }


    public class VideoBody : FileBody
    {
        public override string type
        {
            get { return "video"; }
            set { }
        }

        /// <summary>
        /// "https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/67279b20-7f69-11e4-8eee-21d3334b3a97",//成功上传视频缩略图返回的uuid
        /// </summary>
        public string thumb { get; set; }

        /// <summary>
        /// 视频播放长度
        /// </summary>
        public int length { get; set; }

        /// <summary>
        /// "ZyebKn9pEeSSfY03ROk7ND24zUf74s7HpPN1oMV-1JxN2O2I",// 成功上传视频缩略图后返回的secret
        /// </summary>
        public string thumb_secret { get; set; }
    }

    public class ImageSize
    {
        public int width { get; set; }
        public int height { get; set; }
    }
    
    /// <summary>
    /// 图片消息包
    /// </summary>
    public class ImageBody : FileBody
    {
        public override string type 
        {
            get { return "img"; }
            set { }
        }


        public string thumb { get; set; }
        public string thumb_secret { get; set; }
        public ImageSize size { get; set; }
    }

}
