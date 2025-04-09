using BoatRecords.Models.Entities;
using BoatRecords.Models.Enums;
using BoatRecords.Models.Exceptions;
using BoatRecords.Models.Services.RequestBodyDTOs;
using BoatRecords.Models.Services.ResponseBodyDTOs;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Text;
using System.Text.Json;

namespace BoatRecords.Models.Services;

class UsersRequests : BaseRestAPIService
{
    public static async Task<int> InsertNewRecord(
        string name,
        string surname,
        string? nickname,
        string gender,
        DateOnly birthDate
    ) {
        CreateUserRequestBodyDTO data = new CreateUserRequestBodyDTO();
        data.name = name;
        data.surname = surname;
        data.nickname = nickname;
        data.birthday = birthDate;
        data.gender = gender == "Female" ? 'F' : 'M';

        var serializedData = JsonSerializer.Serialize(data);
        HttpResponseMessage usersResponse = await RequestCall(RequestType.Post, "/user", serializedData);

        if (usersResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            throw new RequestFailureException("Failed to insert data");
        }

        NewUserResponseBodyDTO responseBody = JsonSerializer
            .Deserialize<NewUserResponseBodyDTO>(usersResponse.Content.ReadAsStringAsync().Result);

        if (responseBody == null)
        {
            throw new RequestFailureException("Failed to insert data");
        }

        return responseBody.id;
    }

    public static async Task<List<ICategorisableEntity>> GatAllUsers()
    {
        HttpResponseMessage usersResponse = await RequestCall(RequestType.Get, "/user");

        List<ICategorisableEntity> usersList = new List<ICategorisableEntity>();

        if (usersResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            return usersList;
        }

        List<UserResponseBodyDTO> responseBody = JsonSerializer.Deserialize<List<UserResponseBodyDTO>>(usersResponse.Content.ReadAsStringAsync().Result);

        foreach (UserResponseBodyDTO responseBodyItem in responseBody)
        {
            User user = new User
            {
                Id = responseBodyItem.id,
                Name = responseBodyItem.username,
                BirthDate = DateOnly.FromDateTime(responseBodyItem.birthday.Date),
                NickName = responseBodyItem.nickname,
                Gender = responseBodyItem.gender == 'F' ? Gender.Female : Gender.Male
            };

            usersList.Add(user);
        }

        return usersList;
    }

    public static async Task UpdateUser(string name, string surname, string? nickName, User user)
    {
        UpdateUserRequestBodyDTO data = new UpdateUserRequestBodyDTO() {
            name = name,
            surname = surname,
            nickname = nickName
        };

        HttpResponseMessage boatsResponse = await RequestCall(
            RequestType.Put, "/user/" + user.Id,
            JsonSerializer.Serialize(data)
        );

        if (boatsResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            throw new RequestFailureException("Failed to update data");
        }
    }

    internal static async Task DeleteRecord(User user)
    {
        HttpResponseMessage userResponse = await RequestCall(RequestType.Delete, "/user/" + user.Id);

        if (userResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            throw new RequestFailureException("Failed to delete data");
        }
    }

    public static async Task<bool> SignInUser(string userName, string password)
    {
        LoginRequestBodyDTO credentials = new LoginRequestBodyDTO();
        credentials.email = userName;
        credentials.password = password;

        HttpResponseMessage response = await RequestCall(
            RequestType.Post,
            "/auth/login",
            JsonSerializer.Serialize(credentials)
        );

        return response is { StatusCode: System.Net.HttpStatusCode.OK };
    }

}
