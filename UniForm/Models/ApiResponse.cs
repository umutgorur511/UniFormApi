namespace UniForm.Models
{
    public class ApiResponse<T>
    {
        public bool Succes { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
