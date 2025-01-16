using OpenTraceability.Models.Events;
using OpenTraceability.Models.SeaFresh.Interfaces;
using OpenTraceability.Models.SeaFresh;
using OpenTraceability.Utility.Attributes;
using System.Xml.Linq;
using System.Xml.Serialization;

[XmlRoot("ilmd", Namespace = "http://ns.ftrace.com/epcis")]
public class FishingILMD : EventILMD, IFTraceILMD
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

    [OpenTraceability("http://ns.fish.ftrace.com", "unloadingDate")]
    [XmlElement("unloadingDate", Namespace = "http://ns.fish.ftrace.com")]
    public string UnloadingDate { get; set; }

    [OpenTraceabilityObject]
    [OpenTraceability("http://ns.fish.ftrace.com", "vesselCatchInformation")]
    [XmlElement("vesselCatchInformation", Namespace = "http://ns.fish.ftrace.com")]
    public VesselCatchInformation VesselCatchInfo { get; set; }

    public void ApplyNamespacePrefixes(XElement ilmdElement, XNamespace fTNS, XNamespace fTFishNS)
    {
        foreach (var element in ilmdElement.Elements())
        {
            if (element.Name.LocalName == "bestBeforeDate" || element.Name.LocalName == "storageStateCode")
            {
                element.Name = fTNS + element.Name.LocalName;
            }
            else if (element.Name.LocalName == "unloadingPort" || element.Name.LocalName == "catchingPeriodEnd" || element.Name.LocalName == "unloadingDate" || element.Name.LocalName == "vesselCatchInformation")
            {
                element.Name = fTFishNS + element.Name.LocalName;

                if (element.Name.LocalName == "vesselCatchInformation")
                {
                    foreach (var subElement in element.Elements())
                    {
                        subElement.Name = fTFishNS + subElement.Name.LocalName;
                    }
                }
            }
        }
    }
}