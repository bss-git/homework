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
using Tracing;

namespace Homework.Dialogs
{
    [Obsolete("Нужен для обратной совместимости старых клиентов. Удалить после перехода клиентов на апи сервиса диалогов.")]
    public class ServiceDialogsRepository : IDialogsRepository
    {

        private readonly HttpClient _httpClient;
        private readonly ConsulAddressResolver _resolver;

        public ServiceDialogsRepository(HttpClient httpClient, ConsulAddressResolver resolver)
        {
            _httpClient = httpClient;
            _resolver = resolver;
        }

        public async Task<string> GetListAsync(Guid user1, Guid user2)
        {
            var path = $"/api/dialogs/{user2}";
            var request = new HttpRequestMessage(HttpMethod.Get, await _resolver.GetAddress() + path);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JwtProvider.GetToken("", user1));
            
            request.InjectTracing();

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task SaveAsync(Message message)
        {
            var path = "/api/dialogs/message";
            var request = new HttpRequestMessage(HttpMethod.Post, await _resolver.GetAddress() + path);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JwtProvider.GetToken("", message.From));

            var body = JsonConvert.SerializeObject(new { To = message.To, Text = message.Text });
            request.Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);

            request.InjectTracing();

            await _httpClient.SendAsync(request);
        }
    }
}
