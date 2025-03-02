using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{
    /// <summary>
    /// This class contains extension methods for the UIDocument class
    /// </summary>
    public static class SelectionExtensions
    {
        public static List<Element> PickElements(
            this UIDocument uiDocument,
            Func<Element, bool> validateElement
            , IPickElementsOption pickElementsOption)
        {
            return pickElementsOption.PickElements(uiDocument, validateElement);
        }
    }
}