using Newtonsoft.Json.Linq;
using OpenTraceability.Models.Events;
using OpenTraceability.Utility.Attributes;
using System.Xml.Serialization;

namespace OpenTraceability.Models.SeaFresh
{
    [XmlRoot("ilmd", Namespace = "http://ns.ftrace.com/epcis")]
    public class FTraceEventILMD : EventILMD
    {
        [OpenTraceability("http://ns.ftrace.com/epcis", "bestBeforeDate")]
        [XmlElement("bestBeforeDate", Namespace = "http://ns.ftrace.com/epcis")]
        public string BestBeforeDate { get; set; }

        [OpenTraceability("http://ns.ftrace.com/epcis", "storageStateCode")]
        [XmlElement("storageStateCode", Namespace = "http://ns.ftrace.com/epcis")]
        public string StorageStateCode { get; set; }

        [OpenTraceability("http://ns.fish.ftrace.com", "unloadingPort")]
        [XmlElement("unloadingPort", Namespace = "http://ns.fish.ftrace.com")]
        public string UnloadingPort { get; set; }

        [OpenTraceability("http://ns.fish.ftrace.com", "catchingPeriodEnd")]
        [XmlElement("catchingPeriodEnd", Namespace = "http://ns.fish.ftrace.com")]
        public string CatchingPeriodEnd { get; set; }

        [OpenTraceabilityObject]
        [OpenTraceability("http://ns.fish.ftrace.com", "vesselCatchInformation")]
        [XmlElement("vesselCatchInformation", Namespace = "http://ns.fish.ftrace.com")]
        public VesselCatchInformation VesselCatchInfo { get; set; } = new VesselCatchInformation();
    }
}
