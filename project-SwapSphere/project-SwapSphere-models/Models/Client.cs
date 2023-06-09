using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace project_SwapSphere_models.Models
{
    public class Client:IEntity
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [JsonIgnore]
        [Required]
        public string Password { get; set; }
        [Required]
        [StringLength(50)]
        public string Login { get; set; }
        public string Photo { get; set; }
        public int Rating { get; set; } // значения от 1 до 10
        public Role Role { get; set; }
        
        [JsonIgnore] public virtual ICollection<Swap>? Swaps { get; set; }
    }
}
