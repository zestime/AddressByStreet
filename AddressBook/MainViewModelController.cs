using System.Linq;
using AddressByStreet;
using AddressByStreet.Lib;
using AddressByStreet.Model;
using AddressByStreet.Search;

namespace AddressBook
{
    public class MainViewModelController
    {
        private readonly IRawDataReader<RawAddress> reader;
        private readonly IModelFormatter<RawAddress, Address> formatter;
        private readonly ILuceneSearch<Address> searcher;

        private MainWindowViewModel viewModel;

        public MainViewModelController(MainWindowViewModel viewModel)
        {
            Environment.AllowLeadingWildcard = true;
            reader = new AddressReader();
            formatter = new DefaultFormatter();
            searcher = new AddressSearch();
            Start();

            this.viewModel = viewModel;
        }

        public void Start()
        {
            var files = FileManager.Load(Environment.DataDir);
            
            if (files != null)
            {
                var addrs = reader.Read(files).Select(formatter.FormatTo);
                searcher.AddToLuceneIndex(addrs);
                FileManager.TempClear();
            }

            Environment.Preferences.Save();
        }

        public void DoLuceneSearch()
        {
            var results = searcher.Search(viewModel.SearchTerm);
            
            viewModel.Rows.Clear();
            foreach (var r in results)
            {
                viewModel.Rows.Add(r);
            }
        }
    }
}