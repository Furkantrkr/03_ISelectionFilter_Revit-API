using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace _03_ISelectionFilter.Extensions
{
    public static class CurveExtensions
    {
        /// <summary>
        /// This method is used to visulalize a curve in a revit document.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="document"></param>
        public static void Visualize(
            this Curve curve, Document document )
        {
            document.CreateDirectShape(new List<GeometryObject>() { curve });

        }
    }
}
 