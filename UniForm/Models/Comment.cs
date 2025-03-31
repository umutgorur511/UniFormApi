using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniForm.Models
{
    public class Comment {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("POST_ID")]
        public int PostId { get; set; }

        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("USER_COMMENT")]
        public string UserComment { get; set; }

        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        [Column("RECORD_STATUS")]
        public char RecordStatus { get; set; }
    }
}
