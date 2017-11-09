using System;
using System.Collections.Generic;

namespace PermissionsTemplate.Models.DataModels
{
    public partial class RolePermissions
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public Permissions Permission { get; set; }
        public Roles Role { get; set; }
    }
}
