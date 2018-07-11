using System;
using System.Linq;
using System.Collections.Generic;
	
namespace HomeWork1.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(p => p.IsDeleted == false);
        }

        public override void Delete(客戶銀行資訊 entity)
        {
            entity.IsDeleted = true;
        }

        public 客戶銀行資訊 Find(int id)
        {

            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<客戶銀行資訊> Search(string Keyword)
        {
            var data = this.All();

            if (!String.IsNullOrEmpty(Keyword))
            {
                data = data.Where(p => p.銀行名稱.Contains(Keyword) || p.銀行代碼.Contains(Keyword));
            }
            return data;
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}