using healthcareMIS.Pages.Appointments;
using healthcareMIS.Pages.Doctor;
using healthcareMIS.Pages.Patients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static healthcareMIS.Pages.Medical_Record.MedicalHistoryModel;
using static healthcareMIS.Pages.Medical_Record.UpdateModel;

namespace healthcareMIS.Pages.Medical_Record
{
    public class ViewMedicalHistoryModel : PageModel
    {
        public Medical medicInfo = new Medical();
		public PatientInfo patientInfo = new PatientInfo();
		public DoctorInfo doctorInfo = new DoctorInfo();
		public List<PatientInfo> Patient { get; set; } = new List<PatientInfo>();
		public List<DoctorInfo> Doctor { get; set; } = new List<DoctorInfo>();
		public List<Medical> list = new List<Medical>();
		public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM medicalhistory";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Medical medicInfo = new Medical();
                                medicInfo.mh_id = "" + reader.GetInt32(0);
                                medicInfo.visit_date = reader.GetDateTime(1);
                                medicInfo.diagnosis = reader.GetString(2);
                                medicInfo.treatment = reader.GetString(3);
                                medicInfo.medications = reader.GetString(4);
                                medicInfo.notes = reader.GetString(5);
                                medicInfo.patient_id = "" + reader.GetInt32(6);
                                medicInfo.doctor_id = "" + reader.GetInt32(7);

                                list.Add(medicInfo);
                            }
                        }
                    }
                    // Fetch patient names
                    String patientQuery = "SELECT id, name AS full_name FROM patients";
                    using (SqlCommand command = new SqlCommand(patientQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PatientInfo patient = new PatientInfo();
                                patient.id = reader.GetInt32(0);
                                patient.name = reader.GetString(1);

                                Patient.Add(patient);
                            }
                        }
                    }
                    // Fetch doctor names
                    String doctorQuery = "SELECT doctor_id, CONCAT(firstname, ' ', lastname) AS full_name FROM doctor";
                    using (SqlCommand command = new SqlCommand(doctorQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DoctorInfo doctor = new DoctorInfo();
                                doctor.doctor_id = reader.GetInt32(0);
                                doctor.firstname = reader.GetString(1);

                                Doctor.Add(doctor);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }
}
