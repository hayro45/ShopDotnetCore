#pragma checksum "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "643d767ab63c4a9735f6dc5050282b3b40a74dc7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shop_List), @"mvc.1.0.view", @"/Views/Shop/List.cshtml")]
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
#line 2 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.entity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.WebUI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.WebUI.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.WebUI.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"643d767ab63c4a9735f6dc5050282b3b40a74dc7", @"/Views/Shop/List.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1fea5bbd98565548c8a28aaa63a0a09172a67336", @"/Views/_ViewImports.cshtml")]
    public class Views_Shop_List : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ProductListViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"col-md-3\">\r\n        ");
#nullable restore
#line 5 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
   Write(await Component.InvokeAsync("Categories"));

#line default
#line hidden
#nullable disable
            WriteLiteral("      \r\n    </div>\r\n    <div class=\"col-md-9\">\r\n        <div class=\"row\">\r\n");
#nullable restore
#line 9 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
             foreach (var product in Model.Products)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div class=\"col-md-4\">\r\n                    ");
#nullable restore
#line 12 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
               Write(await Html.PartialAsync("_product", product));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n");
#nullable restore
#line 14 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </div>\r\n        <div class=\"row\">\r\n            <nav aria-label=\"...\">\r\n                <ul class=\"pagination\">\r\n                    \r\n");
#nullable restore
#line 20 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                     for (int i = 0; i < Model.PageInfo.TotalPages(); i++)
                    {   
                        

#line default
#line hidden
#nullable disable
#nullable restore
#line 22 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                         if (String.IsNullOrEmpty(Model.PageInfo.CurrentCategory))
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li");
            BeginWriteAttribute("class", " class=\"", 804, "\"", 868, 2);
            WriteAttributeValue("", 812, "page-item", 812, 9, true);
#nullable restore
#line 24 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue(" ", 821, Model.PageInfo.CurrentPage==i+1?"active":"", 822, 46, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                <a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 924, "\"", 952, 2);
            WriteAttributeValue("", 931, "/products?page=", 931, 15, true);
#nullable restore
#line 25 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue("", 946, i+1, 946, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 25 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                                                                              Write(i+1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n                            </li>\r\n");
#nullable restore
#line 27 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                        }else
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li");
            BeginWriteAttribute("class", " class=\"", 1090, "\"", 1154, 2);
            WriteAttributeValue("", 1098, "page-item", 1098, 9, true);
#nullable restore
#line 29 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue(" ", 1107, Model.PageInfo.CurrentPage==i+1?"active":"", 1108, 46, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                <a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 1210, "\"", 1270, 4);
            WriteAttributeValue("", 1217, "/products/", 1217, 10, true);
#nullable restore
#line 30 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue("", 1227, Model.PageInfo.CurrentCategory, 1227, 31, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1258, "?page=", 1258, 6, true);
#nullable restore
#line 30 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue("", 1264, i+1, 1264, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 30 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                                                                                                              Write(i+1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n                            </li>\r\n");
#nullable restore
#line 32 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 32 "C:\Users\hayre\source\repos\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                         
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    \r\n\r\n                </ul>\r\n            </nav>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n\r\n\r\n\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n<script src=\"https://cdn.jsdelivr.net/npm/popper.js@1.12.9/dist/umd/popper.min.js\"></script>\r\n<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js\"></script>\r\n");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ProductListViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
