using PaymentPicPay.API.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PaymentPicPay.API.Services.Externals.EmailSendService
{
    public class EmailSendService : IEmailSendService
    {
        private readonly static string URL_EXTERNAL_AUTHORIZER = "https://run.mocky.io/v3/54dc2cf1-3add-45b5-b5a9-6bf7e7f1f4a6";

        private readonly IHttpClientFactory _httpClientFactory;
        public EmailSendService(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task SendNotificationTransaction(User user)
        {
            var httpClient = _httpClientFactory.CreateClient();

            // Simulando um logger com Console.WriteLine
            Console.WriteLine($"URL serviço externo: {URL_EXTERNAL_AUTHORIZER}");

            var httpResponseMessage = await httpClient.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri(URL_EXTERNAL_AUTHORIZER)
            });


            Console.WriteLine(JsonSerializer.Serialize(httpResponseMessage));

            if (httpResponseMessage != null && httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<ResponseEmailSend>();

                Console.WriteLine(JsonSerializer.Serialize(response));

                if (response != null && response.Message)
                    Console.WriteLine("Notificação enviada com sucesso.");
            }
        }
    }

    public class ResponseEmailSend
    {
        [JsonPropertyName("message")]
        public bool Message { get; set; }

    }
}
