namespace WillHomework01.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        private CustomerEntities db = new CustomerEntities();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //檢查有無重複的信箱
            if (db.客戶聯絡人.Any(x => x.Id != this.Id && x.客戶Id==this.客戶Id && x.是否已刪除==false && x.Email.Equals(this.Email)))
            {
                yield return new ValidationResult($"已存在{this.Email}，請重新輸入", new string[] { "Email" });
            }
        }
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
        [RegularExpression(pattern: @"\d{4}-\d{6}",ErrorMessage ="電話格式必須為0911-111111")]
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 手機 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
    
        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
