﻿namespace TangyWeb_API.Helper
{
    public class ApiSettings
    {
        public string SecretKey { get; set; }
        public string? ValidAudience { get; set; }
        public string? ValidIssuer { get; set; }
    }
}
