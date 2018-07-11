using System;
using System.Linq;
using System.Collections.Generic;
	
namespace HomeWork1.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
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