using System.Collections.Generic;
using System.Collections.ObjectModel;
using AddressByStreet.Model;

namespace AddressBook
{
    public class MainWindowViewModel
    {
        public string SearchTerm { get; set; }
        public ObservableCollection<Address> Rows { get; set; }

        public MainWindowViewModel()
        {
            Rows = new ObservableCollection<Address>();
        }
    }
}