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
    public class FilettingILMD : BaseFTraceILMD
    {
        [OpenTraceability("http://ns.ftrace.com/epcis", "useByDate")]
        [XmlElement("useByDate", Namespace = "http://ns.ftrace.com/epcis")]
        public DateTime UseByDate { get; set; }

        [OpenTraceability("http://ns.ftrace.com/epcis", "bestBeforeDate")]
        [XmlElement("bestBeforeDate", Namespace = "http://ns.ftrace.com/epcis")]
        public DateTime BestBeforeDate { get; set; }

        [OpenTraceability("http://ns.ftrace.com/epcis", "dateOfFirstFreezing")]
        [XmlElement("dateOfFirstFreezing", Namespace = "http://ns.ftrace.com/epcis")]
        public DateTime DateOfFirstFreezing { get; set; }

        [OpenTraceability("http://ns.ftrace.com/epcis", "storageStateCode")]
        [XmlElement("storageStateCode", Namespace = "http://ns.ftrace.com/epcis")]
        public string StorageStateCode { get; set; }

        public override void ApplyNamespacePrefixes(XElement ilmdElement, XNamespace fTNS, XNamespace fTFishNS)
        {
            foreach (var element in ilmdElement.Elements())
            {
                if (element.Name.LocalName == "useByDate" || element.Name.LocalName == "bestBeforeDate" || element.Name.LocalName == "dateOfFirstFreezing" || element.Name.LocalName == "storageStateCode")
                {
                    element.Name = fTNS + element.Name.LocalName;
                }
            }
        }
    }
}
