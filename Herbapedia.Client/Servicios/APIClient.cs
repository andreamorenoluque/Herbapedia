
using Herbapedia.Model;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Herbapedia.Client.Servicios
{
    public class APIClient: IDisposable
    {

        private readonly HttpClient _httpClient;

        private const string BaseUrl = "http://10.0.2.2:5208/api/";
        //private const string BaseUrl = "http://localhost:5208//api";

        public APIClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<T?> GetObject<T>(string endpoint)
        {
            try
            {
                Uri uri = new Uri(endpoint, UriKind.Relative);
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(json))
                        {
                            return default(T);
                        }
                        T? newObject = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        return newObject;
                    }
                    catch (Exception ex)
                    {
                        return default(T?);
                    }
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception e)
            {

                return default(T);
            }
        }
        public async Task<List<T>?> GetObjects<T>(string endpoint,HttpHeaders customHeaders)
        {
            try
            {
                var url = BaseUrl + endpoint;
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return result;
                }
                else
                {
                    return null; 
                }
            }
            catch (Exception)
            {
                return null; 
            }
        }
        public async Task<List<T>?> GetObjects<T>(string endpoint)
        {
            try
            {
                var url = BaseUrl + endpoint;
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<T?> PostObject<T>(string endpoint, T objeto) where T : new ()
        {
            try
            {
                StringContent content = new StringContent(JsonSerializer.Serialize(objeto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpoint,content);
                //var response = await _httpClient.PostAsync(endpoint,content);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        T? newObject = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        return newObject;
                    }
                    catch
                    {
                        return new T();
                    }
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception e)
            {

                return default(T);
            }
        }
        public async Task<List<T>?> PostObject<T>(string endpoint, T objeto, HttpHeaders customHeaders)
        {
            try
            {
                var url = BaseUrl + endpoint;
                StringContent content = new StringContent(JsonSerializer.Serialize(objeto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    List<T>? newObs = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return newObs;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
        public async Task<T?> PutObject<T>(string endpoint, T objeto)
        {
            try
            {
                var url = BaseUrl + endpoint;
                StringContent content = new StringContent(JsonSerializer.Serialize(objeto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    T? newUser = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return newUser;
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception)
            {

                return default(T);
            }
        }
        public async Task<List<T>?> PutObject<T>(string endpoint, T objeto, HttpHeaders customHeaders)
        {
            try
            {
                var url = BaseUrl + endpoint;
                StringContent content = new StringContent(JsonSerializer.Serialize(objeto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    List<T>? newObs = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return newObs;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
