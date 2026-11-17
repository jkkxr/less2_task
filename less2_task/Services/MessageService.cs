using less2_task.Models;
using Microsoft.EntityFrameworkCore;

namespace less2_task.Services
{
    public class MessageService(ApiDbContext dbContext)
    {
        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await dbContext.Messages.ToListAsync();
        }

        public async Task<Message?> GetMessageByIdAsync(int id)
        {
            return await dbContext.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Message>> GetMessagesBySender(string sender)
        {
            return await dbContext.Messages
                .Where(m => m.Sender.Equals(sender, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public async Task<Message> AddMessageAsync(Message newMessage)
        {
            await dbContext.Messages.AddAsync(newMessage);
            await dbContext.SaveChangesAsync();
            return newMessage;
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            var message = await dbContext.Messages.FirstOrDefaultAsync(p => p.Id == id);
            if(message == null)
            {
                return false;
            }
            dbContext.Messages.Remove(message);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
