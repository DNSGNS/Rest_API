using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class FilterClientDto
    {
        public string? first_name { get; set; }

        public string? second_name { get; set; }


        [Range(1, 31, ErrorMessage = "День должен быть в диапазоне от 1 до 31")]
        public int? day { get; set; }

        [Range(1, 12, ErrorMessage = "Месяц должен быть в диапазоне от 1 до 12")]
        public int? month { get; set; }

        [Range(1, 3000, ErrorMessage = "Год должен быть в диапазоне от 1 до 3000")]
        public int? year { get; set; }
    }
}
