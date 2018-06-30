/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: 易达巴士后台管理系统
 * Website：http://www.nfine.cn
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiDaBus.Com.Manager.Common
{
    public class CacheFactory
    {
        public static ICache Cache()
        {
            return new Cache();
        }
    }
}
