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
using _03_ISelectionFilter.Extensions;
using System.Windows.Documents;

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

            try
            {
                List<Element> selectedElements = uiDoc.PickElements(e => e is FamilyInstance, PickElementsOptionFactory.CreateCurrentDocumentOption());
                Element firstEl = selectedElements.First();
                ElementId id = firstEl.Id;

                LocationPoint elLocation = firstEl.Location as LocationPoint;
                XYZ elLocationPt = elLocation.Point;

                XYZ vector = new XYZ(1,2,0);
                Line line = vector.VisualizeAsLine(doc, elLocationPt);

                using (Transaction t = new Transaction(doc, "Create Poins"))
                {
                    t.Start();

                    ElementTransformUtils.MoveElement(doc, id, vector );
                    doc.CreateDirectShape(new List<GeometryObject>() { line });

                    t.Commit();
                }
            }
            catch (OperationCanceledException)
            {
                TaskDialog.Show("Revit", "Operation canceled");
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}
