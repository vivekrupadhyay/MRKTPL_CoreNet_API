using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MRKTPL.Data.Entities
{
    [Table("RoleRightsMaster", Schema = "dbo")]
    public class RoleRightsMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleRightsMappingID { get; set; }
        public int RoleID { get; set; }
        public int UserID { get; set; }
        public bool IsAssign { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
