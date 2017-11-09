using System;
using System.Collections.Generic;

namespace PermissionsTemplate.Models.DataModels
{
    public partial class Users
    {
        public Users()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
