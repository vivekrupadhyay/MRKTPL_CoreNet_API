using System;
using System.Collections.Generic;
using System.Text;

namespace MRKTPL.Data.Entities
{
   public class RelatedProduct
    {
        public int Id { get; set; }
        public int ProductId1 { get; set; }
        public int ProductId2 { get; set; }
        public int DisplayOrder { get; set; }
        
    }
}
