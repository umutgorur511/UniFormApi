using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniForm.Models
{
    [Table("Tokens")]
    public class Tokens {
        [Key]
        [Column("RecordId")]
        public long RecordId { get; set; }

        [Column("Token")]
        public string Token { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}
