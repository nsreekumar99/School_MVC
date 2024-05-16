// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using School.DataAccess.Data;
using School.Models.Models;
using School.Utility;

namespace SchoolManagement.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;

        public RegisterModel(
            ApplicationDbContext context,
            IUserStore<ApplicationUser> userStore,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailStore = GetEmailStore(userStore);
            _userStore = userStore;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Role { get; set; }

            public List<SelectListItem> RoleList { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null && await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                {
                    Input = new InputModel
                    {
                        RoleList = _roleManager.Roles.Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Name
                        }).ToList()
                    };

                    return Page();
                }
                else
                {
                    return LocalRedirect(returnUrl);
                }
            }

            Input = new InputModel
            {
                RoleList = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }).ToList()
            };

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var userDataString = TempData["UserData"] as string;
            if (!string.IsNullOrEmpty(userDataString))
            {
                Input = JsonConvert.DeserializeObject<InputModel>(userDataString);
            }
            else
            {
                Input = new InputModel();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                if (!string.IsNullOrEmpty(Input.Role))
                {
                    //await _userManager.AddToRoleAsync(user, Input.Role);
                    TempData["UserRole"] = Input.Role;  // Add role information to TempData
                }
                else
                {
                    //await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    //TempData["UserRole"] = SD.Role_Customer;  // Add default role information to TempData
                    TempData["UserRole"] = "Student";
                }

                TempData.Keep("UserRole");

                TempData["UserData"] = JsonConvert.SerializeObject(Input); //store user input to temp data
                TempData.Keep("UserData");

                // Store InputModel directly in TempDataInput

                _logger.LogInformation($"TempData[\"UserData\"] content: {TempData["UserData"]}");

                // Redirect to the next step
                return RedirectToPage("./NextStep");
            }

            return Page();
        }

        public ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore(IUserStore<ApplicationUser> userStore)
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            var userEmailStore = userStore as IUserEmailStore<ApplicationUser>;

            if (userEmailStore == null)
            {
                throw new InvalidOperationException("User store is not of the expected type.");
            }

            return userEmailStore;
        }
    }

}
