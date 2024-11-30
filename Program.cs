using Npgsql;
using System.Configuration;
using System.Threading.Tasks;
using TurboImporter;

DateTime startJob = DateTime.Now;
try
{
    string tableName = ConfigurationManager.AppSettings["table"];
    string connectionString = ConfigurationManager.AppSettings["connectionString"];
    string pathFile = ConfigurationManager.AppSettings["pathFile"];
    int batchSize = Convert.ToInt32(ConfigurationManager.AppSettings["batchSize"]);
    int totalRows = File.ReadLines(pathFile).Count();
    IEnumerable<int> batchIndex = Enumerable.Range(0, (totalRows + batchSize - 1) / batchSize);
    int totalBatchIndex = batchIndex.Count();
    SemaphoreSlim sm = new SemaphoreSlim(10);
    List<Task> tasks = new List<Task>();
    foreach (var i in batchIndex)
    {
        int startRow = (i * batchSize) + 1;
        try
        {
            await sm.WaitAsync();
            //Console.WriteLine($"process {i} of {batchIndex.Count()}");
            var readFile = FileHelper.ReadFile(pathFile, totalRows, batchSize, startRow);
            tasks.Add(
            ProcessBatch(connectionString, tableName, readFile).ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    Console.WriteLine($"Task for {i} of {totalBatchIndex} completed successfully.");
                }
                else if (t.IsFaulted)
                {
                    Console.WriteLine($"Task for {i} of {totalBatchIndex} failed: {t.Exception?.Message}");
                }
            })
        );
        }
        catch (Exception)
        {
        }
        finally
        {
            sm.Release();
        }
    }
    await Task.WhenAll(tasks);
    DateTime endJob = DateTime.Now;
    Console.WriteLine($"total duration execute import:{(endJob - startJob).TotalSeconds}");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
finally
{

}
Console.ReadKey();

async Task ProcessBatch(string connection, string tableName, List<string> batch)
{
    using (var conn = new NpgsqlConnection(connection))
    {
        conn.Open();
        try
        {
            using var writer = conn.BeginTextImport($"COPY {tableName} (transaction_id,customer_id,card_number,timestamp,merchant_category,merchant_type,merchant,amount,currency,country,city,city_size,card_type,card_present,device,channel,device_fingerprint,ip_address,distance_from_home,high_risk_merchant,transaction_hour,weekend_transaction,velocity_last_hour,is_fraud) FROM STDIN WITH (FORMAT csv)");
            foreach (var row in batch)
            {
                await writer.WriteLineAsync(row);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}