using System.Runtime.Serialization;

[DataContract]
public class UserResponse: EmployeeResponse
{
    [DataMember]
    public int UserID { get; set; } = 0;

    [DataMember]
    public short UserRole { get; set; } = 0;

    [DataMember]
    public DepartmentResponse[] ManagedDepartments { get; set; } = new DepartmentResponse[] { };
}

[DataContract]
public class EmployeeResponse
{
    [DataMember]
    public int EmployeeID { get; set; } = 0;
    [DataMember]
    public string EmployeeName { get; set; } = "";
}

[DataContract]
public class DepartmentResponse
{
    [DataMember]
    public int DepartmentID { get; set; } = 0;
    [DataMember]
    public string DepartmentName { get; set; } = "";
}