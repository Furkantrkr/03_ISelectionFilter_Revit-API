using System;
using Autodesk.Revit.DB;

namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{
    /// <summary>
    /// This class is used for implementing a selection filter that can be used to select elements in a linked document
    /// </summary>
    public class LinkableSelectionFilter : BaseSelectionFilter
        {
            private readonly Document _doc;
            public LinkableSelectionFilter(Document doc, Func<Element, bool> validateElement)
                : base(validateElement)
            {
                _doc = doc;
            }


            public override bool AllowElement(Element elem)
            {
                return true;
            }

            public override bool AllowReference(Reference reference, XYZ position)
            {
                Element element = _doc.GetElement(reference.ElementId);

                if (!(element is RevitLinkInstance))
                {
                    return ValidateElement(_doc.GetElement(reference.ElementId));
                }

                RevitLinkInstance linkInstance = element as RevitLinkInstance;

                Element el = linkInstance.GetLinkDocument().GetElement(reference.LinkedElementId);
                return ValidateElement(el);

            }
        }
}