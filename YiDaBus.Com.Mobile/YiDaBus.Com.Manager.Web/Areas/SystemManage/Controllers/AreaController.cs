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
using System.IO;
using System;
using System.Text;
using System.Xml;

namespace YiDaBus.Com.Manager.Web.Areas.SystemManage.Controllers
{
    public class AreaController : ControllerBase
    {
        private AreaApp areaApp = new AreaApp();


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = areaApp.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (AreaEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = true;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                treeList = treeList.TreeWhere(t => t.text.Contains(keyword), "id", "parentId");
            }
            return Content(treeList.TreeGridJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = areaApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (AreaEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson1()
        {
            JsonData r = new JsonData();
            r.Add(null);
            r.Clear();
            var data = areaApp.GetList();
            var treeList = new List<TreeSelectModel>();

            var BootData = data.Where(t => t.F_ParentId == "0");
            foreach (AreaEntity item in BootData)
            {
                JsonData boot = new JsonData();
                boot["id"] = item.F_FullName;
                boot["text"] = item.F_FullName;
                boot["elment"] = item.F_FullName;

                var childData = data.Where(t => t.F_ParentId == item.F_Id);
                JsonData arrChild = new JsonData();
                arrChild.Add(null);
                arrChild.Clear();
                foreach (var citem in childData)
                {
                    JsonData Child = new JsonData();
                    Child["id"] = citem.F_FullName;
                    Child["text"] = citem.F_FullName;
                    Child["elment"] = citem.F_FullName;
                    arrChild.Add(Child);
                }
                boot["children"] = arrChild;
                r.Add(boot);
            }

            return Content(JsonMapper.ToJson(r));
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson2()
        {
            var data = areaApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (AreaEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson2());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson3()
        {
            OperatorModel op = OperatorProvider.Provider.GetCurrent();
            List<AreaEntity> data = areaApp.GetList();
            var treeList = new List<TreeSelectModel>();

            if (string.IsNullOrEmpty(op.F_Area) || op.F_Area.Contains("上海市"))
            {
                foreach (AreaEntity item in data)
                {
                    TreeSelectModel treeModel = new TreeSelectModel();
                    treeModel.id = item.F_Id;
                    treeModel.text = item.F_FullName;
                    treeModel.parentId = item.F_ParentId;
                    treeList.Add(treeModel);
                }
                return Content(treeList.TreeSelectJson2());
            }
            else
            {
                JsonData r = new JsonData();
                r.Add(null);
                r.Clear();
                foreach (AreaEntity item in data)
                {
                    if (op.F_Area.Contains(item.F_FullName))
                    {
                        JsonData boot = new JsonData();
                        boot["id"] = item.F_FullName;
                        boot["text"] = item.F_FullName;
                        boot["elment"] = item.F_FullName;
                        r.Add(boot);
                    }
                }
                return Content(JsonMapper.ToJson(r));
            }
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = areaApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(AreaEntity areaEntity, string keyValue)
        {
            AreaEntity parentEntity = areaApp.GetForm(areaEntity.F_ParentId);
            if (parentEntity != null)
            {
                areaEntity.F_Layers = parentEntity.F_Layers + 1;
            }
            else
            {
                areaEntity.F_Layers = 0;
            }
            areaApp.SubmitForm(areaEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            areaApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        #region 递归获取树状结构图
        [HttpPost]
        [HandlerAuthorize]
        public ActionResult GenerateFile(string ext = "json")
        {
            string jsonStr = GetTreeSelectJsonByRecursion();//使用递归获取，利用litjson生成json字符串
            FileHelper.CreateDir("File");//判断文件是否存在，如果不存在则创建文件夹
            string VirtualPath = @"/File/City." + ext;//虚拟路径
            string PhysicalPath = Server.MapPath(VirtualPath);//实际的物理路径
            if (ext == "json")//如果是要生成json格式
            {
                FileHelper.WriteText(PhysicalPath, jsonStr, UTF8Encoding.UTF8);//将文件存入服务器
            }
            else//如果是要生成xml格式
            {
                XmlDocument xdoc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(jsonStr, "AreaJsonV1");//首先将json装xml
                xdoc.Save(PhysicalPath);//将文件存入服务器
            }
            FileDownHelper.DownLoad(VirtualPath);//从服务器下载文件至本地
            return Success("下载成功！");//下载成功
        }
        /// <summary>
        /// 递归获取树形json
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        private string GetTreeSelectJsonByRecursion()
        {
            JsonData r = new JsonData();
            JsonData city = new JsonData();
            city["root"] = GetChild(areaApp.GetList(), "0");
            r["AreaJsonV1"] = city;
            return JsonMapper.ToJson(r);
        }
        private JsonData GetChild(List<AreaEntity> data, string F_Id)
        {
            var childData = data.Where(t => t.F_ParentId == F_Id);
            JsonData arrChild = new JsonData();
            arrChild.Add(null);
            arrChild.Clear();
            foreach (var citem in childData)
            {
                JsonData Child = new JsonData();
                Child["id"] = citem.F_Id;
                Child["text"] = citem.F_FullName;
                Child["layer"] = citem.F_Layers;
                Child["children"] = GetChild(data, citem.F_Id);
                arrChild.Add(Child);
            }
            return arrChild;
        }
        #endregion
    }
}
