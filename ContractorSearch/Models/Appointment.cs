using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContractorSearch.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public DateTime ApptDate { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string DeliveryMethod { get; set; }
        public double Amount { get; set; } //changed from Amound (might have been typo)
        public int Rating { get; set; }
        public string Review { get; set; }
        public bool ReservedAppointment { get; set; } //added this
        [ForeignKey("Contractor")]
        public int ContractorId { get; set; }
        public Contractor Contractor { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
   
    }

