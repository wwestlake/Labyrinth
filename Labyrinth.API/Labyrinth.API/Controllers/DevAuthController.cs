using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FirebaseAdmin.Auth;
using System.Threading.Tasks;
using System.Collections.Generic;
using Labyrinth.API.Logging;

namespace Labyrinth.API.Controllers
{
    [ApiController]
    [Route("api/dev-auth")]
    public class DevAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly IBenchmark<DevAuthController> _benchmark;

        public DevAuthController(IConfiguration configuration, IWebHostEnvironment env, IBenchmark<DevAuthController> benchmark)
        {
            _configuration = configuration;
            _env = env;
            _benchmark = benchmark;
        }

        // Step 1: Generate a custom token with custom claims
        [HttpPost("custom-token")]
        public async Task<IActionResult> GenerateCustomToken([FromBody] CustomTokenRequest request)
        {
            if (!_env.IsDevelopment())
            {
                return NotFound();
            }
            using (_benchmark.StartScoped("GenerateCustomToken"))
            {
                // Create custom claims
                var customClaims = new Dictionary<string, object>
                    {
                        { "role", request.Role }
                    };

                // Generate a custom token using Firebase Admin SDK
                var customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(request.UserId, customClaims);

                _benchmark.AddData("UserId", request.UserId);
                _benchmark.AddData("Role", request.Role);

                return Ok(new { CustomToken = customToken });
            }
        }

        // Step 2: Exchange the custom token for an ID token
        [HttpPost("exchange-token")]
        public async Task<IActionResult> ExchangeCustomToken([FromBody] TokenExchangeRequest request)
        {
            if (!_env.IsDevelopment())
            {
                return NotFound();
            }
            using (_benchmark.StartScoped("ExchangeCustomToken"))
            {

                var firebaseApiKey = _configuration["Firebase:ApiKey"];

                using (var client = new HttpClient())
                {
                    var data = new
                    {
                        token = request.CustomToken,
                        returnSecureToken = true
                    };

                    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key={firebaseApiKey}", content);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsStringAsync();
                    var idToken = Newtonsoft.Json.Linq.JObject.Parse(result)["idToken"].ToString();

                    _benchmark.AddData("CustomToken", request.CustomToken);

                    return Ok(new { IdToken = idToken });
                }
            }
        }
    }

    public class CustomTokenRequest
    {
        public string UserId { get; set; }
        public string Role { get; set; }  // Additional custom claims can be added here
    }

    public class TokenExchangeRequest
    {
        public string CustomToken { get; set; }
    }
}
