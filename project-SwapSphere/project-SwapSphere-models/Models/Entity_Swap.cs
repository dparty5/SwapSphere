using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_SwapSphere_models.Models
{
    public class Entity_Swap:IEntity
    {
        public Guid Id { get; set; }
        [Required]
        public virtual Swap Swap {get; set;}
        public string? Key { get; set;}
        [Required]
        public virtual Placeswap Placeswap { get; set;}
    }
}
//Placeswap