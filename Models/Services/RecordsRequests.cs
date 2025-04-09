using BoatRecords.Models.Entities;
using BoatRecords.Models.Enums;
using BoatRecords.Models.Exceptions;
using BoatRecords.Models.Services.RequestBodyDTOs;
using BoatRecords.Models.Services.ResponseBodyDTOs;
using System.Text.Json;

namespace BoatRecords.Models.Services;

class RecordsRequests : BaseRestAPIService
{
    public static async Task<string> InsertNewRecord(
        List<User> users,
        int distance,
        DateOnly date,
        Boat boat
    ) {
        RecordPostBodyDTO data = new RecordPostBodyDTO();
        data.date = date;
        data.distance = distance;
        data.boat = boat.Id;
        int index = 0;

        foreach (User user in users)
        {
            string position = boat.IsCoxed && (users.Count() - 1) == index ? "C" : index.ToString();
            data.users.Add(position, user.Id);
            index++;
        }

        var serializationOptions = new JsonSerializerOptions { IncludeFields = true };
        var serializedData = JsonSerializer.Serialize(data, serializationOptions);
        HttpResponseMessage recordsResponse = await RequestCall(RequestType.Post, "/record", serializedData);

        if (recordsResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            throw new RequestFailureException("Failed to insert data");
        }

        NewRecordResponseBodyDTO responseBody = JsonSerializer
            .Deserialize<NewRecordResponseBodyDTO>(recordsResponse.Content.ReadAsStringAsync().Result);

        if (responseBody == null)
        {
            throw new RequestFailureException("Failed to insert data");
        }

        return responseBody.unificator;
    }

    public static async Task<string> ChangeRecord(
        List<User> users,
        int distance,
        DateOnly date,
        Boat boat,
        Record record
    ) {
        RecordPostBodyDTO data = new RecordPostBodyDTO();
        data.date = date;
        data.distance = distance;
        data.boat = boat.Id;
        int index = 0;

        foreach (User user in users)
        {
            string position = boat.IsCoxed && (users.Count() - 1) == index ? "C" : index.ToString();
            data.users.Add(position, user.Id);
            index++;
        }

        var serializationOptions = new JsonSerializerOptions { IncludeFields = true };
        var serializedData = JsonSerializer.Serialize(data, serializationOptions);
        HttpResponseMessage recordsResponse = await RequestCall(RequestType.Put, "/record/" + record.RideUnificator, serializedData);

        if (recordsResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            throw new RequestFailureException("Failed to insert data");
        }

        NewRecordResponseBodyDTO responseBody = JsonSerializer
            .Deserialize<NewRecordResponseBodyDTO>(recordsResponse.Content.ReadAsStringAsync().Result);

        if (responseBody == null)
        {
            throw new RequestFailureException("Failed to insert data");
        }

        return responseBody.unificator;
    }

    public static async Task<List<Record>> GatAllRecords(DateTime dayOfRide)
    {
        HttpResponseMessage recordsResponse = await RequestCall(RequestType.Get, "/record/" + dayOfRide.ToString("yyyy-MM-dd"));
        List<Record> recordsList = new List<Record>();

        if (recordsResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            return recordsList;
        }

        var p = recordsResponse.Content.ReadAsStringAsync().Result;

        List<RecordResponseBodyDTO> responseBody = JsonSerializer.Deserialize<List<RecordResponseBodyDTO>>(p);

        Record? lastRecord = null;
        List<string> coxedRecords = new List<string>();

        foreach (RecordResponseBodyDTO responseBodyItem in responseBody)
        {
            if (responseBodyItem.as_cox)
            {
                coxedRecords.Add(responseBodyItem.ride_unificator);
            }

            if (lastRecord?.RideUnificator != responseBodyItem.ride_unificator)
            {
                if (lastRecord != null)
                {
                    recordsList.Add(lastRecord);
                }

                lastRecord = new Record();
            }

            lastRecord.RideUnificator = responseBodyItem.ride_unificator;
            lastRecord.DateOfRide = DateTime.Parse(responseBodyItem.ride_date);
            lastRecord.Distance = responseBodyItem.distance;

            Boat boat = new Boat { 
                Id = responseBodyItem.boat_id,
                Name = responseBodyItem.boat_name,
                Paired = responseBodyItem.is_paired
            };

            lastRecord.Boat = boat;
            User user = new User {
                Id = responseBodyItem.user_id,
                Name = responseBodyItem.user,
                BirthDate = responseBodyItem.birthday,
                Gender = responseBodyItem.gender == 'M' ? Gender.Male : Gender.Female,
                NickName = responseBodyItem.nickname
            };
            lastRecord.Crew.Add(user);
        }

        if (lastRecord != null)
        {
            recordsList.Add(lastRecord);
        }

        UpdateBoatSeating(coxedRecords, recordsList);
        return recordsList;
    }

    public static async Task DeleteRecord(Record record)
    {
        HttpResponseMessage boatsResponse = await RequestCall(RequestType.Delete, "/record/" + record.RideUnificator);

        if (boatsResponse is not { StatusCode: System.Net.HttpStatusCode.OK })
        {
            throw new RequestFailureException("Failed to delete data");
        }
    }

    private static void UpdateBoatSeating(List<string> coxedRecords, List<Record> recordsList)
    {
        foreach (Record record in recordsList)
        {
            int seats = record.Crew.Count();
            bool isCoxed = coxedRecords.Find(unificator => unificator == record.RideUnificator) != null;

            if (isCoxed)
            {
                seats--;
            }

            record.Boat.SeatsNumber = seats;
            record.Boat.IsCoxed = isCoxed;
        }
    }
}
