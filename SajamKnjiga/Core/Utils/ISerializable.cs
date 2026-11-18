namespace Core.Utils
{
    public interface ISerializable
    {
        int Id { get; set; }
        string ToLine();
        void FromLine(string line);
    }
}