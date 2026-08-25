namespace EmployeeService.Event
{
    public class EmployeeCreatedEvent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Salary { get; set; }

        public string Address { get; set; }
        
        public string Email { get; set; }
    }

}