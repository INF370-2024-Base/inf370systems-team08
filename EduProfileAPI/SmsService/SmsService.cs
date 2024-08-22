namespace EduProfileAPI.SmsService
{
    public class SmsService: ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public SmsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            string apiKey = _configuration["Clickatell:ApiKey"];
            string url = $"https://platform.clickatell.com/messages/http/send?apiKey={apiKey}&to={phoneNumber}&content={Uri.EscapeDataString(message)}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to send SMS");
            }
        }
    }
}
