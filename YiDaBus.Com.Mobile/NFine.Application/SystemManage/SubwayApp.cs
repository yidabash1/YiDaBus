using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace NFine.Application.SystemManage
{
   public class SubwayApp
    {
        private ISubwayRepository service = new SubwayRepository();

        public List<SubwayEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public List<SubwayEntity> GetList(Expression<Func<SubwayEntity, bool>> predicate)
        {
            return service.IQueryable(predicate).ToList();
        }
        public SubwayEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.F_Id == keyValue);
            }
        }
        public void SubmitForm(SubwayEntity SubwayEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SubwayEntity.Modify(keyValue);
                service.Update(SubwayEntity);
            }
            else
            {
                SubwayEntity.Create();
                service.Insert(SubwayEntity);
            }
        }


        public void SubwayInsert(List<SubwayEntity>  SubwayEntityList)
        {
            service.Insert(SubwayEntityList);
        }
    }
}
