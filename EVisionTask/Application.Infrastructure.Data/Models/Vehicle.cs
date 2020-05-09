using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Infrastructure.Data.Interfaces;

namespace Application.Infrastructure.Data.Models
{
    public class Vehicle : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public string VehicleId { get; set; }
        public string RegNumber { get; set; }
        public bool Status { get; set; }


        [ForeignKey("CustomerId")]
        public Customers Customer { get; set; }
        public int CustomerId { get; set; }
    }
}
