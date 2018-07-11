using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HomeWork1.Models.InputValidate
{
    public class PhoneNumberAttribute : DataTypeAttribute
    {
        public PhoneNumberAttribute() : base(DataType.Text) {
            ErrorMessage = "請輸入電話格式( e.g. 0911-111111 )";
        }

        public override bool IsValid(object value)
        {
            bool result = false;
            string pattern = @"\d{4}-\d{6}";
            Regex regex = new Regex(pattern);

            string phone = (string)value;

            if (!String.IsNullOrEmpty(phone))
            {
                result = regex.IsMatch(phone);
            }

            return result;
        }

    }
}