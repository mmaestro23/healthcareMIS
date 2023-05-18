using healthcareMIS.Pages.Doctor;
using healthcareMIS.Pages.Patients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using static healthcareMIS.Pages.Medical_Record.MedicalHistoryModel;
using static healthcareMIS.Pages.Medical_Record.UpdateModel;

namespace healthcareMIS.Pages.Appointments
{
    public class ViewAppointmentsModel : PageModel
    {
        public Appointment appointmentInfo = new Appointment();
        public PatientInfo patientInfo = new PatientInfo();
        public DoctorInfo doctorInfo = new DoctorInfo();
        public List<PatientInfo> Patient { get; set; } = new List<PatientInfo>();
        public List<DoctorInfo> Doctor { get; set; } = new List<DoctorInfo>();
        public List<Appointment> list = new List<Appointment>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM appointments";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Appointment appointmentInfo = new Appointment();
                                appointmentInfo.appointment_id = "" + reader.GetInt32(0);
                                appointmentInfo.appointment_date = reader.GetDateTime(1);
                                appointmentInfo.appointment_time = reader.GetString(2);
                                appointmentInfo.appointment_type = reader.GetString(3);
                                appointmentInfo.notes = reader.GetString(4);
                                appointmentInfo.patient_id = "" + reader.GetInt32(5);
                                appointmentInfo.doctor_id = "" + reader.GetInt32(6);

                                list.Add(appointmentInfo);
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

        public class Appointment
        {
            public String appointment_id { get; set; }
            public DateTime? appointment_date { get; set; }
            public String appointment_time { get; set; }
            public String appointment_type { get; set; }
            public String notes { get; set; }
            public String patient_id { get; set; }
            public String doctor_id { get; set; }
        }
    }
}
