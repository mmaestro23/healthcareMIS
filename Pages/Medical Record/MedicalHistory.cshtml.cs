using healthcareMIS.Pages.Doctor;
using healthcareMIS.Pages.Patients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace healthcareMIS.Pages.Medical_Record
{
    public class MedicalHistoryModel : PageModel
    {
		public MedicInfo medicInfo = new MedicInfo();
		public PatientInfo patientInfo = new PatientInfo();
		public DoctorInfo doctorInfo = new DoctorInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public List<PatientInfo> Patients { get; set; } = new List<PatientInfo>();
		public List<DoctorInfo> Doctors { get; set; } = new List<DoctorInfo>();
		public void OnGet()
        {
			try
			{
				String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
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
								Patients.Add(patientInfo);
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

								Doctors.Add(doctorInfo);
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		public void OnPost()
		{
			string selectedPatientId = Request.Form["patientId"];
			if (int.TryParse(selectedPatientId, out int patientId))
			{
				medicInfo.patient_id = patientId;
			}
			string selectedDoctorId = Request.Form["doctorId"];
			if (int.TryParse(selectedDoctorId, out int doctorId))
			{
				medicInfo.doctor_id = doctorId;
			}
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
					String sql = "INSERT INTO medicalhistory " +
								"(visit_date, diagnosis, treatment, medications, notes, patient_id, doctor_id) VALUES " +
								"(@visit_date, @diagnosis, @treatment, @medications, @notes, @patient_id, @doctor_id)";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@visit_date", medicInfo.visit_date);
						command.Parameters.AddWithValue("@diagnosis", medicInfo.diagnosis);
						command.Parameters.AddWithValue("@treatment", medicInfo.treatment);
						command.Parameters.AddWithValue("@medications", medicInfo.medications);  // Corrected property name
						command.Parameters.AddWithValue("@notes", medicInfo.notes);
						command.Parameters.AddWithValue("@patient_id", medicInfo.patient_id);
						command.Parameters.AddWithValue("@doctor_id", medicInfo.doctor_id);

						command.ExecuteNonQuery();
					}

				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			medicInfo.diagnosis = "";
			medicInfo.treatment = "";
			medicInfo.medications = "";
			medicInfo.notes = "";

			successMessage = "New Medical History Successfully Added";

			Response.Redirect("/Doctor/Doctor");

		}

		


		public class MedicInfo
		{
			public int mh_id { get; set; }
			public DateTime visit_date { get; set; }
			public String diagnosis { get; set; }
			public String treatment { get; set; }
			public String medications { get; set; }
			public String notes { get; set; }
			public int patient_id { get; set; }
			public int doctor_id { get; set; }
		}
	}
}
