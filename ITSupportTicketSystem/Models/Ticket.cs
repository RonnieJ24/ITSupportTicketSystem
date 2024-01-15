using System;
using System.ComponentModel.DataAnnotations;

namespace ITSupportTicketSystem.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public string ProjectName { get; set; }
        public int DepartmentID { get; set; }
        public int RequestedBy { get; set; }
        public int RequestedByEmployeeID { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string RequesterName { get; set; }
        public virtual Department Department { get; set; } // Navigation property to Department

        // Add any other properties or navigation properties as needed
    }
}
