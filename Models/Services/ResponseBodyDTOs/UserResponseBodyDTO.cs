namespace BoatRecords.Models.Services.ResponseBodyDTOs;

class UserResponseBodyDTO
{
    public int id { get; set; }
    public string username { get; set; }
    public string nickname { get; set; }
    public char gender { get; set; }
    public DateTimeOffset birthday { get; set; }
}
