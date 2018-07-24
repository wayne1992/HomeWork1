namespace HomeWork1.Models
{
    using HomeWork1.Models.InputValidate;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人
    //public partial class 客戶聯絡人 : IValidatableObject
    {
        客戶資料Repository CustomerRepo = RepositoryHelper.Get客戶資料Repository();
        客戶聯絡人Repository CustomerContactRepo = RepositoryHelper.Get客戶聯絡人Repository();

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
            //var data = CustomerContactRepo.All().Count(P => P.客戶Id == this.客戶Id);

            //if (data > 0)
            //{
            //    var EmailIsDuplicate = CustomerContactRepo.All().Count(p => p.Email == this.Email && p.客戶Id == this.客戶Id);
            //    if (EmailIsDuplicate > 0)
            //    {
            //        yield return new ValidationResult("此客戶已有重複的聯絡人Email", new string[] { "Email" });
            //    }
            //}

        //}
    }
    
    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }
        
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        //[RegularExpression(@"\d{4}-\d{6}", ErrorMessage = "請輸入電話格式( e.g. 0911-111111 )")]
        [PhoneNumber] //自訂驗證
        public string 手機 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 電話 { get; set; }

        public bool IsDeleted { get; set; }

        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
