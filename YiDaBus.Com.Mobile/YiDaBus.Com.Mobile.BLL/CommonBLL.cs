using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiDaBus.Com.Dal.Base;
using YiDaBus.Com.Manager.Common;
using YiDaBus.Com.Mobile.Model.Const;
using YiDaBus.Com.Model;

namespace YiDaBus.Com.Mobile.BLL
{
    public static class CommonBLL
    {
        /// <summary>
        /// 获取通用字典
        /// </summary>
        /// <returns></returns>
        public static List<Sys_ItemsDetail> GetGlobalConstVariable(string F_ItemCode = "")
        {
            WhereClipBuilder wherebuilder = new WhereClipBuilder();
            wherebuilder.And(Sys_ItemsDetail._.F_ItemId == YiDaBusConst.CommonDicItemId);
            if (!F_ItemCode.IsEmpty())
            {
                wherebuilder.And(Sys_ItemsDetail._.F_ItemCode == F_ItemCode);
            }
            return Db.MySqlContext.From<Sys_ItemsDetail>().Where(wherebuilder.ToWhereClip()).ToList();
        }
    }
}
