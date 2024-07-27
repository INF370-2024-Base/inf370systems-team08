using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EduProfileAPI.WhatsApp
{
    public class WhatsAppHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;

        public WhatsAppHelper(string accessToken)
        {
            _httpClient = new HttpClient();
            _accessToken = accessToken;
        }

        public async Task<(bool, string)> SendMessage(string to, string message)
        {
            var url = "https://graph.facebook.com/v19.0/337116382826437/messages";

            var requestPayload = new
            {
                messaging_product = "whatsapp",
                to,
                type = "text",
                text = new { body = message }
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            try
            {
                var response = await _httpClient.SendAsync(request);

                var responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return (false, responseBody);
                }

                // Log the response body for debugging
                Console.WriteLine($"Response Body: {responseBody}");

                return (true, responseBody);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string)> SendTemplateMessage(string to, string parentName, string studentName, string messageContent)
        {
            var url = "https://graph.facebook.com/v19.0/337116382826437/messages";

            var requestPayload = new
            {
                messaging_product = "whatsapp",
                to,
                type = "template",
                template = new
                {
                    name = "message_to_parent",
                    language = new { code = "en_GB" },
                    components = new[]
                    {
                        new
                        {
                            type = "header",
                            parameters = new[]
                            {
                                new { type = "text", text = parentName }
                            }
                        },
                        new
                        {
                            type = "body",
                            parameters = new[]
                            {
                                new { type = "text", text = studentName },
                                new { type = "text", text = messageContent }
                            }
                        }
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            try
            {
                var response = await _httpClient.SendAsync(request);

                var responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return (false, responseBody);
                }

                // Log the response body for debugging
                Console.WriteLine($"Response Body: {responseBody}");

                return (true, responseBody);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
