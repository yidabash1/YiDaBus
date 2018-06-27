using Aspose.Words;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace YiDaBus.Com.Mobile.Web.Controllers
{
    public class DownLoadController : BaseController
    {
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public ActionResult DownLoadFile(string filePath, string fileName)
        {
            return File(SaveStream(Server.MapPath(filePath)), "application/octet-stream", fileName);
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