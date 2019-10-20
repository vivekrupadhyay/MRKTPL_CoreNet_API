using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MRKTPL.Data.Entities
{

   public class CategoryMaster
    {
        [Key]
        public Int64 CategoryID { get; set; }
        public int ParentCategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryImage { get; set; }
        public string ParentCategoryTitle { get; set; }
        public string ParentCategoryShortDesc { get; set; }
        public string ParentCategoryLongDesc { get; set; }
        public string SubCategoryTitle { get; set; }
        public string ShortCategoryShortDesc { get; set; }
        public string ShortCategoryLongDesc { get; set; }
        public bool IsParentCategory { get; set; }
        public bool IsSubCategory { get; set; }
        public bool ShowOnHomePage { get; set; }
        public string PriceRanges { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public static implicit operator string(CategoryMaster v)
        {
            throw new NotImplementedException();
        }
    }
}
