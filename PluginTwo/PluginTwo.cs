using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTwo
{
    public class PluginTwo : Interface.IPluginTwo
    {
        public string GetString()
        {
            return "Hello world 2";
        }
    }
}
