using Mango.Web.Models;
using Mango.Web.Services.IServices;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        public RespoceDTO responceModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responceModel = new RespoceDTO();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MangoAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if (apiRequest.Data != null) {
                    var content = JsonConvert.SerializeObject(apiRequest.Data);
                    message.Content = new StringContent(content, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage apiResponce = null;
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST: message.Method = HttpMethod.Post; break;
                    case SD.ApiType.PUT: message.Method = HttpMethod.Put; break;
                    case SD.ApiType.DELETE: message.Method = HttpMethod.Delete; break;
                    default: message.Method = HttpMethod.Get; break;
                }

                apiResponce = await client.SendAsync(message);

                var apiContent = await apiResponce.Content.ReadAsStringAsync();
                var apiResponceDto = JsonConvert.DeserializeObject<T>(apiContent);

                return apiResponceDto;
            }
            catch (Exception ex)
            {
                var dto = new RespoceDTO
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var apiResponecDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponecDto;
            };
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
