using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoatRecords.Models.Services.ResponseBodyDTOs;

class LoginResponseBodyDTO
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expiress_in { get; set; }
}
