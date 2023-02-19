﻿using OpenTraceability.Interfaces;
using OpenTraceability.Models.Events.KDEs;
using OpenTraceability.Models.Events;
using OpenTraceability.Models.Identifiers;
using OpenTraceability.Utility.Attributes;
using OpenTraceability.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace OpenTraceability.Mappers
{
    public static class OpenTraceabilityJsonLDMapper
    {
        public static JToken? ToJson(string xname, object? value, bool required = false)
        {
            try
            {
                if (value != null)
                {
                    JToken? json = new JObject();
                    JToken jpointer = json;

                    Type t = value.GetType();
                    OTMappingTypeInformation typeInfo = OTMappingTypeInformation.GetXmlTypeInfo(t);
                    foreach (var property in typeInfo.Properties.Where(p => p.Version == null || p.Version == EPCISVersion.V2))
                    {
                        object? obj = property.Property.GetValue(value);
                        if (obj != null)
                        {
                            JToken jvaluepointer = jpointer;
                            string xchildname = property.Name;

                            if (property.IsQuantityList)
                            {
                                List<EventProduct> products = (List<EventProduct>)obj;
                                products = products.Where(p => p.Quantity != null && p.Type == property.ProductType).ToList();
                                if (products.Count > 0)
                                {
                                    JArray xQuantityList = new JArray();
                                    foreach (var product in products)
                                    {
                                        if (product.EPC != null && product.Quantity != null)
                                        {
                                            JObject xQuantity = new JObject();
                                            xQuantity["epcClass"] = product.EPC.ToString();
                                            xQuantity["quantity"] = product.Quantity.Value;
                                            if (product.Quantity.UoM.UNCode != "EA")
                                            {
                                                xQuantity["uom"] = product.Quantity.UoM.UNCode;
                                            }
                                            xQuantityList.Add(xQuantity);
                                        }
                                    }
                                    jvaluepointer[xchildname] = xQuantityList;
                                }
                            }
                            else if (property.IsEPCList)
                            {
                                List<EventProduct> products = (List<EventProduct>)obj;
                                products = products.Where(p => p.Quantity == null && p.Type == property.ProductType).ToList();
                                if (products.Count > 0 || property.Required)
                                {
                                    JArray xEPCList = new JArray();
                                    foreach (var product in products)
                                    {
                                        if (product.EPC != null)
                                        {
                                            xEPCList.Add(product.EPC.ToString());
                                        }
                                    }
                                    jvaluepointer[xchildname] = xEPCList;
                                }
                            }
                            else if (property.IsArray)
                            {
                                IList list = (IList)obj;
                                JArray xlist = new JArray();

                                foreach (var o in list)
                                {
                                    if (property.IsObject)
                                    {
                                        JToken? xchild = ToJson(property.ItemName ?? xchildname, o, property.Required);
                                        if (xchild != null)
                                        {
                                            xlist.Add(xchild);
                                        }
                                    }
                                    else
                                    {
                                        string? objStr = WriteObjectToString(o);
                                        if (!string.IsNullOrWhiteSpace(objStr))
                                        {
                                            xlist.Add(objStr);
                                        }
                                    }
                                }

                                jvaluepointer[xchildname] = xlist;
                            }
                            else if (property.IsObject)
                            {
                                JToken? xchild = ToJson(xchildname, obj, property.Required);
                                if (xchild != null)
                                {
                                    jvaluepointer[xchildname] = xchild;
                                }
                            }
                            else
                            {
                                string? objStr = WriteObjectToString(obj);
                                if (!string.IsNullOrWhiteSpace(objStr))
                                {
                                    jvaluepointer[xchildname] = objStr;
                                }
                            }
                        }
                    }

                    if (typeInfo.ExtensionKDEs != null)
                    {
                        object? obj = typeInfo.ExtensionKDEs.GetValue(value);
                        if (obj != null && obj is IList<IEventKDE>)
                        {
                            IList<IEventKDE>? kdes = obj as IList<IEventKDE>;
                            if (kdes != null)
                            {
                                foreach (var kde in kdes)
                                {
                                    JToken? xchild = kde.GetJson();
                                    if (xchild != null)
                                    {
                                        jpointer[kde.Name] = xchild;
                                    }
                                }
                            }
                        }
                    }

                    if (typeInfo.ExtensionAttributes != null)
                    {
                        object? obj = typeInfo.ExtensionAttributes.GetValue(value);
                        if (obj != null && obj is IList<IEventKDE>)
                        {
                            IList<IEventKDE>? kdes = obj as IList<IEventKDE>;
                            if (kdes != null)
                            {
                                foreach (IEventKDE kde in kdes)
                                {
                                    JToken? xchild = kde.GetJson();
                                    if (xchild != null)
                                    {
                                        jpointer[kde.Name] = xchild;
                                    }
                                }
                            }
                        }
                    }

                    return json;
                }
                //else if (required == true)
                //{
                //    XElement x = new XElement(xname);
                //    return x;
                //}
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception e = new Exception($"Failed to parse json. value={value} and xname={xname}", ex);
                OTLogger.Error(e);
                throw e;
            }
        }

        public static T FromJson<T>(JToken json)
        {
            T o = (T)FromJson(json, typeof(T));
            return o;
        }

        public static object FromJson(JToken json, Type type)
        {
            object value = Activator.CreateInstance(type) ?? throw new Exception("Failed to create instance of type " + type.FullName);

            try
            {
                OTMappingTypeInformation typeInfo = OTMappingTypeInformation.GetXmlTypeInfo(type);

                List<IEventKDE>? extensionKDEs = null;
                List<IEventKDE>? extensionAttributes = null;

                if (typeInfo.ExtensionAttributes != null)
                {
                    extensionAttributes = new List<IEventKDE>();
                }

                if (typeInfo.ExtensionKDEs != null)
                {
                    extensionKDEs = new List<IEventKDE>();
                }

                OTMappingTypeInformationProperty? mappingProp;

                JObject? jobj = json as JObject;
                if (jobj != null)
                {
                    foreach (JProperty jprop in jobj.Properties())
                    {
                        mappingProp = typeInfo[jprop.Name];
                        JToken? jchild = jobj[jprop.Name];
                        if (jchild != null)
                        {
                            if (mappingProp != null)
                            {
                                ReadPropertyMapping(mappingProp, jchild, value);
                            }
                            else if (extensionKDEs != null)
                            {
                                IEventKDE kde = ReadKDE(jprop.Name, jchild);
                                extensionKDEs.Add(kde);
                            }
                        }
                    }
                }

                if (typeInfo.ExtensionAttributes != null)
                {
                    typeInfo.ExtensionAttributes.SetValue(value, extensionAttributes);
                }

                if (typeInfo.ExtensionKDEs != null)
                {
                    typeInfo.ExtensionKDEs.SetValue(value, extensionKDEs);
                }
            }
            catch (Exception ex)
            {
                OTLogger.Error(ex);
                throw;
            }

            return value;
        }

        private static string? WriteObjectToString(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            else if (obj is DateTimeOffset)
            {
                DateTimeOffset dt = (DateTimeOffset)obj;
                return dt.ToString("O");
            }
            else if (obj is UOM)
            {
                UOM uom = (UOM)obj;
                return uom.UNCode;
            }
            else if (obj is bool)
            {
                bool b = (bool)obj;
                return b.ToString()?.ToLower() ?? string.Empty;
            }
            else if (obj is Country)
            {
                Country b = (Country)obj;
                return b.Abbreviation;
            }
            else if (obj is TimeSpan)
            {
                TimeSpan timespan = (TimeSpan)obj;
                if (timespan.Ticks < 0)
                {
                    return "-" + timespan.Negate().TotalHours.ToString("#00") + ":" + timespan.Minutes.ToString("00");
                }
                else
                {
                    return "+" + timespan.TotalHours.ToString("#00") + ":" + timespan.Minutes.ToString("00");
                }
            }
            else
            {
                return obj.ToString() ?? string.Empty;
            }
        }

        private static void ReadPropertyMapping(OTMappingTypeInformationProperty mappingProp, JToken json, object value)
        {
            if (mappingProp.IsQuantityList)
            {
                IEvent e = (IEvent)value;
                JArray? jQuantityList = json as JArray;
                if (jQuantityList != null)
                {
                    foreach (JObject jQuantity in jQuantityList)
                    {
                        EventProduct product = new EventProduct();
                        product.Type = mappingProp.ProductType;
                        product.EPC = new EPC(jQuantity["epcClass"]?.Value<string>() ?? string.Empty);

                        double quantity = jQuantity.Value<double>("quantity");
                        string uom = jQuantity.Value<string>("uom") ?? "EA";
                        product.Quantity = new Measurement(quantity, uom);

                        e.AddProduct(product);
                    }
                }
            }
            else if (mappingProp.IsEPCList)
            {
                IEvent e = (IEvent)value;
                JArray? jEPCList = json as JArray;
                if (jEPCList != null)
                {
                    foreach (JToken jEPC in jEPCList)
                    {
                        EventProduct product = new EventProduct();
                        product.Type = mappingProp.ProductType;
                        product.EPC = new EPC(jEPC.ToString());
                        e.AddProduct(product);
                    }
                }
            }
            else if (mappingProp.IsArray)
            {
                IList? list = mappingProp.Property.GetValue(value) as IList;
                if (list == null)
                {
                    list = (IList)(Activator.CreateInstance(mappingProp.Property.PropertyType)
                        ?? throw new Exception("Failed to create instance of " + mappingProp.Property.PropertyType.FullName));

                    mappingProp.Property.SetValue(value, list);
                }

                Type itemType = list.GetType().GenericTypeArguments[0];

                JArray? jArr = json as JArray;
                if (jArr != null)
                {
                    foreach (JToken j in jArr)
                    {
                        if (mappingProp.IsObject)
                        {
                            object o = FromJson(j, itemType);
                            list.Add(o);
                        }
                        else
                        {
                            object o = ReadObjectFromString(j.ToString(), itemType);
                            list.Add(o);
                        }
                    }
                }
            }
            else if (mappingProp.IsObject)
            {
                object o = FromJson(json, mappingProp.Property.PropertyType);
                mappingProp.Property.SetValue(value, o);
            }
            else
            {
                string v = json.ToString();
                if (!string.IsNullOrWhiteSpace(v))
                {
                    object o = ReadObjectFromString(v, mappingProp.Property.PropertyType);
                    mappingProp.Property.SetValue(value, o);
                }
            }
        }

        private static object ReadObjectFromString(string value, Type t)
        {
            try
            {
                if (t == typeof(DateTimeOffset) || t == typeof(DateTimeOffset?))
                {
                    DateTimeOffset dt = value.TryConvertToDateTimeOffset() ?? throw new Exception("Failed to convert string to datetimeoffset where value = " + value);
                    return dt;
                }
                else if (t == typeof(UOM))
                {
                    UOM uom = UOM.LookUpFromUNCode(value);
                    return uom;
                }
                else if (t == typeof(bool) || t == typeof(bool?))
                {
                    bool v = bool.Parse(value);
                    return v;
                }
                else if (t == typeof(double) || t == typeof(double?))
                {
                    double v = double.Parse(value);
                    return v;
                }
                else if (t == typeof(Uri))
                {
                    Uri v = new Uri(value);
                    return v;
                }
                else if (t == typeof(TimeSpan) || t == typeof(TimeSpan?))
                {
                    if (value.StartsWith("+")) value = value.Substring(1);
                    TimeSpan ts = TimeSpan.Parse(value);
                    return ts;
                }
                else if (t == typeof(EventAction) || t == typeof(EventAction?))
                {
                    EventAction action = Enum.Parse<EventAction>(value);
                    return action;
                }
                else if (t == typeof(PGLN))
                {
                    PGLN pgln = new PGLN(value);
                    return pgln;
                }
                else if (t == typeof(GLN))
                {
                    GLN gln = new GLN(value);
                    return gln;
                }
                else if (t == typeof(EPC))
                {
                    EPC epc = new EPC(value);
                    return epc;
                }
                else if (t == typeof(Country))
                {
                    Country c = Countries.Parse(value);
                    return c;
                }
                else
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                Exception e = new Exception($"Failed to convert string into object. value={value} and t={t}", ex);
                OTLogger.Error(e);
                throw e;
            }
        }

        private static IEventKDE ReadKDE(string name, JToken json)
        {
            IEventKDE? kde = null;

            //if not, check if it is a simple value or an object
            if (kde == null)
            {
                if (json is JObject || json is JArray)
                {
                    kde = new EventKDEObject(string.Empty, name);
                }
                //else if simple value, then we will consume it as a string
                else
                {
                    kde = new EventKDEString(string.Empty, name);
                }
            }

            if (kde != null)
            {
                kde.SetFromJson(json);
            }
            else
            {
                throw new Exception("Failed to initialize KDE from JSON = " + json.ToString());
            }

            return kde;
        }
    }
}
