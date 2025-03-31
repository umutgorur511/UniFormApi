using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniForm.Enum;

namespace UniForm.Models
{
    public class Action {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("POST_ID")]
        public int PostId { get; set; }

        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("ACTION_TYPE")]
        public ActionType ActionType { get; set; }

        [Column("ACTION_DATE")]
        public DateTime ActionDate { get; set; }

        [Column("RECORD_STATUS")]
        public char RecordStatus { get; set; }
    }
}
