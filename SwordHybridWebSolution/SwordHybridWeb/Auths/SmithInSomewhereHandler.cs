using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwordHybridWeb.Auths
{
    public class SmithInSomewhereHandler : AuthorizationHandler<SmithInSomewhereRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SmithInSomewhereRequirement requirement)
        {
            //var filterContext = context.Resource as AuthorizationFilterContext;
            //if(filterContext == null)
            //{
            //    context.Fail();
            //    return Task.CompletedTask;
            //}

            var familyName = context.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.FamilyName)?.Value;
            var location = context.User.Claims.FirstOrDefault(c => c.Type == "location")?.Value;

            if(familyName == "f" && location == "somewhere" && context.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            context.Fail();
            return Task.CompletedTask;

            // 一个Handler 成功，其他的Handler没有失败 =》 Requirement被满足了
            // 某个Handler 失败 =》 无法满足Requirement
            // 没有成功和失败 =》 无法满足Requirement
        }
    }
}
