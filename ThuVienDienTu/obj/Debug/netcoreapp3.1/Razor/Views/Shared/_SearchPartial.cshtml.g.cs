#pragma checksum "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Views\Shared\_SearchPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "abe3bf2393fc17218979c171692630c310f455af"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__SearchPartial), @"mvc.1.0.view", @"/Views/Shared/_SearchPartial.cshtml")]
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
#line 1 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Views\_ViewImports.cshtml"
using ThuVienDienTu;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Views\_ViewImports.cshtml"
using ThuVienDienTu.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Views\Shared\_SearchPartial.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"abe3bf2393fc17218979c171692630c310f455af", @"/Views/Shared/_SearchPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"25d44ee45ed2776d63e3fe68d51e739060dc050c", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__SearchPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<ul class=\"navbar-nav\">\r\n    <li class=\"nav-item\">\r\n");
#nullable restore
#line 6 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Views\Shared\_SearchPartial.cshtml"
         using (Html.BeginForm("Index", "Home", FormMethod.Get))
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"row\" style=\"margin-top: 5px; margin-left: 10px;\">\r\n                <div class=\"col-xs-6\" style=\"width: 400px\">\r\n                    ");
#nullable restore
#line 10 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Views\Shared\_SearchPartial.cshtml"
               Write(Html.Editor("timkiem", new { htmlAttributes = new { @class = "form-control", autocomplete = "off", placeholder = "Tìm tên tác phẩm, tác giả, nhà xuất bản bạn mong muốn...", style = "border-radis:10px;font-size:14px" } }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n                <div class=\"col-xs-6\">\r\n                    <button id=\"searchbutton\" type=\"submit\" class=\"btn btn-secondary form-control\"");
            BeginWriteAttribute("value", " value=\"", 806, "\"", 814, 0);
            EndWriteAttribute();
            WriteLiteral(" style=\"width: 105px;text-align:left;font-size:14px\" onclick=\"myFunction()\">\r\n                        <i class=\"fas fa-search\"></i> Tìm kiếm\r\n                    </button>\r\n                </div>\r\n            </div>\r\n");
#nullable restore
#line 18 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Views\Shared\_SearchPartial.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </li>\r\n</ul>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public UserManager<IdentityUser> UserManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public SignInManager<IdentityUser> SignInManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
