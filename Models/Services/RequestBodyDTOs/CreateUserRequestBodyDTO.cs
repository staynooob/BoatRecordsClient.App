using System.Runtime.Serialization;

namespace BoatRecords.Models.Services.RequestBodyDTOs;

class CreateUserRequestBodyDTO : IRequestBody
{
    public string name { get; set; }
    public string surname { get; set; }
    public string? nickname { get; set; }
    public char gender { get; set; }
    public DateOnly birthday { get; set; }
}
