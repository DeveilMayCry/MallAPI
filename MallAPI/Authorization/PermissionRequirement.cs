using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Authorization
{
    public class PermissionRequirement :IAuthorizationRequirement
    {
        public string PermissionId { get; set; }

        public PermissionRequirement(string permissionId)
        {
            PermissionId = permissionId;
        }
    }
}
