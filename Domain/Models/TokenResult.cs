namespace Domain.Models
{
    public class TokenResult
    {
        public bool Succeeded { get; set; }
        public int StatusCode { get; set; }
        public string? Token { get; set; }

        public bool IsAdmin { get; set; }

        public string? AdminKey { get; set; }

        public string? Error { get; set; }

        public static TokenResult Ok(string? token = null, bool isAdmin = false, string? adminKey = null) =>
            new() { Succeeded = true, StatusCode = 200, Token = token, IsAdmin = isAdmin, AdminKey = adminKey };

        public static TokenResult Unauthorized(string? error = "Authorization not valid") =>
      new() { Succeeded = false, StatusCode = 401, Error = error };
    }
}