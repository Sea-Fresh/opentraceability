using OpenTraceability.Models.Events;
using OpenTraceability.Models.SeaFresh.Interfaces;
using OpenTraceability.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OpenTraceability.Models.SeaFresh
{
    [XmlRoot("ilmd", Namespace = "http://ns.ftrace.com/epcis")]
    public class FarmingILMD : EventILMD, IFTraceILMD
    {
        [OpenTraceability("http://ns.ftrace.com/epcis", "countryOfOrigin")]
        [XmlElement("countryOfOrigin", Namespace = "http://ns.ftrace.com/epcis")]
        public string CountryOfOrigin { get; set; }

        [OpenTraceability("http://ns.ftrace.com/epcis", "catchingPeriodEnd")]
        [XmlElement("catchingPeriodEnd", Namespace = "http://ns.ftrace.com/epcis")]
        public DateTime CatchingPeriodEnd { get; set; }

        [OpenTraceability("http://ns.ftrace.com/epcis", "bestBeforeDate")]
        [XmlElement("bestBeforeDate", Namespace = "http://ns.ftrace.com/epcis")]
        public DateTime BestBeforeDate { get; set; }

        [OpenTraceability("http://ns.ftrace.com/epcis", "storageStateCode")]
        [XmlElement("storageStateCode", Namespace = "http://ns.ftrace.com/epcis")]
        public string StorageStateCode { get; set; }

        [OpenTraceability("http://ns.ftrace.com/epcis", "listOfFarms")]
        [XmlArray("listOfFarms", Namespace = "http://ns.ftrace.com/epcis")]
        [XmlArrayItem("farm", Namespace = "http://ns.ftrace.com/epcis")]
        public List<Farm> ListOfFarms { get; set; } = new List<Farm>();

        public void ApplyNamespacePrefixes(XElement ilmdElement, XNamespace fTNS, XNamespace fTFishNS)
        {
            foreach (var element in ilmdElement.Elements())
            {
                if (element.Name.LocalName == "countryOfOrigin" || element.Name.LocalName == "catchingPeriodEnd" || element.Name.LocalName == "bestBeforeDate" || element.Name.LocalName == "storageStateCode")
                {
                    element.Name = fTNS + element.Name.LocalName;
                }
                else if (element.Name.LocalName == "listOfFarms")
                {
                    foreach (var subElement in element.Elements())
                    {
                        subElement.Name = fTNS + subElement.Name.LocalName;
                    }
                }
            }
        }
    }
}
