using System.ComponentModel.DataAnnotations;

namespace core.api.client
{
	public class RegisterModel
	{
		[Required]
		[Display(Name = "User name")]
		[RestSharp.Serializers.SerializeAs(Name = "Username")]
		public string UserName { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		[RestSharp.Serializers.SerializeAs(Name = "Password")]

		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		[Display(Name = "Email Address")]
		[RestSharp.Serializers.SerializeAs(Name = "Email")]
		public string Email { get; set; }

		public string RegistrationSource { get; set; }
	}
}