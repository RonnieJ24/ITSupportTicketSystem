# ITSupportTicketSystem

## Overview
Bell's Field Services IT Support Ticket System is a web application designed to streamline the process of requesting and managing IT support for various projects and departments within the organization. This system allows employees to submit support tickets, provides tools for IT support members to filter and manage those tickets, and includes a project overview page with a count of associated tickets.

## Requirements
### Page to Enter Ticket Details
- Responsive web page for entering ticket details.
- Required fields:
  - Project Name
  - Department Name
  - Requested by (auto-generated based on department selection)
  - Description of the problem
  - Date and Time of submission

### Page to Search Received Tickets
- IT Support Team members can filter entered tickets by:
  - Project Name
  - Department
  - Date and Time of request
  - Requestor's name
  - Keywords in the Description of Problem
- Results displayed in a sortable grid.
- All requests shown if no filters are applied.

### Page for Displaying Project Overview
- Displays a chart of all Projects with a count of associated tickets.

## Technologies Used
- .NET Framework 4 or higher
- C#
- MVC (Model-View-Controller) architecture
- Entity Framework 5 or higher
- Bootstrap for responsive design
- JQuery for client-side functionality
- SQL Server Database (SQL Server DB) for data storage
- CSS for styling

## Installation
To set up the IT Support Ticket System, follow these steps:

1. Clone the repository to your local machine: git clone https://github.com/RonnieJ24/ITSupportTicketSystem.git

2. Open the SQL script `ITSupportDB.sql` in your preferred SQL Server management tool and execute it to create the database and its objects.

3. Update the connection string in the `Web.config` file to match your SQL Server configuration.

4. Build and run the application using Visual Studio or your preferred development environment.

## Usage
- Access the IT Support Ticket System through a web browser.
- Employees can submit support tickets using the ticket entry page.
- IT Support Team members can filter and manage tickets using the search received tickets page.
- View project overview on the project overview page.

## Contact
For any questions or issues related to this project, please contact:
- Email: rani_yaqoob@icloud.com

---

Thank you for using the ITSupportTicketSystem. We hope this system enhances the efficiency of IT support within Bell's Field Services.
