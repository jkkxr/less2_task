namespace less2_task.Services
{
    public class JsonFileDataProvider<T>: IDataProvider<T>
    {
        private readonly string dataFile;

        public JsonFileDataProvider(string dataFile)
        {
            this.dataFile = dataFile;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            if (!File.Exists(dataFile))
            {
                return new List<T>();
            }
            var json = await File.ReadAllTextAsync(dataFile);
            return System.Text.Json.JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public async Task SaveChanges(ICollection<T> items)
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = System.Text.Json.JsonSerializer.Serialize(items, options);
            await File.WriteAllTextAsync(dataFile, json);
        }
    }
}
