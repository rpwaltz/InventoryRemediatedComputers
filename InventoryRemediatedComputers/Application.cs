namespace InventoryRemediatedComputers
    {
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Application")]
    public partial class Application
        {
        [Required]
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string RegistryKey { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Version { get; set; }

        [StringLength(250)]
        public string Path { get; set; }

        [StringLength(5)]
        public string Bitness { get; set; }

        [Required]
        [StringLength(50)]
        public string MachineName { get; set; }

        public virtual Machine Machine { get; set; }
        }
    }
