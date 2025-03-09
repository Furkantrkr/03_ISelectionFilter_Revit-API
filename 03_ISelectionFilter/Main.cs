using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;s
using Autodesk.Revit.UI.Selection;
using _03_ISelectionFilter.Extensions.SelectionExtensions;
using _03_ISelectionFilter.Extensions;
using System.Windows.Documents;
using _03_ISelectionFilter.GeometryUtils;
using Microsoft.SqlServer.Server;

namespace _03_ISelectionFilter
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Main : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApplication = commandData.Application;
            Application application = uiApplication.Application;
            UIDocument uiDocument = uiApplication.ActiveUIDocument;
            Document document = uiDocument.Document;
            Element selectedElement = uiDocument.GetSelectedElements().First();

            Options options = new Options();

            //List<Solid> solids = selectedElement.get_Geometry(options)
            //                                                 .OfType<Solid>()
            //                                                 .ToList();
            //TaskDialog.Show("Message", solids.Count.ToString());

            List<Solid> solids = selectedElement.get_Geometry(options)
                                                           .OfType<GeometryInstance>()
                                                           .SelectMany(g => g.GetInstanceGeometry())
                                                           .OfType<Solid>()
                                                           .Where(s => s.Volume > 0)
                                                           .ToList();

            //IEnumerable<Element> subElementsOfFamilyInstance = (selectedElement as FamilyInstance).GetSubComponentIds()
            //                                                                     .Select(id => document.GetElement(id));


            //List<Solid> subSolids = subElementsOfFamilyInstance.Select( familyInstance => familyInstance.get_Geometry(options))
                                              
            //                                               .OfType<GeometryInstance>()
            //                                               .SelectMany(g => g.GetInstanceGeometry())
            //                                               .OfType<Solid>()
            //                                               .Where(s => s.Volume > 0)
            //                                               .ToList();




            Solid unionSolid = solids.Aggregate((x,y) => BooleanOperationsUtils.ExecuteBooleanOperation(x,y,BooleanOperationsType.Union));

            TaskDialog.Show("Message", solids.Count.ToString());




            try
            {
                //List<Element> selectedElements = uiDocument.PickElements(e => e is FamilyInstance, PickElementsOptionFactory.CreateCurrentDocumentOption());
                //ICollection<ElementId> elIds = selectedElements.Select(e => e.Id).ToList();




                using (Transaction t = new Transaction(document, "Create Points"))
                {
                    t.Start();

                    unionSolid.Visualize(document);
                    foreach (Solid solid in solids)
                    {
                        //solid.Visualize(document);   
                    }
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
