using BoatRecords.Models.Entities;
using BoatRecords.Models.Enums;
using BoatRecords.Models.Exceptions;
using BoatRecords.Models.Services.RequestBodyDTOs;
using BoatRecords.Models.Services.ResponseBodyDTOs;
using System.Text.Json;

namespace BoatRecords.Models.Services;

class BoatsRequests : BaseRestAPIService
{
    public static async Task<List<ICategorisableEntity>> GatAllBoats()
    {
        HttpResponseMessage boatsResponse = await RequestCall(RequestType.Get, "/boat");

        List<ICategorisableEntity> boatsList = new List<ICategorisableEntity>();

        if (boatsResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            return boatsList;
        }

        List<BoatResponseBodyDTO> responseBody = JsonSerializer.Deserialize<List<BoatResponseBodyDTO>>(boatsResponse.Content.ReadAsStringAsync().Result);

        foreach (BoatResponseBodyDTO responseBodyItem in responseBody)
        {
            Boat boat = new Boat
            {
                Id = responseBodyItem.id,
                Name = responseBodyItem.name,
                IsCoxed = responseBodyItem.coxed,
                SeatsNumber = responseBodyItem.seats,
                Paired = responseBodyItem.paired,
            };

            boatsList.Add(boat);
        }

        return boatsList;
    }

    internal static async Task<int> InsertNewRecord(
        string name,
        int seatsNumber,
        bool isCoxed,
        bool isPaired
    ) {
        CreateBoatRequestBodyDTO data = new CreateBoatRequestBodyDTO();
        data.name = name;
        data.seats = seatsNumber;
        data.coxed = isCoxed;
        data.paired = isPaired;

        var serializedData = JsonSerializer.Serialize(data);
        HttpResponseMessage usersResponse = await RequestCall(RequestType.Post, "/boat", serializedData);

        if (usersResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            throw new RequestFailureException("Failed to insert data");
        }

        NewBoatResponseBodyDTO responseBody = JsonSerializer
            .Deserialize<NewBoatResponseBodyDTO>(usersResponse.Content.ReadAsStringAsync().Result);

        if (responseBody == null)
        {
            throw new RequestFailureException("Failed to insert data");
        }

        return responseBody.id;
    }

    internal static async Task UpdateRecord(Boat boat,string name) {
        UpdateBoatRequestBodyDTO data = new UpdateBoatRequestBodyDTO() { name = name };

        HttpResponseMessage boatsResponse = await RequestCall(
            RequestType.Put, "/boat/" + boat.Id,
            JsonSerializer.Serialize(data)
        );

        if (boatsResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            throw new RequestFailureException("Failed to update data");
        }
    }

    internal static async Task DeleteRecord(Boat boat) {
        HttpResponseMessage boatsResponse = await RequestCall(RequestType.Delete, "/boat/" + boat.Id);

        if (boatsResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            throw new RequestFailureException("Failed to delete data");
        }
    }
}
