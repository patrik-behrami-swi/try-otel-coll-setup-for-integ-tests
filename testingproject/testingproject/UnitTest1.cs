namespace testingproject
{
    public class Tests
    {


        [Test]
        public void Test1()
        {
            for (int i = 0; i < 100000000; i++)
            {
                Thread.Sleep(1);
                var ee = StorageSingletonProvider.Instance.LinesOfText;
            }
        }
    }
}