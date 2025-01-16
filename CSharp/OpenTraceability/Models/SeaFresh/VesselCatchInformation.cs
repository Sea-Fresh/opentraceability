using OpenTraceability.Utility.Attributes;
using System.Xml.Serialization;

namespace OpenTraceability.Models.SeaFresh
{
    [XmlRoot("vesselCatchInformation", Namespace = "http://ns.fish.ftrace.com")]
    public class VesselCatchInformation
    {
        [OpenTraceability("http://ns.fish.ftrace.com", "vesselID")]
        [XmlElement("vesselID", Namespace = "http://ns.fish.ftrace.com")]
        public string VesselID { get; set; }

        [OpenTraceability("http://ns.fish.ftrace.com", "vesselName")]
        [XmlElement("vesselName", Namespace = "http://ns.fish.ftrace.com")]
        public string VesselName { get; set; }

        [OpenTraceability("http://ns.fish.ftrace.com", "vesselOwner")]
        [XmlElement("vesselOwner", Namespace = "http://ns.fish.ftrace.com")]
        public string VesselOwner { get; set; }

        [OpenTraceability("http://ns.fish.ftrace.com", "vesselFlagState")]
        [XmlElement("vesselFlagState", Namespace = "http://ns.fish.ftrace.com")]
        public string VesselFlagState { get; set; }

        [OpenTraceability("http://ns.fish.ftrace.com", "haul")]
        [XmlElement("haul", Namespace = "http://ns.fish.ftrace.com")]
        public string Haul { get; set; }

        [OpenTraceability("http://ns.ftrace.com/epcis", "catchMethod")]
        [XmlElement("catchMethod", Namespace = "http://ns.ftrace.com/epcis")]
        public string CatchMethod { get; set; }

        [OpenTraceability("http://ns.ftrace.com/epcis", "catchArea")]
        [XmlElement("catchArea", Namespace = "http://ns.ftrace.com/epcis")]
        public string CatchArea { get; set; }
    }
}
