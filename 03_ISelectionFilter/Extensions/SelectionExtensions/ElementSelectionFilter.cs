using System;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{

    /// <summary>
    /// This class is for implementing a selection filter that can be used to select elements in the current document
    /// </summary>
    public class ElementSelectionFilter : BaseSelectionFilter
        {
            private readonly Func<Reference, bool> _validateReference;
            public ElementSelectionFilter(Func<Element, bool> validateElement)
                : base(validateElement)
            {
            }

            public ElementSelectionFilter(
                Func<Element, bool> validateElement,
                Func<Reference, bool> validateReference)
                : base(validateElement)
            {
                _validateReference = validateReference;
            }

            public override bool AllowElement(Element elem)
            {
                return ValidateElement?.Invoke(elem) ?? true;
            }

            public override bool AllowReference(Reference reference, XYZ position)
            {
                return _validateReference?.Invoke(reference) ?? true;
            }
        }
}