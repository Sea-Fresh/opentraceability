using OpenTraceability.Models.Events;
using OpenTraceability.Models.SeaFresh.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace OpenTraceability.Models.SeaFresh
{
    public abstract class BaseFTraceILMD : EventILMD, IFTraceILMD
    {
        public abstract void ApplyNamespacePrefixes(XElement ilmdElement, XNamespace fTNS, XNamespace fTFishNS);
    }
}
