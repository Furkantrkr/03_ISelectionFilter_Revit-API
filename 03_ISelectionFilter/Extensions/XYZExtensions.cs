using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace _03_ISelectionFilter.Extensions
{
    public static class XYZExtensions
    {
        public static Line VisualizeAsLine(
            this XYZ vector, Document doc, XYZ origin = null, int scale = 1)
        {
            if (origin == null ) origin = XYZ.Zero;

            XYZ endPoint = origin + vector * scale;

            Line line = Line.CreateBound(origin, endPoint);

            return line;
        }
    }
}
