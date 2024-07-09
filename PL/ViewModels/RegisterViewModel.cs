using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "First Name is Required")]
		public string FName { get; set; }

		[Required(ErrorMessage = "Last Name is Required")]
		
		public string LName { get; set; }

		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Password is Required")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Password is Required")]
		[Compare(nameof(Password), ErrorMessage = "Password Doesn't Match")]
		public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }


    }
}
