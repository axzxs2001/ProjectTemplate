using System;
using System.Collections.Generic;

namespace PermissionsTemplate.Models.DataModels
{
    public partial class Permissions
    {
        public Permissions()
        {
            RolePermissions = new HashSet<RolePermissions>();
        }

        public int Id { get; set; }
        public string PermissionName { get; set; }
        public string Action { get; set; }
        public string Method { get; set; }
        public int? Pid { get; set; }
        public string Memo { get; set; }

        public ICollection<RolePermissions> RolePermissions { get; set; }
    }
}
