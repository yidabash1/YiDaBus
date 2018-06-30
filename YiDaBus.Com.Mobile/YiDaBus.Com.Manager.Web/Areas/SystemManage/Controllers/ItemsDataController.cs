/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: 指房向后台管理系统
 * Website：http://www.nfine.cn
*********************************************************************************/
using NFine.Application.SystemManage;
using YiDaBus.Com.Manager.Common;
using NFine.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LitJson2;
using YiDaBus.Com.Dal.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml;
using System;

namespace YiDaBus.Com.Manager.Web.Areas.SystemManage.Controllers
{
    public class ItemsDataController : ControllerBase
    {
        private ItemsDetailApp itemsDetailApp = new ItemsDetailApp();
        private ItemsApp itemsApp = new ItemsApp();
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string itemId, string keyword)
        {
            var data = itemsDetailApp.GetList(itemId, keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJson(string enCode)
        {
            var data = itemsDetailApp.GetItemList(enCode);
            List<object> list = new List<object>();
            foreach (ItemsDetailEntity item in data)
            {
                list.Add(new { id = item.F_ItemCode, text = item.F_ItemName });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson(string enCode)
        {
            JsonData r = new JsonData();
            r.Add(null);
            r.Clear();
            var data = itemsDetailApp.GetItemList(enCode);
            foreach (ItemsDetailEntity item in data)
            {
                JsonData boot = new JsonData();
                boot["id"] = item.F_ItemName;
                boot["text"] = item.F_ItemName;
                boot["elment"] = item.F_ItemName;
                r.Add(boot);
            }
            return Content(JsonMapper.ToJson(r));
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = itemsDetailApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ItemsDetailEntity itemsDetailEntity, string keyValue)
        {
            itemsDetailApp.SubmitForm(itemsDetailEntity, keyValue);

            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            itemsDetailApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        #region 生成xml和json
        [HttpPost]
        [HandlerAuthorize]
        public ActionResult GenerateFile(string ext = "json")
        {
            string jsonStr = GetJsonDataByNewTon();//使用递归获取，利用litjson生成json字符串
            FileHelper.CreateDir("File");//判断文件是否存在，如果不存在则创建文件夹
            string VirtualPath = @"/File/Dictionary." + ext;//虚拟路径
            string PhysicalPath = Server.MapPath(VirtualPath);//实际的物理路径
            if (ext == "json")//如果是要生成json格式
            {
                FileHelper.WriteText(PhysicalPath, jsonStr, UTF8Encoding.UTF8);//将文件存入服务器
            }
            else//如果是要生成xml格式
            {
                XmlDocument xdoc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(jsonStr, "DictionaryJsonV1");//首先将json装xml
                xdoc.Save(PhysicalPath);//将文件存入服务器
            }
            FileDownHelper.DownLoad(VirtualPath);//从服务器下载文件至本地
            return Success("下载成功！");//下载成功
        }

        private string GetJsonDataByLitJson()
        {
            JsonData result = new JsonData();
            var data = itemsApp.GetList();
            var detailsData = itemsDetailApp.GetList();//获取所有数据
            JsonData resultArr = new JsonData();
            //遍历分组
            foreach (var item in data)
            {
                JsonData itemJd = new JsonData();
                itemJd["key"] = item.F_EnCode;
                //在所有数据中查找f_itemid==item.key的数据
                JsonData itemAddJd = new JsonData();
                itemAddJd.Add(null);
                itemAddJd.Clear();
                foreach (var details in detailsData.Where(d => d.F_ItemId == item.F_Id))
                {
                    JsonData itemDetailJD = new JsonData();
                    itemDetailJD["Id"] = details.F_Id;
                    itemDetailJD["Code"] = details.F_ItemCode;
                    itemDetailJD["Name"] = details.F_ItemName;
                    itemAddJd.Add(itemDetailJD);
                }
                itemJd["value"] = itemAddJd;
                resultArr.Add(itemJd);
            }
            result["DictionaryJsonV1"] = resultArr;
            return JsonMapper.ToJson(result);
        }
        private string GetJsonDataByNewTon()
        {
            JObject result = new JObject();
            var data = itemsApp.GetList();
            var detailsData = itemsDetailApp.GetList();//获取所有数据
            JArray resultArr = new JArray();
            foreach (var item in data)
            {
                //在所有数据中查找f_itemid==item.key的数据
                JArray itemAddArr = new JArray();
                foreach (var details in detailsData.Where(d => d.F_ItemId == item.F_Id))
                {
                    JObject itemDetailJD = new JObject(
                        new JProperty("Id", details.F_Id),
                        new JProperty("Code", details.F_ItemCode),
                        new JProperty("Name", details.F_ItemName)
                        );
                    itemAddArr.Add(itemDetailJD);
                }
                resultArr.Add(new JObject(
                     new JProperty("key", item.F_EnCode),
                     new JProperty("value", itemAddArr)
                ));
            }
            result["DictionaryJsonV1"] = resultArr;
            return JsonConvert.SerializeObject(result);
        }
        #endregion

        
    }
}
