using System.ComponentModel.DataAnnotations;

namespace UrlShorten.Models.Commons
{
    public class BaseEntity<TKey> : IEntityId<TKey>, IAuditableEntity
    {
        [Key]
        public TKey Id { get ; set ; }
        public DateTime CreatedAt { get ; set ; }
        public string CreatedBy { get ; set ; }
    }
}
