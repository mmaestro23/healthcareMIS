using healthcareMIS.Pages.Doctor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static healthcareMIS.Pages.Patients.ViewPatientsModel;

namespace healthcareMIS.Pages.Patients
{
    public class UpdatePatientModel : PageModel
    {
		public Patient patientInfo = new Patient();
		public List<Patient> patients { get; set; } = new List<Patient>();

		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
        {
			String id = Request.Query["id"];
			String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM patients WHERE id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id", id);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							patientInfo.id = "" + reader.GetInt32(0);
							patientInfo.name = reader.GetString(1);
							patientInfo.email = reader.GetString(2);
							patientInfo.phone = reader.GetString(3);
							patientInfo.address = reader.GetString(4);
							patientInfo.createdAt = reader.GetDateTime(5);
						}
					}
				}
			}
		}

		public void OnPost()
		{
			patientInfo.id = Request.Form["id"];
			patientInfo.name = Request.Form["fullname"];
			patientInfo.email = Request.Form["email"];
			patientInfo.phone = Request.Form["phone"];
			patientInfo.address = Request.Form["address"];
			patientInfo.createdAt = DateTime.Parse(Request.Form["createdAt"]);

			try
			{
				String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE patients " +
								 "SET name=@name, " +
								 "email=@email, phone=@phone, address=@address, createdAt=@createdAt " +
								 "WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", patientInfo.name);
						command.Parameters.AddWithValue("@email", patientInfo.email);
						command.Parameters.AddWithValue("@phone", patientInfo.phone);
						command.Parameters.AddWithValue("@address", patientInfo.address);
						command.Parameters.AddWithValue("@createdAt", patientInfo.createdAt);
						command.Parameters.AddWithValue("@id", patientInfo.id);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}
			patientInfo.id = "";
			patientInfo.name = "";
			patientInfo.email = "";
			patientInfo.phone = "";
			patientInfo.address = "";

			successMessage = " patirnt Successfully Updated";
		}
    }
}
