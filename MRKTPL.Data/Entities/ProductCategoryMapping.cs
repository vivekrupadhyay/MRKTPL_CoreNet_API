using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MRKTPL.Data.Entities
{
    [Table("ProductCategoryMapping", Schema = "dbo")]
    public class ProductCategoryMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public bool IsFeaturedProduct { get; set; }
        public int DisplayOrder { get; set; }
        public virtual CategoryMaster Category { get; set; }
        public virtual ProductMaster Product { get; set; }
    }
}
