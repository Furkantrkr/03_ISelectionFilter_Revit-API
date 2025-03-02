using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{
    /// <summary>
    /// This class is implementing the IPickElementsOption interface to pick elements from the current and linked documents
    /// </summary>
    public class BothDocumentOption : IPickElementsOption
    {
        public List<Element> PickElements(UIDocument uiDocument, Func<Element, bool> validateElement)
        {
            Document doc = uiDocument.Document;
            IList<Reference> references = uiDocument.Selection.PickObjects(
                ObjectType.PointOnElement,
                SelectionFilterFactory.CreateLinkableSelectionFilter(doc, validateElement));

            List<Element> elements = new List<Element>();

            foreach (Reference reference in references)
            {
                Element linkEl = doc.GetElement(reference.ElementId);

                if (linkEl is RevitLinkInstance)
                {
                    RevitLinkInstance linkInstance = linkEl as RevitLinkInstance;
                    Element el = linkInstance.GetLinkDocument().GetElement(reference.LinkedElementId);

                    elements.Add(el);
                    
                } else
                {
                    elements.Add(linkEl);
                }
            }
            return elements;
        }
    }
}