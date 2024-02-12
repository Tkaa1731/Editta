﻿namespace Pobytne.Client.Extensions
{
    public class HttpResponse<T>
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
