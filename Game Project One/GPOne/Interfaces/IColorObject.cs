using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GPOne.Interfaces
{
    /// <summary>
    /// IColorObject enforces a set of properties which allow custom colors to be applied to 
    /// the object
    /// </summary>
    public interface IColorObject
    {
        /// <summary>
        /// The current Fill Color of the object
        /// </summary>
        Brush Fill { get; set; }

        /// <summary>
        /// The current Stroke Color of the object
        /// </summary>
        Brush Stroke { get; set; }

        /// * DISABLED
        /// * 
        /// * NOTE:
        /// * The following properties are currently disabled until I can think of a compelling and useful way to
        /// * implement the design pattern. My hope is that the two lists that follow will be useful to keep track
        /// * of all the previous colors used by this object so that the object may swap colors out as necessary without
        /// * having any knowledge of the actual colors that it is using.
        /// * 
        /// * /// <summary>
        /// * /// A List containing all Fill Colors that may be used by this object
        /// * /// </summary>
        /// * List<Brush> FillColors { get; set; }
        ///
        /// * /// <summary>
        /// * /// A List containing all Stroke Colors that may be used by this object
        /// * /// </summary>
        /// * List<Brush> StrokeColors { get; set; }
        /// * 
        /// */
    }
}
