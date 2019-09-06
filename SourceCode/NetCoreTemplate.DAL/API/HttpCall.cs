// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpCall.cs" company="">
//   
// </copyright>
// <summary>
//   http client that wraps error handling, header creation and basic auth
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NetCoreTemplate.DAL.API
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    /// <summary>
    /// http client that wraps error handling, header creation and basic auth
    /// </summary>
    public class HttpCall
    {
        /// <summary>
        /// TODO The _client.
        /// </summary>
        private static HttpClient _client;

        /// <summary>
        /// TODO The json options.
        /// </summary>
        private readonly JsonSerializerSettings jsonOptions =
            new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore };

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpCall"/> class.
        /// </summary>
        public HttpCall()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// TODO The get json request.
        /// </summary>
        /// <param name="uri">
        /// TODO The uri.
        /// </param>
        /// <param name="username">
        /// TODO The username.
        /// </param>
        /// <param name="password">
        /// TODO The password.
        /// </param>
        /// <param name="additionalHeaders">
        /// TODO The additional headers.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="HttpRequestException">
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>
        public async Task<T> GetJsonRequest<T>(
            string uri,
            string username = null,
            string password = null,
            Dictionary<string, string> additionalHeaders = null)
        {
            try
            {
                _client.DefaultRequestHeaders.Clear();
                if (username != null && password != null)
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(byteArray));
                }

                if (additionalHeaders != null)
                    foreach (var item in additionalHeaders)
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = _client.GetAsync(uri).Result;
                var stringResult = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<T>(stringResult, this.jsonOptions);

                throw new HttpRequestException(
                    response.RequestMessage.RequestUri + "\n" + response.ReasonPhrase + "/n" + stringResult
                    + "statuscode: " + (int)response.StatusCode);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// TODO The get json request response.
        /// </summary>
        /// <param name="uri">
        /// TODO The uri.
        /// </param>
        /// <param name="username">
        /// TODO The username.
        /// </param>
        /// <param name="password">
        /// TODO The password.
        /// </param>
        /// <param name="additionalHeaders">
        /// TODO The additional headers.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public HttpResponseMessage GetJsonRequestResponse(
            string uri,
            string username = null,
            string password = null,
            Dictionary<string, string> additionalHeaders = null)
        {
            try
            {
                _client.DefaultRequestHeaders.Clear();
                if (username != null && password != null)
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(byteArray));
                }

                if (additionalHeaders != null)
                    foreach (var item in additionalHeaders)
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = _client.GetAsync(uri).Result;

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// TODO The post json request.
        /// </summary>
        /// <param name="uri">
        /// TODO The uri.
        /// </param>
        /// <param name="model">
        /// TODO The model.
        /// </param>
        /// <param name="username">
        /// TODO The username.
        /// </param>
        /// <param name="password">
        /// TODO The password.
        /// </param>
        /// <param name="additionalHeaders">
        /// TODO The additional headers.
        /// </param>
        /// <exception cref="HttpRequestException">
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>
        public void PostJsonRequest(
            string uri,
            object model,
            string username = null,
            string password = null,
            Dictionary<string, string> additionalHeaders = null)
        {
            try
            {
                if (username != null && password != null)
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(byteArray));
                }

                if (additionalHeaders != null)
                    foreach (var item in additionalHeaders)
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = _client.PostAsJsonAsync(uri, model).Result;

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(
                        response.RequestMessage.RequestUri + "\n" + "\nstatus code: " + (int)response.StatusCode);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// TODO The post json request.
        /// </summary>
        /// <param name="uri">
        /// TODO The uri.
        /// </param>
        /// <param name="model">
        /// TODO The model.
        /// </param>
        /// <param name="username">
        /// TODO The username.
        /// </param>
        /// <param name="password">
        /// TODO The password.
        /// </param>
        /// <param name="additionalHeaders">
        /// TODO The additional headers.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="HttpRequestException">
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>
        public async Task<T> PostJsonRequest<T>(
            string uri,
            object model,
            string username = null,
            string password = null,
            Dictionary<string, string> additionalHeaders = null)
        {
            try
            {
                _client.DefaultRequestHeaders.Clear();
                if (username != null && password != null)
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(byteArray));
                }

                if (additionalHeaders != null)
                    foreach (var item in additionalHeaders)
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = _client.PostAsJsonAsync(uri, model).Result;
                var stringResult = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<T>(stringResult, this.jsonOptions);
                throw new HttpRequestException(
                    response.RequestMessage.RequestUri + "\n" + stringResult + " \nstatus code: "
                    + (int)response.StatusCode);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// TODO The post json request response.
        /// </summary>
        /// <param name="uri">
        /// TODO The uri.
        /// </param>
        /// <param name="model">
        /// TODO The model.
        /// </param>
        /// <param name="username">
        /// TODO The username.
        /// </param>
        /// <param name="password">
        /// TODO The password.
        /// </param>
        /// <param name="additionalHeaders">
        /// TODO The additional headers.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public HttpResponseMessage PostJsonRequestResponse(
            string uri,
            object model,
            string username = null,
            string password = null,
            Dictionary<string, string> additionalHeaders = null)
        {
            try
            {
                _client.DefaultRequestHeaders.Clear();
                if (username != null && password != null)
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(byteArray));
                }

                if (additionalHeaders != null)
                    foreach (var item in additionalHeaders)
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = _client.PostAsJsonAsync(uri, model).Result;

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// TODO The post multipart request.
        /// </summary>
        /// <param name="uri">
        /// TODO The uri.
        /// </param>
        /// <param name="content">
        /// TODO The content.
        /// </param>
        /// <param name="username">
        /// TODO The username.
        /// </param>
        /// <param name="password">
        /// TODO The password.
        /// </param>
        /// <param name="additionalHeaders">
        /// TODO The additional headers.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="HttpRequestException">
        /// </exception>
        public async Task<T> PostMultipartRequest<T>(
            string uri,
            HttpContent content,
            string username = null,
            string password = null,
            Dictionary<string, string> additionalHeaders = null)
        {
            _client.DefaultRequestHeaders.Clear();
            if (username != null && password != null)
            {
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(byteArray));
            }

            if (additionalHeaders != null)
                foreach (var item in additionalHeaders)
                    _client.DefaultRequestHeaders.Add(item.Key, item.Value);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsync(uri, content);
            var stringResult = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(
                    response.RequestMessage.RequestUri + "\n" + response.ReasonPhrase + " " + stringResult
                    + "\nstatus code: " + (int)response.StatusCode);

            return JsonConvert.DeserializeObject<T>(stringResult, this.jsonOptions);
        }
    }

    public class GenericHttpResponse
    {
        public string Status { get; set; }
        public string Details { get; set; }
    }
}