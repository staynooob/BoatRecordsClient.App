using System.Runtime.Serialization;

namespace BoatRecords.Models.Services.RequestBodyDTOs;

class LoginRequestBodyDTO : IRequestBody
{
    public string email { get; set; }
    public string password { get; set; }
}
