using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AruCareApi.Models
{
    public class ApplicationRole: IdentityRole        
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public ApplicationRole() : base() { }
        public ApplicationRole(string RoleName) : base(RoleName) { }

        public ApplicationRole(ICollection<ApplicationUserRole> userRoles)
        {
            UserRoles = userRoles;
        }
    }
}
