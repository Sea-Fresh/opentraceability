﻿using OpenTraceability.Models.Events;
using OpenTraceability.Models.Identifiers;
using OpenTraceability.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTraceability.GDST.Events
{
    public class GDSTReceiveEvent : ObjectEvent<GDSTILMD>, IGDSTILMDEvent
    {
        [OpenTraceability(Constants.GDST_NAMESPACE, "productOwner")]
        [OpenTraceabilityJson("gdst:productOwner")]
        public PGLN? ProductOwner { get; set; }

        [OpenTraceability(Constants.GDST_NAMESPACE, "humanWelfarePolicy")]
        [OpenTraceabilityJson("gdst:humanWelfarePolicy")]
        public string? HumanWelfarePolicy { get; set; }

        public GDSTShippingEvent()
        {
            this.BusinessStep = new Uri("urn:epcglobal:cbv:bizstep:receiving");
            this.Action = EventAction.OBSERVE;
        }
    }
}
