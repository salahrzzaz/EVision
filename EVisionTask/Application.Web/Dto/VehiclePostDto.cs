using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Web.Dto
{
    public class VehiclePostDto
    {
        [Required]
        public string VehicleId { get; set; }

        [Required]
        public string RegNumber { get; set; }


        [Required]
        public int CustomerId { get; set; }
    }
}
