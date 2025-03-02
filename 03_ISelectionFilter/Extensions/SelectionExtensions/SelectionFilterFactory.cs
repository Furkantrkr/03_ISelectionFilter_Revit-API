using System;
using Autodesk.Revit.DB;

namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{
    /// <summary>
    /// This class contains the factory method to create selection filters
    /// </summary>
    public static class SelectionFilterFactory
    {
        public static ElementSelectionFilter CreateElementSelectionFilter(
            Func<Element, bool> validateElement)
        {
            return new ElementSelectionFilter(validateElement);
        }

         public static LinkableSelectionFilter CreateLinkableSelectionFilter(
             Document doc, Func<Element, bool> validateElement)
            {
                return new LinkableSelectionFilter(doc, validateElement);
            }
    }
}