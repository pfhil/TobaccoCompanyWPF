namespace TobaccoCompanyWPF.Models.Entity
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public DateTime DateBirth { get; set; } = new DateTime(2000, 1, 1);

        public List<Role> Roles { get; set; } = new();

        public List<Order> Orders { get; set; } = new();

    }
}
