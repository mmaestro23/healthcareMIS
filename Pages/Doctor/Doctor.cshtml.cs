using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace healthcareMIS.Pages.Doctor
{
    public class DoctorModel : PageModel
    {
		public DoctorInfo doctorInfo = new DoctorInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
		{
		}

		public void OnPost()
		{
			doctorInfo.firstname = Request.Form["firstname"];
			doctorInfo.lastname = Request.Form["lastname"];
			doctorInfo.specialization = Request.Form["specialization"];
			doctorInfo.email = Request.Form["email"];
			doctorInfo.phone = Request.Form["phone"];
			doctorInfo.address = Request.Form["address"];

			if (doctorInfo.firstname.Length == 0 || doctorInfo.lastname.Length == 0 || doctorInfo.specialization.Length == 0 || doctorInfo.phone.Length == 0 || doctorInfo.address.Length == 0)
			{
				errorMessage = "All fields are required MR/MRS";
				return;
			}

			try
			{
				String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";


				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "INSERT INTO doctor " +
								 "(firstname, lastname, specialization, email, phone, address) VALUES " +
								 "(@firstname, @lastname,@specialization, @email, @phone, @address);";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@firstname", doctorInfo.firstname);
						command.Parameters.AddWithValue("@lastname", doctorInfo.lastname);
						command.Parameters.AddWithValue("@specialization", doctorInfo.specialization);
						command.Parameters.AddWithValue("@email", doctorInfo.email);
						command.Parameters.AddWithValue("@phone", doctorInfo.phone);
						command.Parameters.AddWithValue("@address", doctorInfo.address);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			doctorInfo.firstname = "";
			doctorInfo.lastname = "";
			doctorInfo.specialization = "";
			doctorInfo.email = "";
			doctorInfo.phone = "";
			doctorInfo.address = "";

			successMessage = "New Doctor Successfully Added";

			Response.Redirect("/Doctor/Doctor");
		}
	}

	public class DoctorInfo
	{
		public int doctor_id { get; set; }
		public String firstname { get; set; }
		public String lastname { get; set; }
		public String specialization { get; set; }
		public String email { get; set; }
		public String phone { get; set; }
		public String address { get; set; }
	}
}
