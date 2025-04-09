namespace BoatRecords.Models.Services.ResponseBodyDTOs;

class RecordResponseBodyDTO
{
    public int user_id { get; set; }
    public DateOnly birthday { get; set; }
    public char gender { get; set; }
    public int boat_id { get; set; }
    public string boat_name { get; set; }
    public string user { get; set; }
    public string? nickname { get; set; }
    public int distance { get; set; }
    public string ride_date { get; set; }
    public string ride_unificator { get; set; }
    public bool as_cox { get; set; }
    public bool is_paired { get; set; }
}
