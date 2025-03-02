using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{
    /// <summary>
    /// This class is implementing the IPickElementsOption interface to pick elements from the current document
    /// </summary>
    public class CurrentDocumentOption : IPickElementsOption
    {
        public List<Element> PickElements(UIDocument uiDocument, Func<Element, bool> validateElement)
        {
            IList<Reference> references = uiDocument.Selection.PickObjects(ObjectType.Element, SelectionFilterFactory.CreateElementSelectionFilter(validateElement));
            List<Element> elements = references.Select(r => uiDocument.Document.GetElement(r.ElementId)).ToList();

            return elements;

        }
        
    }
}