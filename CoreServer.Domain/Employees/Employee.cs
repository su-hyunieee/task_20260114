namespace CoreServer.Domain.Employees{
    public class Employee
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Tel { get; private set; } = default!;
        public DateOnly Joined { get; private set; }

        private Employee() { }
        public Employee(string name, string email, string tel, DateOnly joined)
        {
            Name = name.Trim();
            Email = email.Trim();
            Tel = tel.Trim();
            Joined = joined;
        }
    }
}
