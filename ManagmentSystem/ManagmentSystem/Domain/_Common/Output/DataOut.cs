namespace Domain.Common.Output;

public class DataOut
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? RemoveDate { get; set; }
    public bool IsRemoved { get; set; }
}