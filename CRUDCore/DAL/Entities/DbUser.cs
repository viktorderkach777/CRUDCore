using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCore.DAL.Entities
{
    public class DbUser: IdentityUser<long>
    {
        [StringLength(255)]
        public string Image { get; set; }

        [Range(0, 130, ErrorMessage = "Недопустимый год")]
        public double Age { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public virtual ICollection<DbUserRole> UserRoles { get; set; }
    }
}
