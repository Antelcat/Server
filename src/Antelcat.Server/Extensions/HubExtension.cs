using Microsoft.AspNetCore.SignalR;

namespace Antelcat.Server.Extensions;

public static class HubExtension
{
    public static Task AddToGroupAsync(this Hub hub, string groupName) => 
        hub.Groups.AddToGroupAsync(groupName, hub.Context.ConnectionId);

    public static Task RemoveFromGroupAsync(this Hub hub, string groupName) =>
        hub.Groups.RemoveFromGroupAsync(groupName, hub.Context.ConnectionId);
}