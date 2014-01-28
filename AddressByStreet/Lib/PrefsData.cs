using System.Collections;
using System.ComponentModel;

namespace AddressByStreet.Lib
{
    public class PrefsData
    {
        public ArrayList IndexArray;
        
        [DefaultValue(50)]
        public int MaxIndexedCount;

        public PrefsData()
        {
            MaxIndexedCount = 50;
        }
    }
}