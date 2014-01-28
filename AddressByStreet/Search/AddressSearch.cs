using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AddressByStreet.Lib;
using AddressByStreet.Model;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace AddressByStreet.Search
{
    public class AddressSearch : ILuceneSearch<Address>
    {
        public bool IsIndexed { get; set; }

        private static FSDirectory _directory;

        private static FSDirectory Directory
        {
            get
            {
                if (_directory == null)
                {
                    if (!System.IO.Directory.Exists(Environment.IndexDir))
                        System.IO.Directory.CreateDirectory(Environment.IndexDir);

                    _directory = FSDirectory.Open(new DirectoryInfo(Environment.IndexDir));
                }

                if (IndexWriter.IsLocked(_directory)) IndexWriter.Unlock(_directory);

                var lockFilePath = Path.Combine(Environment.IndexDir, "write.lock");

                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);

                return _directory;
            }
        }

        public AddressSearch()
        {
            IsIndexed = IndexReader.IndexExists(Directory);
        }

        private void CreateLuceneIndex(Address addr, IndexWriter writer)
        {
            var doc = new Document();
            doc.Add(new Field("Code", addr.Code, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Street", addr.Street, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("LandLot", addr.LandLot, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Post", addr.Post, Field.Store.YES, Field.Index.ANALYZED));
            writer.AddDocument(doc);
        }

        public void AddToLuceneIndex(IEnumerable<Address> dataToIndex)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var element in dataToIndex)
                    CreateLuceneIndex(element, writer);

                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }

            IsIndexed = true;
        }

        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        } 

        public IEnumerable<Address> Search(string searchQuery)
        {
            return Search(searchQuery, null);
        }

        public IEnumerable<Address> Search(string searchQuery, string searchField)
        {
            if (!IsIndexed) throw new FileNotFoundException("Index is not exists.");

            using (var searcher = new IndexSearcher(Directory, false))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                QueryParser parser;
                if (!string.IsNullOrEmpty(searchField))
                {
                    parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                }
                else
                {
                    parser = new MultiFieldQueryParser(Version.LUCENE_30,
                                                        new[] {"Street", "LandLot", "Post"},
                                                        analyzer);
                }

                parser.AllowLeadingWildcard = Environment.AllowLeadingWildcard;
                var query = ParseQuery(searchQuery, parser);
                var hits = searcher.Search(query, null, Environment.MaxResult, Sort.RELEVANCE).ScoreDocs;
                var results = MapLuceneToDataList(hits, searcher);
                analyzer.Close();
                searcher.Dispose();
                return results;

            }
        }

        private static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        private static Address MapDocumentToAddress(Document doc)
        {
            return new Address
                       {
                           Code = doc.Get("Code"),
                           Street = doc.Get("Street"),
                           LandLot = doc.Get("LandLot"),
                           Post = doc.Get("Post"),
                       };
        }

        private static IEnumerable<Address> MapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => MapDocumentToAddress(searcher.Doc(hit.Doc))).ToList();
        }
    }
}