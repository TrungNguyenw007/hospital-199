using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazorise;
using Contract;
using Contract.Identity.UserManager;
using Contract.Parts;
using Core.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Radzen;
using WebClient.Exceptions;
using WebClient.Identity;

namespace WebClient.RequestHttp
{
    public static class RequestClient
    {
        private readonly static HttpClient _client;

        //intergated service
        private static ILocalStorageService _localStorage;


        static RequestClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(AppSetting.API_Base_ADRESS);
        }

        public static void SetLocalStorageService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }
        

     
        public static void AttachToken(string Token = "")
        {
            if (!string.IsNullOrEmpty(Token))
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }
        }


        public static async Task<T> GetAPIAsync<T>([Required] string URL)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage = await _client.GetAsync(URL);
            return await ReturnApiResponse<T>(httpResponseMessage);
        }
        
        
        public static async Task<T> GetWithPassingContentAPI<T>([Required] string URL, dynamic input)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
             
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
          
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_client.BaseAddress + URL),
                Content = new StringContent(input, Encoding.UTF8, MediaTypeNames.Application.Json),
            };
            httpResponseMessage = await _client.SendAsync(request);
            return await ReturnApiResponse<T>(httpResponseMessage);
        }

        public static async Task<T> PostAPIAsync<T>([Required] string URL, dynamic input,bool notifyOk = true)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

           
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");

            httpResponseMessage = await _client.PostAsync(URL, content);
            return await ReturnApiResponse<T>(httpResponseMessage);
        }
        
        
        public static async Task<T> PostAPIWithFileAsync<T>([Required] string URL, IBrowserFile file)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            using (var ms = new MemoryStream())
            {
                await file.OpenReadStream(20 * 1024 * 1024).CopyToAsync(ms);

                ms.Seek(0, SeekOrigin.Begin);
                using var content = new MultipartFormDataContent
                {
                    { new StreamContent(ms), "file", file.Name }
                };
            
                httpResponseMessage = await _client.PostAsync(URL, content);
                return await ReturnApiResponse<T>(httpResponseMessage);
            }
            
        }


      
        
        public static async Task<T> PutAPIAsync<T>([Required] string URL, dynamic input)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            httpResponseMessage = await _client.PutAsync(URL, content);
            return await ReturnApiResponse<T>(httpResponseMessage);
        }

        public static async Task<T> DeleteAPIAsync<T>([Required] string URL)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage = await _client.DeleteAsync(URL);
            return await ReturnApiResponse<T>(httpResponseMessage);
        }


        private static async Task<T> ReturnApiResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string? jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync() ?? null;
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            

            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                return await HandleForUnauthorization<T>(httpResponseMessage);
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
            { 
                throw new ServerErrorException("Server-Error");
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
            {
                string? jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync() ?? null;
               var response =  JsonConvert.DeserializeObject<ResponseApi>(jsonResponse);
               throw new BadRequestException(response.message);
            }

            throw new Exception("AAAAAAAAAAAAA");
        }


        private static async Task<T> HandleForUnauthorization<T>(HttpResponseMessage httpResponseMessage)
        {
            var request = httpResponseMessage.RequestMessage.Clone();
            var accessToken = await _localStorage.GetItemAsync<string>("my-access-token");
            var refreshToken = await _localStorage.GetItemAsync<string>("my-refresh-token");
            var tokenModel = new TokenModel() {AccessToken = accessToken, RefreshToken = refreshToken};
            var response = await PostAPIAsync<ApiIdentityResponse>("user/refresh-token", tokenModel,false);

            AttachToken(response.AccessToken);
            await _localStorage.SetItemAsync("my-access-token", response.AccessToken);
            await _localStorage.SetItemAsync("my-refresh-token", response.RefreshToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.AccessToken);
            var responseMessage = await _client.SendAsync(request);


            if (responseMessage.IsSuccessStatusCode)
            {
                string? jsonResponse = await responseMessage.Content.ReadAsStringAsync() ?? null;
                return JsonConvert.DeserializeObject<T>(jsonResponse);
                ;
            }
            
            if (httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new ServerErrorException("error-server");
            }

            throw new Exception("AAAAAAAAAAAAA");
        }
    }

    public class ResponseApi
    {
        public string message { get; set; }
        public int status { get; set; }
    }
  
}