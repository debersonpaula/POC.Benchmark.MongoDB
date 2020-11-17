using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DataApi.Repository;

namespace DataBenchmarks
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TestWithCollections>();
            BenchmarkRunner.Run<TestWithDocuments>();
        }
    }

    [MarkdownExporter, HtmlExporter]
    [MinColumn, MaxColumn]
    [MemoryDiagnoser]
    public class TestBase
    {
        protected IPersonRepository _repo;

        [Benchmark]
        [Arguments(1)]
        [Arguments(5)]
        [Arguments(10)]
        [Arguments(50)]
        public void RunQuery(int qty)
        {
            for (var i = 1; i <= qty; i++)
            {
                _repo.GetPersonById(i).Wait();
            }
        }
    }

    public class TestWithCollections : TestBase
    {
        public TestWithCollections()
        {
            _repo = new PersonCollectionRepository();
        }
    }

    public class TestWithDocuments : TestBase
    {
        public TestWithDocuments()
        {
            _repo = new PersonDocumentRepository();
        }
    }
}
