using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_SwapSphere_models.Models
{
    public class Placeswap :IEntity
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public virtual ICollection<Entity_Swap>? Swaps { get; set; }
    }
}
