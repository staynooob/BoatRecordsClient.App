namespace BoatRecords.Models.Services.RequestBodyDTOs;

class RecordPostBodyDTO : IRequestBody
{
    public Dictionary<string, int> users = new Dictionary<string, int>();
    public int distance { get; set; }
    public DateOnly date { get; set; }
    public int boat { get; set; }
}
