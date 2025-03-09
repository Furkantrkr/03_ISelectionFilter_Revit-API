using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _03_ISelectionFilter.Extensions;
using Autodesk.Revit.DB;

namespace _03_ISelectionFilter.GeometryUtils
{
    public static class SolidExtensions
    {
        public static DirectShape Visualize(
            this Solid solid, Document document)
        {
            return document.CreateDirectShape(new List<GeometryObject> { solid });            
        }
    }
}
