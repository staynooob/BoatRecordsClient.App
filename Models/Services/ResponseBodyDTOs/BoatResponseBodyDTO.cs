namespace BoatRecords.Models.Services.ResponseBodyDTOs;

class BoatResponseBodyDTO
{
    public int id { get; set; }
    public string name { get; set; }
    public int seats { get; set; }
    public bool coxed { get; set; }
    public bool paired { get; set; }
}
