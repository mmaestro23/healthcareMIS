using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace healthcareMIS.Pages.Register
{
    public class RegisterModel : PageModel
    {
		public RegisterInfo registerInfo = new RegisterInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
		{
		}

		public void OnPost()
		{
			registerInfo.firstname = Request.Form["firstname"];
			registerInfo.lastname = Request.Form["lastname"];
			registerInfo.username = Request.Form["username"];
			registerInfo.email = Request.Form["email"];
			registerInfo.password = Request.Form["password"];

			if (registerInfo.firstname.Length == 0 || registerInfo.lastname.Length == 0 || registerInfo.username.Length == 0 || registerInfo.password.Length == 0)
			{
				errorMessage = "All fields are required MR/MRS";
				return;
			}

			try
			{
				String connectionString = "Data Source=DESKTOP-26CN3O2\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "INSERT INTO users " +
								 "(firstname, lastname, username, email, password) VALUES " +
								 "(@firstname, @lastname,@username, @email, @password);";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@firstname", registerInfo.firstname);
						command.Parameters.AddWithValue("@lastname", registerInfo.lastname);
						command.Parameters.AddWithValue("username", registerInfo.username);
						command.Parameters.AddWithValue("@email", registerInfo.email);
						command.Parameters.AddWithValue("@password", registerInfo.password);
		

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			registerInfo.firstname = "";
			registerInfo.lastname = "";
			registerInfo.username = "";
			registerInfo.email = "";
			registerInfo.password = "";

			successMessage = "New Register Successfully Added";

			Response.Redirect("/Register/Register");
		}
	}

	public class RegisterInfo
	{
		public int user_id { get; set; }
		public String firstname { get; set; }
		public String lastname { get; set; }
		public String email { get; set; }
		public String username { get; set; }
		public String password { get; set; }
	}
}
