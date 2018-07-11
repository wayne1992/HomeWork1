using System;
using System.Linq;
using System.Collections.Generic;
	
namespace HomeWork1.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public 客戶資料 Find(int id) {

            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<客戶資料> Search(string Keyword) {
            var data = this.All();

            if (!String.IsNullOrEmpty(Keyword))
            {
                data = data.Where(p => p.客戶名稱.Contains(Keyword) || p.統一編號.Contains(Keyword));
            }
            return data;
        }
	}

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}