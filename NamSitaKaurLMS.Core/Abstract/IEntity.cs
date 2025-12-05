using NamSitaKaurLMS.Core.Abstract;
using System.ComponentModel.DataAnnotations;


namespace NamSitaKaurLMS.Core.Abstract
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
