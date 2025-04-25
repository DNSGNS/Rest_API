using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class UpdateClientDto
    {

        public string first_name { get; set; }

        public string second_name { get; set; }

        public DateTime birth_date { get; set; }
    }
}
