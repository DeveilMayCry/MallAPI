using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Enum
{
    public enum PermissionNameEnum
    {
        ProductQuery = 1,
        ProductModify = 2,
        ProductAdd = 4,
        CategoryQuery = 5,
        CategoryModify = 6,
        CategoryAdd = 7,
        ProductCartQuery = 8,
        ProductCartAdd = 9,
    }
}
