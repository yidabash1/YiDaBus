﻿/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: 易达巴士后台管理系统
 * Website：http://www.nfine.cn
*********************************************************************************/
using System.Collections.Generic;
using System.Text;

namespace YiDaBus.Com.Manager.Common
{
    public static class TreeSelect
    {
        public static string TreeSelectJson(this List<TreeSelectModel> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(TreeSelectJson(data, "0", ""));
            sb.Append("]");
            return sb.ToString();
        }
        private static string TreeSelectJson(List<TreeSelectModel> data, string parentId, string blank)
        {
            StringBuilder sb = new StringBuilder();
            var ChildNodeList = data.FindAll(t => t.parentId == parentId);
            var tabline = "";
            if (parentId != "0")
            {
                tabline = "　　";
            }
            if (ChildNodeList.Count > 0)
            {
                tabline = tabline + blank;
            }
            foreach (TreeSelectModel entity in ChildNodeList)
            {
                entity.text = tabline + entity.text;
                string strJson = entity.ToJson();
                sb.Append(strJson);
                sb.Append(TreeSelectJson(data, entity.id, tabline));
            }
            return sb.ToString().Replace("}{", "},{");
        }
        public static string TreeSelectJson2(this List<TreeSelectModel> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(TreeSelectJson2(data, "0", ""));
            sb.Append("]");
            return sb.ToString();
        }
        private static string TreeSelectJson2(List<TreeSelectModel> data, string parentId, string blank)
        {
            StringBuilder sb = new StringBuilder();
            var ChildNodeList = data.FindAll(t => t.parentId == parentId);
            var tabline = "";
            if (parentId != "0")
            {
                tabline = "　　";
            }
            if (ChildNodeList.Count > 0)
            {
                tabline = tabline + blank;
            }
            foreach (TreeSelectModel entity in ChildNodeList)
            {
                string _id = entity.id;
                entity.id = entity.text;
                entity.text = tabline + entity.text;
                string strJson = entity.ToJson();
                sb.Append(strJson);
                sb.Append(TreeSelectJson2(data, _id, tabline));
            }
            return sb.ToString().Replace("}{", "},{");
        }
    }
}