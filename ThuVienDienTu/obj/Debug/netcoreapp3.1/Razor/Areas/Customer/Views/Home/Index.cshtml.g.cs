#pragma checksum "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4650a8028a312c4a9c01b25aee5fe15aadd2ab6b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Customer_Views_Home_Index), @"mvc.1.0.view", @"/Areas/Customer/Views/Home/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4650a8028a312c4a9c01b25aee5fe15aadd2ab6b", @"/Areas/Customer/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"53f062d23fc8da7544beb0cd768f7adbc76d977c", @"/Areas/Customer/Views/_ViewImports.cshtml")]
    public class Areas_Customer_Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ThuVienDienTu.Models.ViewModels.BooksViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("page-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("page-class", "btn border", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("page-class-normal", "btn btn-default active", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("page-class-selected", "btn btn-primary active", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn-group m-1"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("border-radius:unset !important"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        private global::ThuVienDienTu.TagHelpers.PageLinkTagHelper __ThuVienDienTu_TagHelpers_PageLinkTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"site-section bg-light\" id=\"products-section\">\r\n");
#nullable restore
#line 6 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
     if (Model.ApplicationUser != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        <div class=""container"">
            <div class=""fade"" id=""myModal"" role=""dialog"">
                <div class=""modal-dialog"">
                    <div class=""modal-content"">
                        <div class=""modal-header"">
                            <h5>Đăng ký nhanh tài khoản</h5>
                            <button type=""button"" class=""close float-right"" data-dismiss=""modal"">&times;</button>
                        </div>
                        <div class=""modal-body"">
                            <p>amen</p>
                            <input id=""confirmPolicy"" type=""checkbox"" onchange=""EnableRegister()"" /> <span>Tôi đã đọc và xác nhận tuân theo điều khoản</span>
                        </div>
                        <div class=""modal-footer"">
                            <button id=""registerButton"" type=""submit"" class=""btn btn-primary align-self-center"" disabled>Đăng ký</button>
                            <button type=""button"" class=""btn btn-danger"" data-dismiss=""modal"">Close</butt");
            WriteLiteral("on>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n");
#nullable restore
#line 28 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"container\">\r\n");
            WriteLiteral("        <div class=\"row\">\r\n");
#nullable restore
#line 36 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
             foreach (var book in Model.Books)
            {


#line default
#line hidden
#nullable disable
            WriteLiteral("                <div class=\"col-md-6 col-lg-4 mb-4\">\r\n                    <div class=\"service h-100\">\r\n                        <div class=\"card\">\r\n                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4650a8028a312c4a9c01b25aee5fe15aadd2ab6b8773", async() => {
                WriteLiteral("\r\n                                <div class=\"card-body\">\r\n                                    <img class=\"card-img\"");
                BeginWriteAttribute("src", " src=\"", 2034, "\"", 2055, 1);
#nullable restore
#line 44 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
WriteAttributeValue("", 2040, book.BookImage, 2040, 15, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" width=\"300\" height=\"300\" data-imagezoom=\"true\" />\r\n                                </div>\r\n                            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 42 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
                                                      WriteLiteral(book.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "title", 1, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
#nullable restore
#line 42 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
AddHtmlAttributeValue("", 1902, book.BookName, 1902, 14, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                            <div class=\"card-footer\">\r\n                                <button class=\"btn btn-outline-secondary\">");
#nullable restore
#line 48 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
                                                                     Write(book.BookPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</button>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n\r\n                </div>\r\n");
#nullable restore
#line 54 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"

            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </div>\r\n    </div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4650a8028a312c4a9c01b25aee5fe15aadd2ab6b12884", async() => {
            }
            );
            __ThuVienDienTu_TagHelpers_PageLinkTagHelper = CreateTagHelper<global::ThuVienDienTu.TagHelpers.PageLinkTagHelper>();
            __tagHelperExecutionContext.Add(__ThuVienDienTu_TagHelpers_PageLinkTagHelper);
#nullable restore
#line 58 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
__ThuVienDienTu_TagHelpers_PageLinkTagHelper.PageModel = Model.PagingInfo;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("page-model", __ThuVienDienTu_TagHelpers_PageLinkTagHelper.PageModel, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __ThuVienDienTu_TagHelpers_PageLinkTagHelper.PageAction = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
#nullable restore
#line 58 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
__ThuVienDienTu_TagHelpers_PageLinkTagHelper.PageClassesEnabled = true;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("page-classes-enabled", __ThuVienDienTu_TagHelpers_PageLinkTagHelper.PageClassesEnabled, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __ThuVienDienTu_TagHelpers_PageLinkTagHelper.PageClass = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __ThuVienDienTu_TagHelpers_PageLinkTagHelper.PageClassNormal = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __ThuVienDienTu_TagHelpers_PageLinkTagHelper.PageClassSelected = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 61 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
     if (Model.BooksSeen.Count > 0)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"site-section\">\r\n            <hr />\r\n            <h4>Sách bạn đã xem</h4>\r\n");
#nullable restore
#line 66 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
             if (Model.BooksSeen.Count > 0)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div id=\"bookseen\" class=\"carousel slide\" data-ride=\"carousel\">\r\n                    <div class=\"carousel-inner\">\r\n");
#nullable restore
#line 70 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
                          int index1 = 0;

#line default
#line hidden
#nullable disable
#nullable restore
#line 71 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
                         for (int j = 0; j < Model.BooksSeen.Count; j++)
                        {
                            if (Model.BooksSeen.Count > 5 && (j + 5) >= Model.BooksSeen.Count)
                            {
                                break;
                            }
                            index1++;
                            var active = index1 == 1 ? "active" : "";

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <div");
            BeginWriteAttribute("class", " class=\"", 3607, "\"", 3636, 2);
            WriteAttributeValue("", 3615, "carousel-item", 3615, 13, true);
#nullable restore
#line 79 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
WriteAttributeValue(" ", 3628, active, 3629, 7, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                <div class=\"row\">\r\n");
#nullable restore
#line 81 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
                                     for (int k = j; k < j + 5; k++)
                                    {
                                        if (Model.BooksSeen.Count > 5 && (j + 5) >= Model.BooksSeen.Count)
                                        {
                                            break;
                                        }
                                        if (k >= Model.BooksSeen.Count)
                                        {
                                            break;
                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <div class=\"col-md-2 col-sm-6 col-12\" style=\"margin-left:27px\">\r\n                                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4650a8028a312c4a9c01b25aee5fe15aadd2ab6b18723", async() => {
                WriteLiteral("\r\n                                                <img style=\"width:300px;height:400px\"");
                BeginWriteAttribute("src", " src=\"", 4626, "\"", 4661, 1);
#nullable restore
#line 93 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
WriteAttributeValue("", 4632, Model.BooksSeen[k].BookImage, 4632, 29, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("alt", " alt=\"", 4662, "\"", 4668, 0);
                EndWriteAttribute();
                WriteLiteral(">\r\n                                            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_8);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 92 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
                                                                                                        WriteLiteral(Model.BooksSeen[k].Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "title", 1, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
#nullable restore
#line 92 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
AddHtmlAttributeValue("", 4509, Model.BooksSeen[k].BookName, 4509, 28, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                                        </div>\r\n");
#nullable restore
#line 96 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                </div>\r\n                            </div>\r\n");
#nullable restore
#line 99 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                    </div>
                    <a class=""carousel-control-prev"" style=""width:5%;"" href=""#bookseen"" role=""button"" data-slide=""prev"">
                        <i class=""fas fa-chevron-circle-left"" style=""font-size:30px;color:black""></i>
                        <span class=""sr-only"">Previous</span>
                    </a>
                    <a class=""carousel-control-next"" style=""width:5%;"" href=""#bookseen"" role=""button"" data-slide=""next"">
                        <i class=""fas fa-chevron-circle-right"" style=""font-size:30px;color:black""></i>
                        <span class=""sr-only"">Next</span>
                    </a>
                </div>
");
#nullable restore
#line 110 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </div>\r\n");
#nullable restore
#line 112 "C:\Users\Thanh\Desktop\Thuviendientu\ThuVienDienTu\Areas\Customer\Views\Home\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ThuVienDienTu.Models.ViewModels.BooksViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
