using healthcareMIS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace healthcareMIS.Pages
{
    public class LoginModel : PageModel
    {
		private readonly SignInManager<IdentityUser> signInManager;

		[BindProperty]
        public LoginInfo loginInfo { get; set; }

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
			this.signInManager = signInManager;
		}

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(loginInfo.Email, loginInfo.Password, loginInfo.RememberMe, false);
                if (identityResult.Succeeded)
                {
                    if(returnUrl == null || returnUrl == "/")
                    {
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        return RedirectToPage(returnUrl);
                    }
                }
				ModelState.AddModelError("", "Username AND/OR password incorrect");
			}
			return Page();
		}
    }
}
