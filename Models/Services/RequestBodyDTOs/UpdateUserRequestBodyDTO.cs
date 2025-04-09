namespace BoatRecords.Models.Services.RequestBodyDTOs;

class UpdateUserRequestBodyDTO : IRequestBody
{
    public string name { get; set; }
    public string surname { get; set; }
    public string? nickname { get; set; }
}
