﻿using OpenTraceability.GDST.Events;
using OpenTraceability.GDST.MasterData;
using OpenTraceability.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTraceability.GDST
{
    public static class OpenTraceabilityGDST
    {
        public static void Initialize()
        {
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTFishingEvent), EventType.ObjectEvent, "urn:gdst:bizStep:fishingEvent", Models.Events.EventAction.ADD));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTTransshipmentEvent), EventType.ObjectEvent, "urn:gdst:bizStep:transshipment", Models.Events.EventAction.OBSERVE));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTLandingEvent), EventType.ObjectEvent, "urn:gdst:bizstep:landing", Models.Events.EventAction.OBSERVE));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTHatchingEvent), EventType.ObjectEvent, "urn:gdst:bizstep:hatching", Models.Events.EventAction.ADD));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTFarmHarvestEvent), EventType.TransformationEvent, "urn:gdst:bizstep:farmharvest"));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTProcessingEvent), EventType.TransformationEvent, "urn:epcglobal:cbv:bizstep:commissioning"));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTProcessingEvent), EventType.TransformationEvent, "https://ref.gs1.org/cbv/BizStep-commissioning"));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTShippingEvent), EventType.ObjectEvent, "urn:epcglobal:cbv:bizstep:receiving", Models.Events.EventAction.OBSERVE));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTShippingEvent), EventType.ObjectEvent, "https://ref.gs1.org/cbv/BizStep-receiving", Models.Events.EventAction.OBSERVE));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTReceiveEvent), EventType.ObjectEvent, "urn:epcglobal:cbv:bizstep:shipping", Models.Events.EventAction.OBSERVE));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTReceiveEvent), EventType.ObjectEvent, "https://ref.gs1.org/cbv/BizStep-shipping", Models.Events.EventAction.OBSERVE));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTAggregationEvent), EventType.AggregationEvent, "urn:epcglobal:cbv:bizstep:packing", Models.Events.EventAction.ADD));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTAggregationEvent), EventType.AggregationEvent, "https://ref.gs1.org/cbv/BizStep-packing", Models.Events.EventAction.ADD));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTDisaggregationEvent), EventType.AggregationEvent, "urn:epcglobal:cbv:bizstep:unpacking", Models.Events.EventAction.DELETE));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTDisaggregationEvent), EventType.AggregationEvent, "https://ref.gs1.org/cbv/BizStep-unpacking", Models.Events.EventAction.DELETE));

            // The feedmill event is special in the sense that it requires a KDE profile to detect.
            // We know it is a feedmill event when it has the proteinSource KDE in the ILMD.
            List<OpenTraceabilityEventKDEProfile> feedmillKDEProfile = new List<OpenTraceabilityEventKDEProfile>() {
                new OpenTraceabilityEventKDEProfile("extension/ilmd/" + (Constants.GDST_XNAMESPACE + "proteinSource"), "ilmd/" + (Constants.GDST_XNAMESPACE + "proteinSource"), "ilmd.gdst:proteinSource")
            };

            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTFeedmillTransformationEvent), EventType.TransformationEvent, "urn:epcglobal:cbv:bizstep:commissioning", feedmillKDEProfile));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTFeedmillTransformationEvent), EventType.TransformationEvent, "https://ref.gs1.org/cbv/BizStep-commissioning", feedmillKDEProfile));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTFeedmillObjectEvent), EventType.ObjectEvent, "urn:epcglobal:cbv:bizstep:commissioning", Models.Events.EventAction.ADD, feedmillKDEProfile));
            OpenTraceabilityInitializer.RegisterEventProfile(new OpenTraceabilityEventProfile(typeof(GDSTFeedmillObjectEvent), EventType.ObjectEvent, "https://ref.gs1.org/cbv/BizStep-commissioning", Models.Events.EventAction.ADD, feedmillKDEProfile));

            OpenTraceabilityInitializer.RegisterMasterDataType<GDSTLocation>();
        }
    }
}
