namespace testingproject
{
    [SetUpFixture]
    public class TestsSetup
    {
        public TestsSetup()
        {

        }

        FileSystemWatcher _watcher = new FileSystemWatcher("logging");

        [OneTimeSetUp]
        public void Setup()
        {
            _watcher = new FileSystemWatcher("logging");

            _watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            _watcher.Changed += (s, e) =>
            {
                Task task = new Task(async () =>
                {
                    //await Task.Delay(500);
                    var fileLines = await File.ReadAllLinesAsync(e.FullPath);
                    StorageSingletonProvider.Instance.LinesOfText = fileLines.Length;
                });
                
                task.Start();
            };

            _watcher.Created += (s, e) =>
            {
                StorageSingletonProvider.Instance.LinesOfText = -1;
            };


            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        }

        [OneTimeTearDown]
        public void Test1()
        {
            _watcher.Dispose();
        }
    }

    public sealed class StorageSingletonProvider
    {
        private StorageSingletonProvider() 
        {
        }

        private static readonly object lockObj = new object();

        private static MyStorage _instance = default!;
        public static MyStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new MyStorage();
                        }
                    }
                }
                return _instance;
            }
        }
    }

    public class MyStorage
    {
        public int LinesOfText { get; set; }
        public MyStorage()
        {

        }
    }
}

