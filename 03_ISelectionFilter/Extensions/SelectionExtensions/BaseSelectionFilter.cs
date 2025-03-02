using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{
    /// <summary>
    /// This interface is used to implement a selection filter that can be used to select elements
    /// </summary>
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
}