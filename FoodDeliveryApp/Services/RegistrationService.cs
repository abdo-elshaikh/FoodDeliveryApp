using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;

namespace FoodDeliveryApp.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RegistrationService> _logger;

        private readonly string _captchaSecretKey;
        private readonly int _maxAttemptsPerIp;
        private readonly int _maxAttemptsPerEmail;
        private readonly int _rateLimitWindowSeconds;
        private readonly int _tokenTtlSeconds;

        public RegistrationService(
            IConnectionMultiplexer redis,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<RegistrationService> logger)
        {
            _redis = redis;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;

            _captchaSecretKey = _configuration["Captcha:SecretKey"];
            _maxAttemptsPerIp = int.Parse(_configuration["Registration:MaxAttemptsPerIp"] ?? "5");
            _maxAttemptsPerEmail = int.Parse(_configuration["Registration:MaxAttemptsPerEmail"] ?? "3");
            _rateLimitWindowSeconds = int.Parse(_configuration["Registration:RateLimitWindowSeconds"] ?? "3600");
            _tokenTtlSeconds = int.Parse(_configuration["Registration:TokenTtlSeconds"] ?? "86400"); // 24 hours
        }

        public async Task<bool> IsRegistrationAllowedAsync(string email, string ipAddress, string captchaToken)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(ipAddress) || string.IsNullOrEmpty(captchaToken))
            {
                _logger.LogWarning("Invalid input for registration check: Email={Email}, IP={IpAddress}, CaptchaToken={CaptchaToken}", email, ipAddress, captchaToken);
                return false;
            }

            try
            {
                // Verify CAPTCHA
                if (!await VerifyCaptchaAsync(captchaToken))
                {
                    _logger.LogWarning("CAPTCHA verification failed for IP {IpAddress}", ipAddress);
                    return false;
                }

                var db = _redis.GetDatabase();

                // Check IP-based rate limit
                var ipKey = $"RateLimit:IP:{ipAddress}";
                var ipCount = await db.StringIncrementAsync(ipKey);
                if (ipCount == 1)
                {
                    await db.KeyExpireAsync(ipKey, TimeSpan.FromSeconds(_rateLimitWindowSeconds));
                }
                if (ipCount > _maxAttemptsPerIp)
                {
                    _logger.LogWarning("IP {IpAddress} exceeded registration limit: {Count}/{Max}", ipCount, _maxAttemptsPerIp);
                    return false;
                }

                // Check email-based rate limit
                var emailKey = $"RateLimit:Email:{email.ToLowerInvariant()}";
                var emailCount = await db.StringIncrementAsync(emailKey);
                if (emailCount == 1)
                {
                    await db.KeyExpireAsync(emailKey, TimeSpan.FromSeconds(_rateLimitWindowSeconds));
                }
                if (emailCount > _maxAttemptsPerEmail)
                {
                    _logger.LogWarning("Email {Email} exceeded registration limit: {Count}/{Max}", email, emailCount, _maxAttemptsPerEmail);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking registration allowance for Email={Email}, IP={IpAddress}", email, ipAddress);
                return false;
            }
        }

        public async Task<string> GenerateRegistrationTokenAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }

            try
            {
                var token = GenerateSecureToken();
                var db = _redis.GetDatabase();
                var tokenKey = $"RegistrationToken:{userId}";

                await db.StringSetAsync(tokenKey, token, TimeSpan.FromSeconds(_tokenTtlSeconds));
                _logger.LogInformation("Generated registration token for user {UserId}", userId);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating registration token for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ValidateRegistrationTokenAsync(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Invalid token validation attempt: UserId={UserId}, Token={Token}", userId, token);
                return false;
            }

            try
            {
                var db = _redis.GetDatabase();
                var tokenKey = $"RegistrationToken:{userId}";
                var storedToken = await db.StringGetAsync(tokenKey);

                if (storedToken.IsNullOrEmpty)
                {
                    _logger.LogWarning("No registration token found for user {UserId}", userId);
                    return false;
                }

                if (storedToken != token)
                {
                    _logger.LogWarning("Invalid registration token for user {UserId}", userId);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating registration token for user {UserId}", userId);
                return false;
            }
        }

        public async Task RemoveRegistrationTokenAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Invalid user ID for token removal: {UserId}", userId);
                return;
            }

            try
            {
                var db = _redis.GetDatabase();
                var tokenKey = $"RegistrationToken:{userId}";
                await db.KeyDeleteAsync(tokenKey);
                _logger.LogInformation("Removed registration token for user {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing registration token for user {UserId}", userId);
            }
        }

        private async Task<bool> VerifyCaptchaAsync(string captchaToken)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://www.google.com/recaptcha/api/siteverify");
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", _captchaSecretKey),
                    new KeyValuePair<string, string>("response", captchaToken)
                });
                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var captchaResult = JsonSerializer.Deserialize<CaptchaResponse>(responseContent);

                if (captchaResult == null || !captchaResult.Success)
                {
                    _logger.LogWarning("CAPTCHA verification failed: {ErrorCodes}", string.Join(", ", captchaResult?.ErrorCodes ?? Array.Empty<string>()));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying CAPTCHA token");
                return false;
            }
        }

        private string GenerateSecureToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        private class CaptchaResponse
        {
            public bool Success { get; set; }
            public string[] ErrorCodes { get; set; }
        }
    }
}