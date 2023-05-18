using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace healthcareMIS.Pages.Patients
{
    public class ViewPatientsModel : PageModel
    {
        public Patient patientInfo = new Patient();
        public List<Patient> patients { get; set; } = new List<Patient>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM patients";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Patient patientInfo = new Patient();
                                patientInfo.id = "" + reader.GetInt32(0);
                                patientInfo.name = reader.GetString(1);
                                patientInfo.email = reader.GetString(2);
                                patientInfo.phone = reader.GetString(3);
                                patientInfo.address = reader.GetString(4);
                                patientInfo.createdAt = reader.GetDateTime(5);

                                patients.Add(patientInfo);
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

        public class Patient
        {
            public String id { get; set; }
            public String name { get; set; }
            public String email { get; set; }
            public String phone { get; set; }
            public String address { get; set; }
            public DateTime createdAt { get; set; }
        }
    }
}
