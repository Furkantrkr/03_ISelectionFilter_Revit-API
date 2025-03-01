using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{
    // BASE ABSTRACT CLASS
    public abstract class BaseSelectionFilter : ISelectionFilter
    {
        protected readonly Func<Element, bool> ValidateElement;
        protected readonly Func<Reference, bool> ValidateReference;

        protected BaseSelectionFilter(Func<Element, bool> validateElement)
        {
            ValidateElement = validateElement;
        }

        protected BaseSelectionFilter(Func<Element, bool> validateElement, Func<Reference, bool> validateReference)
            : this(validateElement)
        {
            ValidateReference = validateReference;
        }
        public abstract bool AllowElement(Autodesk.Revit.DB.Element elem);

        public abstract bool AllowReference(Autodesk.Revit.DB.Reference reference, Autodesk.Revit.DB.XYZ position);
    }
    public class ElementSelectionFilter : BaseSelectionFilter

    {
        public ElementSelectionFilter(Func<Element, bool> validateElement) : base(validateElement)
        {
        }
        
        public ElementSelectionFilter(Func<Element, bool> validateElement, Func<Reference,bool> validateReference) : base(validateElement)
        {
        }

        public override bool AllowElement(Element elem)
        {
            return ValidateElement?.Invoke(elem) ?? false;
        }

        public override bool AllowReference(Reference reference, XYZ position)
        {
            return ValidateReference?.Invoke(reference) ?? true;
        }
    }

    public class LinkableSelectionFilter : BaseSelectionFilter
    {
        private readonly Document _doc;
        public LinkableSelectionFilter(Document doc, Func<Element, bool> validateElement) : base(validateElement)
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
