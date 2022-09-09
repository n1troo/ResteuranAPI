using Microsoft.EntityFrameworkCore;

namespace ResteuranAPI.Entities
{
    public class RestaurantDbContext :DbContext
    {
        private string _connectionString = "Server=IP-NB59\\SQLEXPRESS;Database=RestaurantDb;User Id=sa;Password=admin123;";
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Dish>()
                .Property(s => s.Name)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(s => s.Email)
                .IsRequired();
            
            modelBuilder.Entity<Role>()
                .Property(s => s.Name)
                .IsRequired();
            
            modelBuilder.Entity<Role>().HasData(
                new Role() {Id = 3, Name = "user" },
                new Role() {Id = 2, Name = "Manager" },
                new Role() {Id = 1, Name = "Admin" }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
