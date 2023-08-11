// using Apps.Zendesk.Dtos;
// using Apps.Zendesk.Models.Requests.Chat;
// using Apps.Zendesk.Models.Responses.Chat;
// using Blackbird.Applications.Sdk.Common;
// using Blackbird.Applications.Sdk.Common.Actions;
// using Blackbird.Applications.Sdk.Common.Authentication;
// using RestSharp;
//
// namespace Apps.Zendesk.Actions;
//
// [ActionList]
// public class ChatActions
// {
//     [Action("List chats", Description = "List all chats")]
//     public ListChatsResponse ListChats(
//         IEnumerable<AuthenticationCredentialsProvider> creds)
//     {
//         var client = new ZendeskClient(creds);
//         var request = new ZendeskRequest("https://www.zopim.com/api/v2/chats", Method.Get, creds);
//
//         var response = client.GetPaginated<ChatsResponseWrapper>(request);
//         var chats = response.SelectMany(x => x.Chats).ToArray();
//
//         return new(chats);
//     }
//     
//     [Action("Get chat", Description = "Get specific chat")]
//     public Task<ChatDto> GetChat(
//         IEnumerable<AuthenticationCredentialsProvider> creds,
//         [ActionParameter] [Display("Chat ID")] string chatId)
//     {
//         var client = new ZendeskClient(creds);
//         var request = new ZendeskRequest($"/api/v2/chats/{chatId}", Method.Get, creds);
//
//         return client.ExecuteWithHandling<ChatDto>(request);
//     }
//     
//     [Action("Create chat", Description = "Create new chat")]
//     public Task<ChatDto> CreateChat(
//         IEnumerable<AuthenticationCredentialsProvider> creds,
//         [ActionParameter] CreateChatRequest input)
//     {
//         var client = new ZendeskClient(creds);
//         var request = new ZendeskRequest("/api/v2/chats/", Method.Post, creds);
//         request.AddJsonBody(input);
//
//         return client.ExecuteWithHandling<ChatDto>(request);
//     }   
//     
//     [Action("Delete chat", Description = "Delete specific chat")]
//     public Task DeleteChat(
//         IEnumerable<AuthenticationCredentialsProvider> creds,
//         [ActionParameter] [Display("Chat ID")] string chatId)
//     {
//         var client = new ZendeskClient(creds);
//         var request = new ZendeskRequest($"/api/v2/chats/{chatId}", Method.Delete, creds);
//
//         return client.ExecuteWithHandling(request);
//     }
// }