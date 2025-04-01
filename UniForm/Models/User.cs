using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniForm.Models
{
    public class User {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NAME")]
        public string Name { get; set; }

        [Column("MAIL")]
        public string Email { get; set; }

        [Column("PASSWORD")]
        public string Password { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        [Column("RECORD_STATUS")]
        public char RecordStatus { get; set; }

        [Column("ACCES_TOKEN")]
        public string ?AccessToken { get; set; }

    }
}
