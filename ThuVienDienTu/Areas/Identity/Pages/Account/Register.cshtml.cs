using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PayPal.Api;
using ThuVienDienTu.Models;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IHostingEnvironment _hostingEnvironment;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress(ErrorMessage = "Email phải có định dạng abc@xyz.de")]
            [Display(Name = "Địa chỉ Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} phải tối thiểu {2} và tối đa {1} kí tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu")]
            [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
            public string ConfirmPassword { get; set; }
            [Required]
            [Display(Name = "Tài khoản"), StringLength(15, ErrorMessage = "{0} tối thiểu {2} và tối đa {1} kí tự.", MinimumLength = 6)]
            public string Displayname { get; set; }
            [Display(Name = "Họ và tên")]
            public string Fullname { get; set; }
            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }
            [Display(Name = "Avatar")]
            public string UserAvatar { get; set; }
            public bool IsLibrarian { get; set; }
            public bool IsCensor { get; set; }
            public bool IsAdmin { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync(SD.ADMIN_ROLE))
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.ADMIN_ROLE));
                }
                if (!await _roleManager.RoleExistsAsync(SD.CENSOR_ROLE))
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.CENSOR_ROLE));
                }
                if (!await _roleManager.RoleExistsAsync(SD.LIBRARIAN_ROLE))
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.LIBRARIAN_ROLE));
                }
                if (!await _roleManager.RoleExistsAsync(SD.READER_ROLE))
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.READER_ROLE));
                }

                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, DisplayName = Input.Displayname, Date = DateTime.Now, Address = Input.Address };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (Input.IsAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, SD.ADMIN_ROLE);
                    }
                    else if (Input.IsCensor)
                    {
                        await _userManager.AddToRoleAsync(user, SD.CENSOR_ROLE);
                    }
                    else if (Input.IsLibrarian)
                    {
                        await _userManager.AddToRoleAsync(user, SD.LIBRARIAN_ROLE);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.READER_ROLE);
                    }
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {
                        var uploads = Path.Combine(webRootPath, SD.UserAvatar);
                        var extension = Path.GetExtension(files[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, user.Id + extension), FileMode.Create))
                        {
                            await files[0].CopyToAsync(fileStream);
                        }
                        user.UserAvatar = @"\" + SD.UserAvatar + @"\" + user.Id + extension;
                    }
                    else
                    {
                        var uploads = Path.Combine(webRootPath, SD.DefaultUserAvatar);
                        System.IO.File.Copy(uploads,webRootPath + @"\" + SD.UserAvatar + @"\" + user.Id + ".png");
                        user.UserAvatar = @"\" + SD.UserAvatar + @"\" + user.Id + ".png";
                    }
                    await _userManager.UpdateAsync(user);
                    _logger.LogInformation("User created a new account with password.");
                    if(User.IsInRole(SD.ADMIN_ROLE))
                    {
                        return RedirectToAction("Index", "AdminUser", new { area = "Admin" });
                    }    
                    return RedirectToPage("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
