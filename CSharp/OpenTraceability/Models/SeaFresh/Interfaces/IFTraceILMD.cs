using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace OpenTraceability.Models.SeaFresh.Interfaces
{
    public interface IFTraceILMD
    {
        /// <summary>
        /// Applies FTrace-specific namespace prefixes to the ILMD XML elements.
        /// </summary>
        /// <param name="ilmdElement">The ILMD XElement.</param>
        /// <param name="fTNS">The FTrace namespace (http://ns.ftrace.com/epcis).</param>
        /// <param name="fTFishNS">The FTrace fish namespace (http://ns.fish.ftrace.com).</param>
        void ApplyNamespacePrefixes(XElement ilmdElement, XNamespace fTNS, XNamespace fTFishNS);
    }
}
