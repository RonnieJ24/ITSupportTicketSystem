using System;
using System.Linq;
using System.Web.Mvc;
using ITSupportTicketSystem.Models;
using System.Data.Entity; // Import this namespace
using Newtonsoft.Json;


public class TicketController : Controller
{
    private SupportContext db = new SupportContext();

    // GET: Ticket/Create
    public ActionResult Create()
    {
        ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Ticket ticket)
    {
        if (ModelState.IsValid)
        {
            ticket.CreatedAt = DateTime.Now; // Set CreatedAt to the current date and time
            ticket.DateSubmitted = DateTime.Now; // Set DateSubmitted to the current date and time

            // Fetch the requester's name based on the RequestedByEmployeeID and set it to RequesterName
            var employee = db.Employees.Find(ticket.RequestedByEmployeeID);
            if (employee != null)
            {
                ticket.RequesterName = employee.Name;
            }

            db.Tickets.Add(ticket); // Add the ticket to the database
            db.SaveChanges(); // Save changes to the database
            return RedirectToAction("ReceivedTickets"); // Redirect to the ReceivedTickets view
        }

        // If we get to this point, something went wrong; redisplay form
        ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", ticket.DepartmentID);
        return View(ticket);
    }


    // This action gets called by JavaScript to populate the "Requested By" dropdown
    public JsonResult GetEmployeesByDepartment(int departmentId)
    {
        var employees = db.Employees.Where(e => e.DepartmentID == departmentId).Select(e => new
        {
            Value = e.EmployeeID,
            Text = e.Name
        }).ToList();

        return Json(employees, JsonRequestBehavior.AllowGet);
    }

    
    public ActionResult RemoveTicket(int id)
    {
        // Find the ticket by id and remove it from the database
        var ticket = db.Tickets.Find(id);
        if (ticket != null)
        {
            db.Tickets.Remove(ticket);
            db.SaveChanges();
        }

        // Redirect back to the ReceivedTickets view
        return RedirectToAction("ReceivedTickets");
    }
    public ActionResult ReceivedTickets(string searchQuery, string sortOrder)
    {
        var tickets = db.Tickets.Include(t => t.Department).AsQueryable();

        // Filtering logic based on the searchQuery
        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower(); // Convert to lowercase for case-insensitive search
            tickets = tickets.Where(t =>
                t.ProjectName.ToLower().Contains(searchQuery) ||
                t.Department.Name.ToLower().Contains(searchQuery) ||
                t.Description.ToLower().Contains(searchQuery) ||
                t.RequestedBy.ToString().ToLower().Contains(searchQuery) // Convert RequestedBy to string
            );

            // Check if searchQuery is a valid date and filter by DateReceived if so
            DateTime dateReceived;
            if (DateTime.TryParse(searchQuery, out dateReceived))
            {
                tickets = tickets.Where(t => t.DateSubmitted.Date == dateReceived.Date);
            }
        }

        // Sorting logic
        ViewBag.DateSortParm = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
        ViewBag.ProjectNameSortParm = sortOrder == "ProjectName" ? "projectName_desc" : "ProjectName";
        ViewBag.DepartmentSortParm = sortOrder == "Department" ? "department_desc" : "Department";
        ViewBag.RequestorSortParm = sortOrder == "Requestor" ? "requestor_desc" : "Requestor";

        switch (sortOrder)
        {
            case "projectName_desc":
                tickets = tickets.OrderByDescending(t => t.ProjectName);
                break;
            case "ProjectName":
                tickets = tickets.OrderBy(t => t.ProjectName);
                break;
            case "department_desc":
                tickets = tickets.OrderByDescending(t => t.Department.Name);
                break;
            case "Department":
                tickets = tickets.OrderBy(t => t.Department.Name);
                break;
            case "requestor_desc":
                tickets = tickets.OrderByDescending(t => t.RequestedByEmployeeID);
                break;
            case "Requestor":
                tickets = tickets.OrderBy(t => t.RequestedByEmployeeID);
                break;
            case "date_desc":
                tickets = tickets.OrderByDescending(t => t.DateSubmitted);
                break;
            default:
                tickets = tickets.OrderBy(t => t.DateSubmitted);
                break;
        }

        return View(tickets.ToList());
    }

    public ActionResult ProjectChart()
    {
        var projectData = db.Tickets
            .GroupBy(t => t.ProjectName)
            .Select(g => new { ProjectName = g.Key, TicketCount = g.Count() })
            .ToList();

        // Retrieve requester names for each project
        var projectNames = projectData.Select(p => p.ProjectName).ToList();
        var requesterNames = db.Tickets
            .Where(t => projectNames.Contains(t.ProjectName))
            .Select(t => new { t.ProjectName, t.RequesterName })
            .Distinct()
            .ToDictionary(p => p.ProjectName, p => p.RequesterName);

        // Create a list of objects with project name, ticket count, and requester name
        var chartData = projectData.Select(p => new
        {
            ProjectName = p.ProjectName,
            TicketCount = p.TicketCount,
            RequesterName = requesterNames.ContainsKey(p.ProjectName) ? requesterNames[p.ProjectName] : ""
        });

        // Convert the data to JSON for use in JavaScript
        var chartLabels = chartData.Select(p => p.ProjectName);
        var chartTicketCounts = chartData.Select(p => p.TicketCount);
        var chartRequesterNames = chartData.Select(p => p.RequesterName);

        ViewBag.ChartLabels = JsonConvert.SerializeObject(chartLabels);
        ViewBag.ChartTicketCounts = JsonConvert.SerializeObject(chartTicketCounts);
        ViewBag.ChartRequesterNames = JsonConvert.SerializeObject(chartRequesterNames);

        return View();
    }


    // Dispose of the database context
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (db != null)
            {
                db.Dispose();
                db = null;
            }
        }
        base.Dispose(disposing);
    }
}
