namespace less2_task.Services
{
    public class JsonFileDataProvider<T>: IDataProvider<T>
    {
        private readonly string dataFile;
        private readonly SemaphoreSlim _fileLock = new(1, 1);

        public JsonFileDataProvider(string dataFile)
        {
            this.dataFile = dataFile;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            await _fileLock.WaitAsync();
            try
            {
                if (!File.Exists(dataFile))
                {
                    return new List<T>();
                }
                var json = await File.ReadAllTextAsync(dataFile);
                return System.Text.Json.JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task SaveChanges(ICollection<T> items)
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = System.Text.Json.JsonSerializer.Serialize(items, options);
            await _fileLock.WaitAsync();
            try
            {
                await File.WriteAllTextAsync(dataFile, json);
            }
            finally
            {
                _fileLock.Release();
            }
        }
    }
}
