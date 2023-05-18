using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace healthcareMIS.Pages.Patients
{
	[Authorize]
    public class PatientModel : PageModel
    {
		public PatientInfo patientInfo = new PatientInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
		{
		}

		public void OnPost()
		{
			patientInfo.name = Request.Form["fullname"];
			patientInfo.email = Request.Form["email"];
			patientInfo.phone = Request.Form["phone"];
			patientInfo.address = Request.Form["address"];
			patientInfo.createdAt = DateTime.Parse(Request.Form["createdAt"]);

			if (patientInfo.name.Length == 0 || patientInfo.email.Length == 0 || patientInfo.phone.Length == 0 || patientInfo.address.Length == 0 || patientInfo.createdAt == DateTime.MinValue)
			{
				errorMessage = "All fields are required MR/MRS";
				return;
			}

			try
			{
				String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=healthcareMIS;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "INSERT INTO patients " +
								 "(name, email, phone, address, createdAt) VALUES " +
								 "(@name, @email, @phone, @address, @createdAt);";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", patientInfo.name);
						command.Parameters.AddWithValue("@email", patientInfo.email);
						command.Parameters.AddWithValue("@phone", patientInfo.phone);
						command.Parameters.AddWithValue("@address", patientInfo.address);
						command.Parameters.AddWithValue("@createdAt", patientInfo.createdAt);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			patientInfo.name = "";
			patientInfo.email = "";
			patientInfo.phone = "";
			patientInfo.address = "";

			successMessage = "New Patient Successfully Added";

			Response.Redirect("/Patients/Patient");
		}
	}

	public class PatientInfo
	{
		public int id { get; set; }
		public String name { get; set; }
		public String email { get; set; }
		public String phone { get; set; }
		public String address { get; set; }
		public DateTime createdAt { get; set; }
	}
}

