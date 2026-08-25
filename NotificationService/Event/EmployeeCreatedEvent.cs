namespace NotificationService.Event
{
    public class EmployeeCreatedEvent
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public string Address { get; set; } = string.Empty;
    }
}