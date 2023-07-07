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
    public class GDSTDisaggregationEvent : AggregationEvent<GDSTILMD>, IGDSTEvent
    {
        [OpenTraceability(Constants.GDST_NAMESPACE, "productOwner")]
        [OpenTraceabilityJson("gdst:productOwner")]
        public PGLN? ProductOwner { get; set; }

        public GDSTDisaggregationEvent()
        {
            this.BusinessStep = new Uri("urn:epcglobal:cbv:bizstep:unpacking");
            this.Action = EventAction.DELETE;
        }
    }
}
