using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Authorization
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        private string _id;

        public string PermissionId 
        {
            get { return _id; }

            set { Policy = value; }
        }

        public PermissionAuthorizeAttribute(string permissionId)
        {
            PermissionId = permissionId;
        }

    }
}
