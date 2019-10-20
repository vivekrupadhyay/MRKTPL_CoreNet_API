using System;

namespace MRKTPL.Data.ViewModel
{
    public class ProductViewModel
    {
        public long ProductID { get; set; }
        public string ProductTitle { get; set; }
        public string ProductShortDesc { get; set; }
        public string ProductLongDesc { get; set; }
        public string ProductBannerPath { get; set; }//HttpPostedFileBase
        public string ProductThumbnailImgPath { get; set; }
        public int ProductCount { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
