#pragma checksum "D:\CodeDemos\Id4demo\102AuthCodeDemo\SwordHybridWebSolution\SwordHybridWeb\Views\Home\AccessApi1.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fe7089a59a1e8a7ea4d1843e4591abe243714b65"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_AccessApi1), @"mvc.1.0.view", @"/Views/Home/AccessApi1.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\CodeDemos\Id4demo\102AuthCodeDemo\SwordHybridWebSolution\SwordHybridWeb\Views\_ViewImports.cshtml"
using SwordHybridWeb;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\CodeDemos\Id4demo\102AuthCodeDemo\SwordHybridWebSolution\SwordHybridWeb\Views\_ViewImports.cshtml"
using SwordHybridWeb.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fe7089a59a1e8a7ea4d1843e4591abe243714b65", @"/Views/Home/AccessApi1.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c0b184d68bca39f6631a4320f51192f03d1ba018", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_AccessApi1 : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<string>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\CodeDemos\Id4demo\102AuthCodeDemo\SwordHybridWebSolution\SwordHybridWeb\Views\Home\AccessApi1.cshtml"
   
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"text-center\">\r\n    <h1 class=\"display-4\">sword Api resource respose: </h1>\r\n    <p>");
#nullable restore
#line 9 "D:\CodeDemos\Id4demo\102AuthCodeDemo\SwordHybridWebSolution\SwordHybridWeb\Views\Home\AccessApi1.cshtml"
  Write(Model);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<string> Html { get; private set; }
    }
}
#pragma warning restore 1591
