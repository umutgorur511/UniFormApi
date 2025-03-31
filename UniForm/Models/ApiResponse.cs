﻿namespace UniForm.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
