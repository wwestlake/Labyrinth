using FirebaseAdmin.Auth;
using Labyrinth.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;

namespace Labyrinth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly string _firebaseApiKey;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _firebaseApiKey = configuration["Firebase:ApiKey"];
            _userService = userService;
        }

        // Existing login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var client = new HttpClient();
            var requestUri = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_firebaseApiKey}";

            var requestBody = new
            {
                email = model.Email,
                password = model.Password,
                returnSecureToken = true
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
            {
                return Unauthorized(new { error = "Invalid login attempt" });
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<FirebaseLoginResponse>(responseContent);

            return Ok(new { token = result.IdToken });
        }

        // New endpoint for setting up custom claims
        [HttpPost("set-custom-claims")]
        public async Task<IActionResult> SetCustomClaims([FromBody] SetClaimsModel model)
        {
            try
            {
                // Retrieve the user from MongoDB based on Firebase UID
                var user = await _userService.GetUserByUidAsync(model.FirebaseUid);
                if (user == null)
                {
                    return NotFound(new { error = "User not found." });
                }

                // Set custom claims
                var claims = new Dictionary<string, object>
                {
                    { "role", user.Role.ToString() }
                };

                await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(model.FirebaseUid, claims);

                // Force token refresh to get new claims
                var idToken = await GetFreshToken(model.FirebaseUid);

                return Ok(new { token = idToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error setting custom claims: {ex.Message}" });
            }
        }

        // Helper method to get a fresh token
        private async Task<string> GetFreshToken(string uid)
        {
            var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
            var customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid);

            var client = new HttpClient();
            var requestUri = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key={_firebaseApiKey}";

            var requestBody = new
            {
                token = customToken,
                returnSecureToken = true
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<FirebaseLoginResponse>(responseContent);

            return result.IdToken;
        }

        // New endpoint for refreshing token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var client = new HttpClient();
            var requestUri = $"https://securetoken.googleapis.com/v1/token?key={_firebaseApiKey}";

            var requestBody = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", model.RefreshToken }
            };

            var content = new FormUrlEncodedContent(requestBody);
            var response = await client.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
            {
                return Unauthorized(new { error = "Invalid refresh attempt" });
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<FirebaseRefreshResponse>(responseContent);

            return Ok(new { token = result.IdToken });
        }
    }

    // Models used in the controller

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SetClaimsModel
    {
        public string FirebaseUid { get; set; }
    }

    public class RefreshTokenModel
    {
        public string RefreshToken { get; set; }
    }

    public class FirebaseLoginResponse
    {
        [JsonPropertyName("idToken")]
        public string IdToken { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expiresIn")]
        public string ExpiresIn { get; set; }

        [JsonPropertyName("localId")]
        public string LocalId { get; set; }
    }

    public class FirebaseRefreshResponse
    {
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expires_in")]
        public string ExpiresIn { get; set; }
    }
}
