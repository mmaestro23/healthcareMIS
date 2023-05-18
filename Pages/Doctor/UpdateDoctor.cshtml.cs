using healthcareMIS.Pages.Appointments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static healthcareMIS.Pages.Medical_Record.MedicalHistoryModel;

namespace healthcareMIS.Pages.Doctor
{
    public class UpdateDoctorModel : PageModel
    {
		public Doctor doctorInfo = new Doctor();
		public List<Doctor> doctors { get; set; } = new List<Doctor>();

		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
        {
			String id = Request.Query["doctor_id"];
			String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM doctor WHERE doctor_id=@doctor_id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@doctor_id", id);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
                            doctorInfo.doctor_id = "" + reader.GetInt32(0);
                            doctorInfo.firstname = reader.GetString(1);
                            doctorInfo.lastname = reader.GetString(2);
                            doctorInfo.specialization = reader.GetString(3);
                            doctorInfo.email = reader.GetString(4);
                            doctorInfo.phone = reader.GetString(5);
                            doctorInfo.address = reader.GetString(6);
						}
					}
				}
			}
		}

		public void OnPost()
		{
			doctorInfo.doctor_id = Request.Form["id"];
            doctorInfo.firstname = Request.Form["firstname"];
            doctorInfo.lastname = Request.Form["lastname"];
            doctorInfo.specialization = Request.Form["specialization"];
            doctorInfo.email = Request.Form["email"];
            doctorInfo.phone = Request.Form["phone"];
            doctorInfo.address = Request.Form["address"];

			try
			{
				String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE doctor " +
								 "SET firstname=@firstname, lastname=@lastname, specialization=@specialization, " +
								 "email=@email, phone=@phone, address=@address " +
								 "WHERE doctor_id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@firstname",doctorInfo.firstname);
						command.Parameters.AddWithValue("@lastname", doctorInfo.lastname);
						command.Parameters.AddWithValue("@specialization", doctorInfo.specialization);
						command.Parameters.AddWithValue("@email", doctorInfo.email);
						command.Parameters.AddWithValue("@phone", doctorInfo.phone);
						command.Parameters.AddWithValue("@address", doctorInfo.address);
						command.Parameters.AddWithValue("@id", doctorInfo.doctor_id);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}
			doctorInfo.doctor_id = "";
			doctorInfo.firstname = "";
			doctorInfo.lastname = "";
			doctorInfo.specialization = "";
			doctorInfo.email = "";
			doctorInfo.phone = "";
			doctorInfo.address = "";

			successMessage = " Doctor Successfully Updated";

		}

		public class Doctor
		{
			public String doctor_id { get; set; }
			public String firstname { get; set; }
			public String lastname { get; set; }
			public String specialization { get; set; }
			public String email { get; set; }
			public String phone { get; set; }
			public String address { get; set; }
		}
	}
}
