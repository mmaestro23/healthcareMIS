using System.ComponentModel.DataAnnotations;

namespace healthcareMIS.ViewModels
{
	public class RegisterInfo
	{
		[Required]
		[DataType(DataType.Text)]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Passwords aren't matching brother!")]
		public string ConfirmPassword { get; set; }
	}
}
