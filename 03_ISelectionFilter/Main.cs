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

namespace _03_ISelectionFilter
{
    public class Main : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Application app= uiApp.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            return Autodesk.Revit.UI.Result.Succeeded; 
        }
    }
}
