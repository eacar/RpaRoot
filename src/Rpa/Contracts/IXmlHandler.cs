using System.Xml;
using System.Xml.Linq;

namespace Rpa.Contracts
{
    public interface IXmlHandler
    {
        #region Methods

        XmlDocument CreateXmlFromString(string xmlString);
        XDocument Parse(string xmlString);
        string RemoveAllNamespaces(string xmlDocumentString);
        XElement RemoveAllNamespaces(XElement xmlDocument);
        string RemoveInvalidXmlChars(string text);
        bool IsValidXmlString(string text);

        #endregion
    }
}