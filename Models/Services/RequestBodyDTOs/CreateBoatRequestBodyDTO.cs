namespace BoatRecords.Models.Services.RequestBodyDTOs;

class CreateBoatRequestBodyDTO : IRequestBody
{
    public string name { get; set; }
    public int seats { get; set; }
    public bool coxed { get; set; }
    public bool paired { get; set; }
}
