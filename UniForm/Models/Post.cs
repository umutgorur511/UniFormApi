using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniForm.Models
{
    public class Post {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("TITLE")]
        public string Title { get; set; }

        [Column("USER_CONTENT")]
        public string Content{ get; set; }

        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        [Column("RECORD_STATUS")]
        public char RecordStatus { get; set; }
    }
}
