using System;
using System.Collections.Generic;

namespace PermissionsTemplate.Models.DataModels
{
    public partial class Users
    {
      

        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string RoleName { get; set; }
    
    }
}
