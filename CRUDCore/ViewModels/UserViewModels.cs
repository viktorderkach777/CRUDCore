using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCore.ViewModels
{
    public class UserItemViewModel
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string Image { get; set; }

        public double Age { get; set; }

        public string Phone { get; set; }
      
        public string Description { get; set; }

        public IEnumerable<RoleItemViewModel> Roles { get; set; }

    }
    public class RoleItemViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
