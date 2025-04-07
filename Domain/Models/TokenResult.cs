namespace Domain.Models
{
    public class TokenResult
    {
        public bool Succeeded { get; set; }
        public int StatusCode { get; set; }
        public string? Token { get; set; }

        public string? Error { get; set; }

        public static TokenResult Ok(string? token = null) =>
            new() { Succeeded = true, StatusCode = 200, Token = token };

        public static TokenResult Unauthorized(string? error = "Authorization not valid") =>
      new() { Succeeded = false, StatusCode = 401, Error = error };
    }
}