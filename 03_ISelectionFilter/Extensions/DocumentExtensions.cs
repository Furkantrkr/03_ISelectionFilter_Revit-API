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
            this Document document, List<GeometryObject> geometryObjects, BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel)
        {
            DirectShape directShape = DirectShape.CreateElement(document, new ElementId(builtInCategory));
            directShape.SetShape(geometryObjects);

            return directShape;
        }
        public static void Run(
            this Document document, Action doAction, string transactionName = "Default transaction name")
        {
            using (var transaction = new Transaction(document, transactionName))
            {
                transaction.Start();
                doAction.Invoke();
                transaction.Commit();
            }
        }
    }
}
