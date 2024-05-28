using Gambler.Bot.AutoBet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.MAUI.Blazor.Services
{
    public interface IInstanceService
    {
        void AddInstance(string name,Gambler.Bot.AutoBet.Doormat instance);
        Gambler.Bot.AutoBet.Doormat GetInstance(string name);
    }

    public class InstanceService : IInstanceService
    {
        Dictionary<string, Gambler.Bot.AutoBet.Doormat> Instances = new Dictionary<string, Gambler.Bot.AutoBet.Doormat>();
        public Gambler.Bot.AutoBet.Doormat GetInstance(string name)
        {
            if (Instances.ContainsKey(name))
                return Instances[name];
            return null;
        }
        public void AddInstance(string name, Gambler.Bot.AutoBet.Doormat instance)
        {
            if (!Instances.ContainsKey(name))
                Instances.Add(name, instance);
        }
    }
}
