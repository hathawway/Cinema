using System.ComponentModel.DataAnnotations;

namespace CInema.Domain.Models.Common
{
    public abstract class Entity
    {
        [Key]
        public virtual long Id { get; set; }
    }
}
