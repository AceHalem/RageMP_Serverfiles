using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay
{
    class Chat : Script
    {
        private int[] staatsf = new int[]
        {
            1,
            2,
            3
        };

        [ServerEvent(Event.ChatMessage)]
        public void EventChatMessage(Client client, string message)
        {

            if (client.HasData("sc"))
            {
                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    for (int i = 0, max = staatsf.Length; i < max; i++)
                    {
                        if (target.GetData("fraktion") == staatsf[i])
                        {
                            target.SendNotification($"[~g~SC~w~] {client.Name} sagt: {message}");
                        }
                    }
                }
            }
            else if (client.HasData("fc"))
            {
                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    if (target.GetData("fraktion") == client.GetData("fraktion"))
                    {
                        target.SendNotification($"[~b~FC~w~] {client.Name} sagt: {message}");
                    }
                }
            } else
            {
                Client[] clients = NAPI.Pools.GetAllPlayers().FindAll(x => x.Position.DistanceTo2D(client.Position) <= 15).ToArray();

                for (int i = 0; i < clients.Length; i++)
                {
                    if (!clients[i].Exists)
                        continue;

                    clients[i].SendChatMessage($"{client.Name} sagt: {message}");
                }
            }
        }
    }
}
