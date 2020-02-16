using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Authorization
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionAuthorizeAttribute()
        {
        }

        public PermissionAuthorizeAttribute(string permissionId)
        {
            Policy = permissionId;
        }

    }
}
