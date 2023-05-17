using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace healthcareMIS.Pages.Appointments
{
    public class AppointmentsModel : PageModel
    {
        public void OnGet()
        {
        }
    }

	public class DoctorInfo
	{
		public int appointment_id { get; set; }
		public String appointment_date { get; set; }
		public String appointement_time { get; set; }
		public String appointment_type{ get; set; }
		public String notes { get; set; }
		public String patient_id { get; set; }
		public String doctor_id { get; set; }
	}

}
