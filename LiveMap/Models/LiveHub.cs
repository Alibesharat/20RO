using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LiveMap.Models
{
    public class LiveHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task GetGeo(string token,string geo)
        {
            await Clients.Group(token).SendAsync("livegeo", geo);
        }
    }
}
