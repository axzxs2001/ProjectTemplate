using System;
using System.Collections.Generic;

namespace PermissionsTemplate.Models.DataModels
{
    public partial class Roles
    {
        public Roles()
        {
            RolePermissions = new HashSet<RolePermissions>();
            UserRoles = new HashSet<UserRoles>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }

        public ICollection<RolePermissions> RolePermissions { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
