using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRDemoAdmin.Services.Models
{
    public class AttendanceResponse
    {
        public int AttendanceID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTimeOffset Date { get; set; }
        [DisplayName("Check In Time")]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public System.DateTimeOffset CheckInTime { get; set; }
        [DisplayName("Check Out Time")]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public System.DateTimeOffset CheckOutTime { get; set; }
        public string Status { get; set; }
        public EmployeeResponse Employee { get; set; }
    }
}