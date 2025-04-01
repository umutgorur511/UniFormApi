namespace UniForm.Entity
{
    public class UserDto {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime CreateDate { get; set; }
        public char RecordStatus { get; set; }
        public string AccessToken { get; set; }
    }
}
