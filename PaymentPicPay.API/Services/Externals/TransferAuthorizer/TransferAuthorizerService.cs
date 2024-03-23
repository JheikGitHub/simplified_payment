using System.Text.Json;
using System.Text.Json.Serialization;

namespace PaymentPicPay.API.Services.Externals.TransferAuthorizer
{
    public class TransferAuthorizerService : ITransferAuthorizerService
    {
        private readonly static string URL_EXTERNAL_AUTHORIZER = "https://run.mocky.io/v3/5794d450-d2e2-4412-8131-73d0293ac1cc";

        private readonly IHttpClientFactory _httpClientFactory;
        public TransferAuthorizerService(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;

        public async Task<bool> AuthorizationTranfer()
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
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<ResponseAuthorization>();

                Console.WriteLine(JsonSerializer.Serialize(response));

                if (response != null && response.Message.Equals("Autorizado"))
                    return true;
            }

            return false;
        }

        public void Dispose() 
            => GC.SuppressFinalize(this);
    }

    public class ResponseAuthorization
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

    }
}
