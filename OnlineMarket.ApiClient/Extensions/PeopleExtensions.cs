using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Functional;
using OnlineMarket.ApiClient.Contracts.Requests;
using OnlineMarket.ApiClient.Contracts.Responses;

namespace OnlineMarket.ApiClient.Extensions;

public static class PeopleExtensions
{
    public static async Task<AuthorizedModel?> Authorize(this OnlineMarketApiClient api, string login, string password)
    {
        using var response = await api.HttpClient.GetAsync($"People/Authorization?login={login}&password={password}");

        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<AuthorizedModel>()
            : null;
    }

    public static async Task<IEnumerable<Error>> Registr(this OnlineMarketApiClient api, PersonReg person)
    {
        using var jsonContent = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");
        using var response = await api.HttpClient.PostAsync("People/Registration", jsonContent);

        return response.IsSuccessStatusCode
            ? Enumerable.Empty<Error>()
            : (await response.Content.ReadFromJsonAsync<List<Error>>())!;
    }

    public static async Task Recover(this OnlineMarketApiClient api, string login)
    {
        using var response = await api.HttpClient.PostAsync($"People/RecoveryPassword?login={login}",
            new StringContent(""));
    }

    public static async Task<Person?> GetMe(this OnlineMarketApiClient api)
    {
        using var response = await api.HttpClient.GetAsync("People/GetMe");
        return await response.Content.ReadFromJsonAsync<Person>();
    }

    public static async Task<Result<Person, IEnumerable<Error>>> PersonUpdate(this OnlineMarketApiClient api,
        EditPersonModel person)
    {
        using var jsonContent = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");
        using var response = await api.HttpClient.PutAsync("People/Update", jsonContent);

        return response.IsSuccessStatusCode
            ? new Ok<Person>((await response.Content.ReadFromJsonAsync<Person>())!)
            : new Error<IEnumerable<Error>>((await response.Content.ReadFromJsonAsync<List<Error>>())!);
    }

    public static async Task<Person?> UpdatePersonPhoto(this OnlineMarketApiClient api, EditPersonModel model)
    {
        if (model.Photo is null) return null;

        var stream = model.Photo.OpenReadStream();
        var multipart = new MultipartFormDataContent
        {
            { new StreamContent(stream), "photo", model.Photo.FileName }
        };

        using var response = await api.HttpClient.PutAsync("People/Photo", multipart);

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<Person>();

        return null;
    }

    public static async Task<int> GetCountOfClient(this OnlineMarketApiClient api)
    {
        using var response = await api.HttpClient.GetAsync("People/GetCountOfClient");

        var count = Convert.ToInt32(await response.Content.ReadAsStringAsync());
        return count;
    }

    public static async Task<List<Person>?> GetPeople(this OnlineMarketApiClient api)
    {
        using var response = await api.HttpClient.GetAsync("People/GetPeople");
        return await response.Content.ReadFromJsonAsync<List<Person>>();
    }

    public static async Task<bool> DeletePeople(this OnlineMarketApiClient api, string login)
    {
        using var response = await api.HttpClient.DeleteAsync($"People/Delete?login={login}");
        return response.IsSuccessStatusCode;
    }
}