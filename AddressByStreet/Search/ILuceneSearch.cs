using System.Collections.Generic;

namespace AddressByStreet.Search
{
    public interface ILuceneSearch<T>
    {
        void AddToLuceneIndex(IEnumerable<T> dataToIndex);
        IEnumerable<T> Search(string term);
        IEnumerable<T> Search(string term, string field);
    }
}