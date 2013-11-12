using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GPOne.Interfaces
{
    /// <summary>
    /// This interface enforces gravity on objects which implement it.
    /// </summary>
    public interface IHasGravity
    {

        Vector ApplyGravity(long ElapsedTime);
    }
}
