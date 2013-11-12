using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GPOne.Interfaces
{
    /// <summary>
    /// This interface contains a set of methods for keyboard input
    /// </summary>
    public interface IReceivesInput
    {
        void OnKeyDown(object sender, KeyEventArgs e);
        void OnKeyUp(object sender, KeyEventArgs e);
    }
}
