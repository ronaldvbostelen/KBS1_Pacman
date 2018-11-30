using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfGame.Models;

namespace WpfGame.Generals
{
    public class ImmovableEventArgs : EventArgs
    {
        public ImmovableObject Coin { get; set; }

        public ImmovableEventArgs(ImmovableObject coin)
        {
            Coin = coin;
        }
    }
}
