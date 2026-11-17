using less2_task.Models;

namespace less2_task.Services
{
    public class MessageProvider : JsonFileDataProvider<Message>
    {
        public MessageProvider(string dataFile) : base(dataFile)
        {
        }
    }
}
