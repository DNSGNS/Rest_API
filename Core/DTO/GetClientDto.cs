using Core.Models;

namespace Core.DTO
{
    public class GetClientDto
    {
        public GetClientDto(Client client)
        {
            id = client.id;
            first_name = client.first_name;
            second_name = client.second_name;
            birth_date = client.birth_date;
        }

        public GetClientDto(int id, string first_name, string second_name, DateTime birth_date)
        {
            this.id = id;
            this.first_name = first_name;
            this.second_name = second_name;
            this.birth_date = birth_date;
        }

        public int id { get; set; }

        public string first_name { get; set; }

        public string second_name { get; set; }

        public DateTime birth_date { get; set; }

    }
}
