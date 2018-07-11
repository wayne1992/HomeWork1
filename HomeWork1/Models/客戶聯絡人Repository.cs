using System;
using System.Linq;
using System.Collections.Generic;
	
namespace HomeWork1.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(p => p.IsDeleted == false);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.IsDeleted = true;
        }

        public 客戶聯絡人 Find(int id)
        {

            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<客戶聯絡人> Search(string Keyword)
        {
            var data = this.All();

            if (!String.IsNullOrEmpty(Keyword))
            {
                data = data.Where(p => p.姓名.Contains(Keyword) || p.Email.Contains(Keyword));
            }
            return data;
        }
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}