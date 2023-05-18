
using healthcareMIS.Pages.Appointments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static healthcareMIS.Pages.Medical_Record.UpdateModel;

namespace healthcareMIS.Pages.Doctor
{
    public class ViewDoctorModel : PageModel
    {
        public Doctor doctorInfo = new Doctor();
        public List<Doctor> doctors { get; set; } = new List<Doctor>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM doctor";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Doctor doctorInfo = new Doctor();
                                doctorInfo.doctor_id = "" + reader.GetInt32(0);
                                doctorInfo.firstname = reader.GetString(1);
                                doctorInfo.lastname = reader.GetString(2);
                                doctorInfo.specialization = reader.GetString(3);
                                doctorInfo.email = reader.GetString(4);
                                doctorInfo.phone = reader.GetString(5);
                                doctorInfo.address = reader.GetString(6);

                                doctors.Add(doctorInfo);
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
