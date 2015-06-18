using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginOne
{
    public class PluginOne : Interface.IPluginOne
    {
        public string GetString()
        {
            return "Hello world";
        }
    }
}
