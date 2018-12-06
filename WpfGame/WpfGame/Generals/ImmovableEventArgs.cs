using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfGame.Models;
using WpfGame.Models.Playgroundobjects;

namespace WpfGame.Generals
{
    public class ImmovableEventArgs : EventArgs
    {
        public ImmovableObject Coin { get; }

        public ImmovableEventArgs(ImmovableObject coin)
        {
            Coin = coin;
        }
    }
}
