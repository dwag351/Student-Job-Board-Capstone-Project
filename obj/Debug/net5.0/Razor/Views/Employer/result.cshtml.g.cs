#pragma checksum "D:\399\project\Views\Employer\result.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0b3f9681ef758591e4034587b979097f574b9853"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Employer_result), @"mvc.1.0.view", @"/Views/Employer/result.cshtml")]
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
#line 1 "D:\399\project\Views\_ViewImports.cshtml"
using project;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\399\project\Views\_ViewImports.cshtml"
using project.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\399\project\Views\_ViewImports.cshtml"
using project.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\399\project\Views\_ViewImports.cshtml"
using project.Infrastructure;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\399\project\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\399\project\Views\_ViewImports.cshtml"
using System.Collections;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\399\project\Views\_ViewImports.cshtml"
using System.Collections.Specialized;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\399\project\Views\_ViewImports.cshtml"
using System.Web;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0b3f9681ef758591e4034587b979097f574b9853", @"/Views/Employer/result.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d8a99a8daff12cb375cafa2e00f2aa79185d3042", @"/Views/_ViewImports.cshtml")]
    public class Views_Employer_result : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<SurveyViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 5 "D:\399\project\Views\Employer\result.cshtml"
  
    ViewData["Title"] = "Result";
    Layout = "~/Views/Shared/_EmployerLayout.cshtml";


#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"jumbotron\">\r\n    <h1>");
#nullable restore
#line 11 "D:\399\project\Views\Employer\result.cshtml"
   Write(ViewBag.msgTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n    <p class=\"lead\">");
#nullable restore
#line 12 "D:\399\project\Views\Employer\result.cshtml"
               Write(ViewBag.info);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n</div>\r\n<a href=\"/employer\"><< Post a new job ad</a>\r\n\r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Microsoft.AspNetCore.Http.IHttpContextAccessor HttpContextAccessor { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<SurveyViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
