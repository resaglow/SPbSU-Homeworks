using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TrainTickets.Models
{
    public class Ticket
    {
        public long Id { get; set; }

        public int Number { get; set; }

        [Display(Name="Owner login")]
        public string OwnerLogin { get; set; }

        public string From { get; set; }
        public string To { get; set; }

        [Display(Name="Departure time")]
        public DateTime DateFrom { get; set; }
        [Display(Name="Arrival time")]
        public DateTime DateTo { get; set; }

        public decimal Price { get; set; }
    }

    public class TicketsDbContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }
        
    }
}