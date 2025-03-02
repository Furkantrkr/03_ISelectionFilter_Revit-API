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

        protected BaseSelectionFilter(Func<Element, bool> validateElement)
        {
            ValidateElement = validateElement;
        }

        public abstract bool AllowElement(Autodesk.Revit.DB.Element elem);

        public abstract bool AllowReference(Autodesk.Revit.DB.Reference reference, Autodesk.Revit.DB.XYZ position);
    }
    public class ElementSelectionFilter : BaseSelectionFilter
    {
        private readonly Func<Reference, bool> _validateReference;
        public ElementSelectionFilter(Func<Element, bool> validateElement) 
            : base(validateElement)
        {
        }
        
        public ElementSelectionFilter(Func<Element, bool> validateElement, Func<Reference,bool> validateReference) : base(validateElement)
        {
            _validateReference = validateReference;
    }

        public override bool AllowElement(Element elem)
        {
            return ValidateElement?.Invoke(elem) ?? false;
        }

        public override bool AllowReference(Reference reference, XYZ position)
        {
            return _validateReference?.Invoke(reference) ?? true;
        }
    }

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
