public class Employee
{
    public int EmployeeID { get; set; }
    public string Name { get; set; }
    public int DepartmentID { get; set; }
    public virtual Department Department { get; set; }
}