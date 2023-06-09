using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace project_SwapSphere_models.Models
{
    public class Swap : IEntity
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime Swap_date { get; set; }
        [JsonIgnore] public virtual Category? Category { get; set; }

        [JsonIgnore] public virtual Client? Clients { get; set; }
    }
}