using System.Runtime.Serialization;

[DataContract]
public class EmployeeReport
{
    [DataMember]
    public int EmployeeID { get; set; } = 0;
    [DataMember]
    public string FirstName { get; set; } = "";
    [DataMember]
    public string LastName { get; set; } = "";
    [DataMember]
    public string State { get; set; } = "";
    [DataMember]
    public string Country { get; set; } = "";
    [DataMember]
    public double AnnualSalary { get; set; } = 0;
    [DataMember]
    public int PresentDays { get; set; } = 0;
    [DataMember]
    public int LateDays { get; set; } = 0;
    [DataMember]
    public int LeaveDays { get; set; } = 0;
    [DataMember]
    public int SickLeave { get; set; } = 0;
    [DataMember]
    public int CasualLeave { get; set; } = 0;
    [DataMember]
    public int ParentalLeave { get; set; } = 0;
    [DataMember]
    public int BereavementLeave { get; set; } = 0;
    [DataMember]
    public int UnpaidLeave { get; set; } = 0;
}
