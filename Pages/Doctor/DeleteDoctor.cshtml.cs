using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace healthcareMIS.Pages.Doctor
{
    public class DeleteDoctorModel : PageModel
    {
        public DoctorInfo doctorInfo = new DoctorInfo();
        public String errorMessage = "";
        public void OnGet()
        {
            String id = Request.Query["doctor_id"];
            try
            {
                String connectionString = "Data Source=DRKST-MTTR\\SQLEXPRESS;Initial Catalog=healthcareMIS;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "DELETE FROM Doctor WHERE doctor_id = @doctor_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@doctor_id", id);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }
    }
}
