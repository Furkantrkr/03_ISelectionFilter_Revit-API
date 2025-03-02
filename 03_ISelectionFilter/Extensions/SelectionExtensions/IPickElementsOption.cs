using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{
    /// <summary>
    /// This interface defines the method to pick elements
    /// </summary>
    public interface IPickElementsOption
    {
        List<Element> PickElements(
            UIDocument uiDocument,
            Func<Element, bool> validateElement);
    }
}