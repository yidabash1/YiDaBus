using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YiDaBus.Com.Manager.Common;

namespace YiDaBus.Com.Manager.Web.Controllers
{
    public class DownController : ControllerBase
    {
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownLoadFromRemote(string url, string fileName)
        {
            fileName = string.Format(@"{0}\QRIMG\{1}.jpg", System.AppDomain.CurrentDomain.BaseDirectory, fileName);
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                WebClient wc = new WebClient();
                var data = wc.DownloadData(url);
                foreach (var b in data)
                {
                    fs.WriteByte(b);
                }
            }
            return Success("下载成功");
        }

        /// <summary>
        /// 保存流
        /// </summary>
        /// <param name="path">文件的路径</param>
        /// <returns></returns>
        public Stream SaveStream(string path)
        {
            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
            {
                WebClient webClient = new WebClient();
                var obj = webClient.OpenRead(path);
                return obj;
            }
        }
    }
}