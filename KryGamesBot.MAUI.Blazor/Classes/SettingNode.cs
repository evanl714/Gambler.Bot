using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.MAUI.Blazor.Classes
{
    public class SettingNode
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public Type UserControl { get; set; }
        public Dictionary<string, object> ComponentParameters { get; set; }

    }
}
