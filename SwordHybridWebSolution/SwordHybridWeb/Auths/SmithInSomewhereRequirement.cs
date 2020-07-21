using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwordHybridWeb.Auths
{
    public class SmithInSomewhereRequirement : IAuthorizationRequirement
    {
        public SmithInSomewhereRequirement()
        {

        }

        // 当然也支持有参数的构造函数
    }
}
