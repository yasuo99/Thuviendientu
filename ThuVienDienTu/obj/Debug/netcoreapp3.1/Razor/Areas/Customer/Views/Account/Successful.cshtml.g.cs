#pragma checksum "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Account\Successful.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7a75ed658e97b1589cbacb6553b6dfacc2887bf2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Customer_Views_Account_Successful), @"mvc.1.0.view", @"/Areas/Customer/Views/Account/Successful.cshtml")]
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
#line 1 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\_ViewImports.cshtml"
using ThuVienDienTu;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\_ViewImports.cshtml"
using ThuVienDienTu.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7a75ed658e97b1589cbacb6553b6dfacc2887bf2", @"/Areas/Customer/Views/Account/Successful.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"53f062d23fc8da7544beb0cd768f7adbc76d977c", @"/Areas/Customer/Views/_ViewImports.cshtml")]
    public class Areas_Customer_Views_Account_Successful : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ThuVienDienTu.Models.ApplicationUser>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7a75ed658e97b1589cbacb6553b6dfacc2887bf23220", async() => {
                WriteLiteral(@"
    <br />
    <br />
    <br />
    <br />
    <h1 class=""text-info"" style=""text-align:center;color:grey;margin-top:40px;font-family:Bahnschrift;font-size:30px"">Đăng ký nhanh</h1>
    <div class=""site-section bg-light"" style=""text-align:center !important"">
        <h5>
                Đăng ký nhanh tài khoản thành công
        </h5>
        <p>
                Tài khoản: ");
#nullable restore
#line 13 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Account\Successful.cshtml"
                      Write(Model.Email);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n        </p>\r\n        <p>\r\n                Mật khẩu: ");
#nullable restore
#line 16 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Account\Successful.cshtml"
                     Write(Model.Address);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n        </p>\r\n    </div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ThuVienDienTu.Models.ApplicationUser> Html { get; private set; }
    }
}
#pragma warning restore 1591
