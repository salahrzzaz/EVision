using System;
namespace Application.Web.Dto
{
    public class VehicleGetDto
    {
        public string VehicleId { get; set; }

        public string RegNumber { get; set; }

        public string CustomerName { get; set; }

        public bool Status { get; set; }
    }
}
