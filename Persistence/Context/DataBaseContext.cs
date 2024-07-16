using Application.Interfaces.Context;
using Domain.Attributes;
using Domain.Files;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DbSet<CsvFiles> csvFiles { get; set; }
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetCustomAttributes(typeof(AuditableAttribute), true).Length > 0)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("InsertTime").HasDefaultValue(DateTime.Now); ;
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("UpdateTime");
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("RemoveTime");
                    modelBuilder.Entity(entityType.Name).Property<bool>("IsRemoved").HasDefaultValue(false);
                }
            }
            modelBuilder.Entity<CsvFiles>().HasKey(u => u.Id);
            modelBuilder.Entity<CsvFiles>().Property(e=>e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<CsvFiles>().HasQueryFilter(m => EF.Property<bool>(m, "IsRemoved") == false).HasIndex(u => u.Code).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges()
        {
            //لیست موجودیت ها  برای  ویرایش
            var modifiedEntries = ChangeTracker.Entries()
               .Where(p => p.State == EntityState.Modified ||
               p.State == EntityState.Added ||
               p.State == EntityState.Deleted
               );
            foreach (var item in modifiedEntries)
            {
                var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
                if (entityType != null)
                {
                    var inserted = entityType.FindProperty("InsertTime");
                    var updated = entityType.FindProperty("UpdateTime");
                    var RemoveTime = entityType.FindProperty("RemoveTime");
                    var IsRemoved = entityType.FindProperty("IsRemoved");
                    if (item.State == EntityState.Added && inserted != null)
                    {
                        item.Property("InsertTime").CurrentValue = DateTime.Now;
                    }
                    if (item.State == EntityState.Modified && updated != null)
                    {
                        item.Property("UpdateTime").CurrentValue = DateTime.Now;
                    }

                    if (item.State == EntityState.Deleted && RemoveTime != null && IsRemoved != null)
                    {
                        item.Property("RemoveTime").CurrentValue = DateTime.Now;
                        item.Property("IsRemoved").CurrentValue = true;
                        item.State = EntityState.Modified;
                    }
                }

            }
            return base.SaveChanges();
        }

    }
}
