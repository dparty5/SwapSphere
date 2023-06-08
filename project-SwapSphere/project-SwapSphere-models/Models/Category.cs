using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace project_SwapSphere_models.Models
{
    public class Category:IEntity
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [JsonIgnore] public virtual ICollection<Swap>? Swaps { get; set; }
    }
}
