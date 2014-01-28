namespace AddressByStreet.Lib
{
    public interface IModelFormatter<in T1, out T2>
    {
        T2 FormatTo(T1 data);
    }
}