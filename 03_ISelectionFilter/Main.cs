using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using _03_ISelectionFilter.Extensions.SelectionExtensions;

namespace _03_ISelectionFilter
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Main : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Application app = uiApp.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            // Create a selection filter object
            LinkableSelectionFilter filter = new LinkableSelectionFilter(doc,e => e.Category.Id.Value == (int)BuiltInCategory.OST_Walls);

            // Selection refernces
            IList<Reference> references = uiDoc.Selection.PickObjects(ObjectType.PointOnElement, filter, "Please select walls");

            TaskDialog.Show("Count of selected elements",references.Count.ToString());

            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}
