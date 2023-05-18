using System.Data.SqlClient;
using healthcareMIS.Pages.Doctor;
using healthcareMIS.Pages.Patients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static healthcareMIS.Pages.Medical_Record.MedicalHistoryModel;

namespace healthcareMIS.Pages.Appointments
{
    public class AppointmentsModel : PageModel
    {
		private const string V = "";
		public AppointmentInfo appointmentInfo = new AppointmentInfo();
		public List<Patients> patient = new List<Patients>();
		public List<Doctor> doctor = new List<Doctor>();
		public String errorMessage = "";
		public String successMessage = "";
		public IActionResult OnGet()
		{
			String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();
				string query = "SELECT id, name AS full_name FROM patients";
				using (SqlCommand command = new SqlCommand(query, con))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							Patients patients = new Patients();
							patients.id = reader.GetInt32(0);
							patients.name = reader.GetString(1);

							patient.Add(patients);
						}
					}
				}
				String dquery = "SELECT doctor_id, CONCAT(firstname, ' ', lastname) AS full_name FROM doctor";

				using (SqlCommand command = new SqlCommand(dquery, con))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							Doctor doctors = new Doctor();
							doctors.doctor_id = reader.GetInt32(0);
							doctors.firstname = reader.GetString(1);

							doctor.Add(doctors);
						}
					}
				}
			}
			return Page();
		}
		public void OnPost()
		{
			string selectedPatientId = Request.Form["patient_id"];
			if (int.TryParse(selectedPatientId, out int patientId))
			{
				appointmentInfo.patient_id = patientId;
			}
			string selectedDoctorId = Request.Form["doctor_id"];
			if (int.TryParse(selectedDoctorId, out int doctorId))
			{
				appointmentInfo.doctor_id = doctorId;
			}
			appointmentInfo.appointment_date = Convert.ToDateTime(Request.Form["appointment_date"]);
			appointmentInfo.appointment_time = Request.Form["appointment_time"];
			appointmentInfo.appointment_type = Request.Form["appointment_type"];
			appointmentInfo.notes = Request.Form["notes"];



			try
			{
				String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";
				using (SqlConnection con = new SqlConnection(connectionString))
				{
					con.Open();
					String sqlQuery = "INSERT INTO appointments(patient_id, doctor_id, appointment_date, appointment_time, appointment_type, notes) " +
						"VALUES(@patient_id,@doctor_id,@appointment_date,@appointment_time,@appointment_type,@notes)";
					using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
					{
						cmd.Parameters.AddWithValue("@patient_id", appointmentInfo.patient_id);
						cmd.Parameters.AddWithValue("@doctor_id", appointmentInfo.doctor_id);
						cmd.Parameters.AddWithValue("@appointment_date", appointmentInfo.appointment_date);
						cmd.Parameters.AddWithValue("@appointment_time", appointmentInfo.appointment_time);
						cmd.Parameters.AddWithValue("@appointment_type", appointmentInfo.appointment_type);
						cmd.Parameters.AddWithValue("@notes", appointmentInfo.notes);

						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = "somememe";
				return;
			}
			appointmentInfo.appointment_date = null;
			appointmentInfo.appointment_time = "";
			appointmentInfo.appointment_type = "";
			appointmentInfo.notes = "";
			successMessage = "Appointment set successfully";
		}
	}

	public class Patients
	{
		public int id { get; set; }
		public String name { get; set; }
		public String email { get; set; }
		public String phone { get; set; }
		public String address { get; set; }
		public DateTime createdAt { get; set; }
	}

	public class Doctor
	{
		public int doctor_id { get; set; }
		public String firstname { get; set; }
		public String lastname { get; set; }
		public String specialization { get; set; }
		public String email { get; set; }
		public String phone { get; set; }
		public String address { get; set; }
	}

	public class AppointmentInfo
	{
		public int appointment_id { get; set; }
		public DateTime? appointment_date { get; set; }
		public String appointment_time { get; set; }
		public String appointment_type{ get; set; }
		public String notes { get; set; }
		public int patient_id { get; set; }
		public int doctor_id { get; set; }
	}

}
