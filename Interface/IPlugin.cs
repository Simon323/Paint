using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Interface
{
    public interface IPlugin : IDisposable
    {
        string GetString();

        string GetID();

        MenuItem GetMenuItem();

        event EventHandler Finished;

        void Initialize(Canvas canvas, Color color, int thikness);

        void UpdateParameters(Color color, int thinkess);
    }
}
