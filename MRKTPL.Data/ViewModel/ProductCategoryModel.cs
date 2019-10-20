using System;
using System.Collections.Generic;
using System.Text;

namespace MRKTPL.Data.ViewModel
{
    public class ProductCategoryModel
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public bool IsFeaturedProduct { get; set; }
        public string ProductName { get; set; }
        public int DisplayOrder { get; set; }
        
    }
}
