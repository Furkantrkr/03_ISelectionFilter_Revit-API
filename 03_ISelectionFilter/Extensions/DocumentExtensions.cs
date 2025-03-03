using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace _03_ISelectionFilter.Extensions
{
    public static class DocumentExtensions
    {
        /// <summary>
        /// This method is used to create direct shapes in a document
        /// </summary>
        public static DirectShape CreateDirectShape(
            this Document doc, List<GeometryObject> geometryObjects, BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel)
        {


            DirectShape directShape = DirectShape.CreateElement(doc, new ElementId(builtInCategory));
            directShape.SetShape(geometryObjects);

            return directShape;
        } 
    }
}
