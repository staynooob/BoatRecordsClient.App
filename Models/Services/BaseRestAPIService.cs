using BoatRecords.Models.Services.RequestBodyDTOs;
using System.Text.Json;
using System.Text;
using BoatRecords.Models.Exceptions;
using BoatRecords.Models.Enums;
using System.Runtime.Serialization;
using System.Collections.Generic;
using BoatRecords.Models.Services.ResponseBodyDTOs;

namespace BoatRecords.Models.Services;

abstract class BaseRestAPIService
{
    private static string? Token { get; set; }

    private static async Task LoginCall()
    {
        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://api.vklysa.cz");

        LoginRequestBodyDTO credentials = new LoginRequestBodyDTO();
        credentials.email = "jwt-user@jwt.no";
        credentials.password = "9EChyIN0ZyqHSaJ";

        HttpResponseMessage response = await httpClient.PostAsync(
            "/auth/login",
            new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json")
        );

        if (response is not { StatusCode: System.Net.HttpStatusCode.OK})
        {
            throw new AuthenticationException(string.Format("Login request failed"));
        }

        LoginResponseBodyDTO responseBody = JsonSerializer.Deserialize<LoginResponseBodyDTO>(response.Content.ReadAsStringAsync().Result);
        Token = responseBody?.access_token;
    }

    protected static async Task<HttpResponseMessage> RequestCall(RequestType requestType, string uri, string? data)
    {
        if (Token == null)
        {
            await LoginCall();
        }

        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://api.vklysa.cz");

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token ?? "");
        return requestType switch
        {
            RequestType.Get => await httpClient.GetAsync(uri),
            RequestType.Post => await httpClient.PostAsync(uri, new StringContent(data ?? "{}", Encoding.UTF8, "application/json")),
            RequestType.Put => await httpClient.PutAsync(uri, new StringContent(data ?? "{}", Encoding.UTF8, "application/json")),
            RequestType.Delete => await httpClient.DeleteAsync(uri),
            _ => throw new NotImplementedException("Mothod is not implemented")
        };
    }

    protected static async Task<HttpResponseMessage> RequestCall(RequestType requestType, string uri)
    {
        return await RequestCall(requestType, uri, null);
    }
}
