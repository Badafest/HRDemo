using System.Runtime.Serialization;

[DataContract]
public class UserRequest
{
    [DataMember]
    public string Username { get; set; } = "";
}

[DataContract]
public class EmployeeRequest
{
    [DataMember]
    public string SearchName { get; set; } = "";
    [DataMember]
    public int[] ManagedDepartments { get; set; } = new int[] { };
}
