using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace healthcareMIS.Pages.Medical_Record
{
    public class MedicalHistoryModel : PageModel
    {
		public MedicInfo medicInfo = new MedicInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
        {
        }

		public class MedicInfo
		{
			public int mh_id { get; set; }
			public DateTime visit_date { get; set; }
			public String diagnosis { get; set; }
			public String treatment { get; set; }
			public String medictions { get; set; }
			public String notes { get; set; }
			public int patient_id { get; set; }
			public int doctor_id { get; set; }
		}
	}
}
