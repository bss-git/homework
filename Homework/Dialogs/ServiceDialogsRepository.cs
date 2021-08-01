using Auth;
using Homework.Dialogs.Application;
using Homework.Dialogs.Application.Dto;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Dialogs
{
    [Obsolete("Нужен для обратной совместимости старых клиентов. Удалить после перехода клиентов на апи сервиса диалогов.")]
    public class ServiceDialogsRepository : IDialogsRepository
    {
        private readonly string _baseUri;
        private readonly HttpClient _httpClient;

        public ServiceDialogsRepository(IOptions<DialogsOptions> options, HttpClient httpClient)
        {
            _baseUri = $"http://{options.Value.Host}:{options.Value.Port}";
            _httpClient = httpClient;
        }

        public async Task<string> GetListAsync(Guid user1, Guid user2)
        {
            var path = $"/api/dialogs/{user2}";
            var request = new HttpRequestMessage(HttpMethod.Get, _baseUri + path);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JwtProvider.GetToken("", user1));
            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public Task SaveAsync(Message message)
        {
            var path = "/api/dialogs/message";
            var request = new HttpRequestMessage(HttpMethod.Post, _baseUri + path);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JwtProvider.GetToken("", message.From));

            var body = JsonConvert.SerializeObject(new { To = message.To, Text = message.Text });
            request.Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);

            return _httpClient.SendAsync(request);
        }
    }
}
