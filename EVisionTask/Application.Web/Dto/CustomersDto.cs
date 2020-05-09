using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Web.Dto
{
    public class CustomersDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
