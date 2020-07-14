namespace InventoryRemediatedComputers
    {
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MachineInventory : DbContext
        {
        public MachineInventory()
            : base("name=MachineInventory")
            {
            }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Machine> Machines { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
            modelBuilder.Entity<Machine>()
                .HasMany(e => e.Applications)
                .WithRequired(e => e.Machine)
                .WillCascadeOnDelete(false);
            }
        }
    }
