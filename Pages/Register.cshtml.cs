using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using healthcareMIS.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace healthcareMIS.Pages
{
    public class RegisterModel : PageModel
    {
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		[BindProperty]
		public RegisterInfo registerInfo { get; set; }
		public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
           

        public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostAsync()
		{
			if(ModelState.IsValid)
			{
				var user = new IdentityUser()
				{
					UserName = registerInfo.Username,
					Email = registerInfo.Email
				};

				var result = await userManager.CreateAsync(user, registerInfo.Password);
				if(result.Succeeded)
				{
					await signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToPage("index");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return Page();
		}
	}
}
