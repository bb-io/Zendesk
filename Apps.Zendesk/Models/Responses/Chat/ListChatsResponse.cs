using Apps.Zendesk.Dtos;

namespace Apps.Zendesk.Models.Responses.Chat;

public record ListChatsResponse(ChatDto[] Chats);