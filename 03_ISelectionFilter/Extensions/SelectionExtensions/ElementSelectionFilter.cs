using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{
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

    public interface IPickElementsOption
    {
        List<Element> PickElements(
            UIDocument uiDocument,
            Func<Element, bool> validateElement);
    }

    public static class PickElementsOptionFactory
    {
        public static CurrentDocumentOption CreateCurrentDocumentOption() => new CurrentDocumentOption();

        public static LinkDocumentOption CreateLinkDocumentOption() => new LinkDocumentOption();

        public static BothDocumentOption CreateBothDocumentOption() => new BothDocumentOption();
    }

    public class LinkDocumentOption : IPickElementsOption
    {
        public List<Element> PickElements(UIDocument uiDocument, Func<Element, bool> validateElement)
        {
            Document doc = uiDocument.Document;
            IList<Reference> references = uiDocument.Selection.PickObjects(
                ObjectType.LinkedElement,
                SelectionFilterFactory.CreateLinkableSelectionFilter(doc, validateElement));

            List<Element> elements = new List<Element>();

            foreach(Reference reference in references)
            {
                Element linkEl = doc.GetElement(reference.ElementId);
                if (linkEl is RevitLinkInstance)
                {
                    RevitLinkInstance linkInstance = linkEl as RevitLinkInstance;
                    Element el = linkInstance.GetLinkDocument().GetElement(reference.LinkedElementId);

                    elements.Add(el);
                }
            }

            return elements;
        }
    }
    public class CurrentDocumentOption : IPickElementsOption
    {
        public List<Element> PickElements(UIDocument uiDocument, Func<Element, bool> validateElement)
        {
            IList<Reference> references = uiDocument.Selection.PickObjects(ObjectType.Element, SelectionFilterFactory.CreateElementSelectionFilter(validateElement));
            List<Element> elements = references.Select(r => uiDocument.Document.GetElement(r.ElementId)).ToList();

            return elements;

        }
        
    }

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