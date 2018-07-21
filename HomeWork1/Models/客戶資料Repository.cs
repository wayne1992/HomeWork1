using System;
using System.Linq;
using System.Collections.Generic;
	
namespace HomeWork1.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public IQueryable<客戶資料> All(string sortType, string colName)
        {
            if (!String.IsNullOrEmpty(sortType) && !String.IsNullOrEmpty(colName))
            {
                if (sortType == "DESC")
                {
                    switch (colName){
                        case "客戶名稱":
                            return base.All().Where(p => p.IsDeleted == false).OrderByDescending(p => p.客戶名稱);
                            
                        case "統一編號":
                            return base.All().Where(p => p.IsDeleted == false).OrderByDescending(p => p.統一編號);

                        case "電話":
                            return base.All().Where(p => p.IsDeleted == false).OrderByDescending(p => p.電話);

                        case "傳真":
                            return base.All().Where(p => p.IsDeleted == false).OrderByDescending(p => p.傳真);

                        case "地址":
                            return base.All().Where(p => p.IsDeleted == false).OrderByDescending(p => p.地址);

                        case "Email":
                            return base.All().Where(p => p.IsDeleted == false).OrderByDescending(p => p.Email);

                        case "客戶分類":
                            return base.All().Where(p => p.IsDeleted == false).OrderByDescending(p => p.客戶分類);

                        default:
                            return base.All().Where(p => p.IsDeleted == false);    
                    }
                    
                }
                else
                {
                    switch (colName)
                    {
                        case "客戶名稱":
                            return base.All().Where(p => p.IsDeleted == false).OrderBy(p => p.客戶名稱);

                        case "統一編號":
                            return base.All().Where(p => p.IsDeleted == false).OrderBy(p => p.統一編號);

                        case "電話":
                            return base.All().Where(p => p.IsDeleted == false).OrderBy(p => p.電話);

                        case "傳真":
                            return base.All().Where(p => p.IsDeleted == false).OrderBy(p => p.傳真);

                        case "地址":
                            return base.All().Where(p => p.IsDeleted == false).OrderBy(p => p.地址);

                        case "Email":
                            return base.All().Where(p => p.IsDeleted == false).OrderBy(p => p.Email);

                        case "客戶分類":
                            return base.All().Where(p => p.IsDeleted == false).OrderBy(p => p.客戶分類);

                        default:
                            return base.All().Where(p => p.IsDeleted == false);
                    }
                }

            }
            else
            {
                return base.All().Where(p => p.IsDeleted == false);
            }

            
        }

        public override void Delete(客戶資料 entity)
        {
            entity.IsDeleted = true;
        }

        public 客戶資料 Find(int id) {

            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<客戶資料> Search(string Keyword, int? classification) {
            var data = this.All();

            if (!String.IsNullOrEmpty(Keyword))
            {
                data = data.Where(p => p.客戶名稱.Contains(Keyword) || p.統一編號.Contains(Keyword));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(classification)))
            {
                data = data.Where(p => p.客戶分類 == classification);
            }
            return data;
        }
	}

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}