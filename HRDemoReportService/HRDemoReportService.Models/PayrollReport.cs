using System.Runtime.Serialization;

[DataContract]
public class PayrollReport
{
    [DataMember]
    public int PayrollID { get; set; } = 0;
    [DataMember]
    public short Month { get; set; } = 1;
    [DataMember]
    public short Year { get; set; } = 1900;
    [DataMember]
    public double GrossAmount { get; set; } = 0;
    [DataMember]
    public double PreTaxDeduction { get; set; } = 0;
    [DataMember]
    public double TaxDeduction { get; set; } = 0;
    [DataMember]
    public double PostTaxDeduction { get; set; } = 0;
    [DataMember]
    public double NetAmount { get; set; } = 0;
}
