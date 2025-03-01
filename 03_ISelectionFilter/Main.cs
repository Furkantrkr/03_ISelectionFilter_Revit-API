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

            ElementSelectionFilter filter = new ElementSelectionFilter(elem => elem.Category.Id == new ElementId(BuiltInCategory.OST_Walls));

            IList<Reference> reference = uiDoc.Selection.PickObjects(ObjectType.Element, filter, "Select a wall or walls");

            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}
