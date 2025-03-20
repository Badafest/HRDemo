using System.Runtime.Serialization;

[DataContract]
public class ReportRequest
{
    [DataMember]
    public int EmployeeID { get; set; } = 0;
    [DataMember]
    public short Year { get; set; } = 1900;
    [DataMember]
    public short Month { get; set; } = 1;
    [DataMember]
    public float TimezoneOffset { get; set; } = 0;
}
