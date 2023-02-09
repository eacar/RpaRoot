namespace Rpa.Contracts
{
    public interface IEnumAttribute<out T>
    {
        T Value { get; }
    }
}