using healthcareMIS.Pages.Appointments;
using healthcareMIS.Pages.Doctor;
using healthcareMIS.Pages.Patients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static healthcareMIS.Pages.Medical_Record.MedicalHistoryModel;

namespace healthcareMIS.Pages.Medical_Record
{
    public class UpdateModel : PageModel
    {
		public Medical medicInfo = new Medical();
		public PatientInfo patientInfo = new PatientInfo();
		public DoctorInfo doctorInfo = new DoctorInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public List<PatientInfo> Patient { get; set; } = new List<PatientInfo>();
		public List<DoctorInfo> Doctor { get; set; } = new List<DoctorInfo>();
		public List<Medical> medics { get; set; } = new List<Medical>();
		public void OnGet()
        {
			String id = Request.Query["mh_id"];
			try
			{
				String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM medicalhistory WHERE mh_id=@mh_id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@mh_id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								medicInfo.mh_id = "" + reader.GetInt32(0);
								medicInfo.visit_date = reader.GetDateTime(1);
								medicInfo.diagnosis = reader.GetString(2);
								medicInfo.treatment = reader.GetString(3);
								medicInfo.medications = reader.GetString(4);
								medicInfo.notes = reader.GetString(5);
							}
						}
					}

					string query = "SELECT id, name AS full_name FROM patients";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								PatientInfo patientInfo = new PatientInfo();
								patientInfo.id = reader.GetInt32(0);
								patientInfo.name = reader.GetString(1);

								//Patients.Add(new SelectListItem { Value = patientId.ToString(), Text = fullName });
								Patient.Add(patientInfo);
							}
						}
					}

					String dquery = "SELECT doctor_id, CONCAT(firstname, ' ', lastname) AS full_name FROM doctor";

					using (SqlCommand command = new SqlCommand(dquery, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								DoctorInfo doctorInfo = new DoctorInfo();
								doctorInfo.doctor_id = reader.GetInt32(0);
								doctorInfo.firstname = reader.GetString(1);

								Doctor.Add(doctorInfo);
							}
						}
					}

				}
			}
			catch (Exception ex)
			{
				errorMessage = "Somethoigjk";
				return;
			}

		}

		public void OnPost()
		{

			medicInfo.mh_id = Request.Form["id"];
			medicInfo.patient_id = Request.Form["patientId"];
			medicInfo.doctor_id = Request.Form["doctorId"];
			medicInfo.visit_date = DateTime.Parse(Request.Form["visitday"]);
			medicInfo.diagnosis = Request.Form["diagnosis"];
			medicInfo.treatment = Request.Form["treatment"];
			medicInfo.medications = Request.Form["medications"];
			medicInfo.notes = Request.Form["notes"];

			try
			{
				String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE medicalhistory " +
								 "SET visit_date=@visit_date, diagnosis=@diagnosis, treatment=@treatment, " +
								 "medications=@medications, notes=@notes, patient_id=@patient_id, doctor_id=@doctor_id " +
								 "WHERE mh_id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@visit_date", medicInfo.visit_date);
						command.Parameters.AddWithValue("@diagnosis", medicInfo.diagnosis);
						command.Parameters.AddWithValue("@treatment", medicInfo.treatment);
						command.Parameters.AddWithValue("@medications", medicInfo.medications);
						command.Parameters.AddWithValue("@notes", medicInfo.notes);
						command.Parameters.AddWithValue("@patient_id", medicInfo.patient_id);
						command.Parameters.AddWithValue("@doctor_id", medicInfo.doctor_id);
						command.Parameters.AddWithValue("@id", medicInfo.mh_id);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}
			medicInfo.patient_id = "";
			medicInfo.doctor_id = "";
			medicInfo.diagnosis = "";
			medicInfo.treatment = "";
			medicInfo.medications = "";
			medicInfo.notes = "";

			successMessage = " Medical History Successfully Updated";
		} 

        public class Medical
        {
            public String mh_id { get; set; }
            public DateTime visit_date { get; set; }
            public String diagnosis { get; set; }
            public String treatment { get; set; }
            public String medications { get; set; }
            public String notes { get; set; }
            public String patient_id { get; set; }
            public String doctor_id { get; set; }
        }
    }
}
