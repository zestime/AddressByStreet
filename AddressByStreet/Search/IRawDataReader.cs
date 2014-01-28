using System.Collections.Generic;

namespace AddressByStreet
{
    public interface IRawDataReader<out T>
    {
        IEnumerable<T> Read(string name);
        IEnumerable<T> Read(IEnumerable<string> names);
    }
}