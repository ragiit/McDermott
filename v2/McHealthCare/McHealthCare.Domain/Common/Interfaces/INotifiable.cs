namespace McHealthCare.Domain.Common.Interfaces
{
    public interface INotifiable
    {
        string Type { get; }
        object Data { get; }
    }
}