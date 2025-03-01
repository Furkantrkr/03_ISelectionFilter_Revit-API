using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{
    public class ElementSelectionFilter : ISelectionFilter
    {
        public readonly Func<Element,bool> _validateElement;
    
        public ElementSelectionFilter(Func<Element, bool> validateElement)
        {
            _validateElement = validateElement;
        }

        public bool AllowElement(Autodesk.Revit.DB.Element elem)
        {
            if(elem != null)
            {
                return _validateElement(elem);
            }
            return false;
        }
        public bool AllowReference(Autodesk.Revit.DB.Reference reference, Autodesk.Revit.DB.XYZ position)
        {
            return false;
        }
    }
}
