using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Rpa.Contracts;

namespace Rpa.Handlers
{
    public sealed class XmlHandler : IXmlHandler
    {
        #region Methods - Public - IXmlHandler

        public XmlDocument CreateXmlFromString(string xmlString)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(xmlString);
            return soapEnvelopeDocument;
        }

        public XDocument Parse(string xmlString)
        {
            XDocument xDoc = XDocument.Parse(xmlString);
            return xDoc;
        }
        public string RemoveInvalidXmlChars(string text)
        {
            var validXmlChars = text.Where(XmlConvert.IsXmlChar).ToArray();
            return new string(validXmlChars);
        }

        public bool IsValidXmlString(string text)
        {
            try
            {
                XmlConvert.VerifyXmlChars(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Implemented based on interface, not part of algorithm
        public string RemoveAllNamespaces(string xmlDocumentString)
        {
            //Removes all namespaces from an XML formatted string and builds clean XMLDocument
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocumentString));
            return xmlDocumentWithoutNs.ToString();
        }

        public XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName) { Value = xmlDocument.Value };
                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);
                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(RemoveAllNamespaces));
        }

        #endregion
    }
}