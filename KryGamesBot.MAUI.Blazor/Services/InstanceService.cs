using DoormatBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.MAUI.Blazor.Services
{
    public interface IInstanceService
    {
        void AddInstance(string name,DoormatBot.Doormat instance);
        DoormatBot.Doormat GetInstance(string name);
    }

    public class InstanceService : IInstanceService
    {
        Dictionary<string, DoormatBot.Doormat> Instances = new Dictionary<string, DoormatBot.Doormat>();
        public DoormatBot.Doormat GetInstance(string name)
        {
            if (Instances.ContainsKey(name))
                return Instances[name];
            return null;
        }
        public void AddInstance(string name, DoormatBot.Doormat instance)
        {
            if (!Instances.ContainsKey(name))
                Instances.Add(name, instance);
        }
    }
}
