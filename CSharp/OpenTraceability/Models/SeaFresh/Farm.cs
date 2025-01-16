using OpenTraceability.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OpenTraceability.Models.SeaFresh
{
    public class Farm
    {
        [OpenTraceability("http://ns.ftrace.com/epcis", "farmIdentityType")]
        [XmlAttribute("farmIdentType", Namespace = "http://ns.ftrace.com/epcis")]
        public string FarmIdentType { get; set; }

        [XmlText]
        public string FarmID { get; set; }
    }
}
