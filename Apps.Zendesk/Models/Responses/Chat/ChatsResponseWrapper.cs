using Apps.Zendesk.Dtos;

namespace Apps.Zendesk.Models.Responses.Chat;

public class ChatsResponseWrapper : PaginatedResponse
{
    public IEnumerable<ChatDto> Chats { get; set; }
}