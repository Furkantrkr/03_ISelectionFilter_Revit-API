namespace _03_ISelectionFilter.Extensions.SelectionExtensions
{
    /// <summary>
    /// This class is used to create instances of IPickElementsOption
    /// </summary>
    public static class PickElementsOptionFactory
    {
        public static CurrentDocumentOption CreateCurrentDocumentOption() => new CurrentDocumentOption();

        public static LinkDocumentOption CreateLinkDocumentOption() => new LinkDocumentOption();

        public static BothDocumentOption CreateBothDocumentOption() => new BothDocumentOption();
    }
}