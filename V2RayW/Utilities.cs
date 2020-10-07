using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Script.Serialization;
using V2RayW.Resources;

namespace V2RayW
{
    public enum ProxyMode { pac, global, manual }

    class Utilities
    {

        public static JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        // BFS
        private static void AddMissingKeysForVmess(Dictionary<string, object> outbound, Dictionary<string, object> template)
        {
            Queue<Dictionary<string, object>> outboundQueue = new Queue<Dictionary<string, object>>();
            outboundQueue.Enqueue(outbound);
            Queue<Dictionary<string, object>> templateQueue = new Queue<Dictionary<string, object>>();
            templateQueue.Enqueue(template);
            while (templateQueue.Count > 0)
            {
                //Debug.WriteLine($"size is {templateQueue.Count}");
                int size = templateQueue.Count;
                for (int i = 0; i < size; i += 1)
                {
                    var templateNode = templateQueue.Dequeue();
                    var outboundNode = outboundQueue.Dequeue();
                    foreach (KeyValuePair<string, object> entry in templateNode)
                    {
                        if (!outboundNode.ContainsKey(entry.Key))
                        {
                            //Debug.WriteLine($"find missing key {entry.Key}");
                            outboundNode.Add(entry.Key, entry.Value);
                        }
                        else
                        {
                            if (entry.Value.GetType().Equals(typeof(Dictionary<string, object>)))
                            {
                                templateQueue.Enqueue(entry.Value as Dictionary<string, object>);
                                outboundQueue.Enqueue(outboundNode[entry.Key] as Dictionary<string, object>);
                            } else if (entry.Value.GetType().Equals(typeof(object[])) && (entry.Value as object[])[0].GetType().Equals(typeof(Dictionary<string, object>)))
                            {
                                //Debug.WriteLine($"find array at key {entry.Key}, call self recursively");
                                var subtemplate = (entry.Value as object[])[0] as Dictionary<string, object>;
                                foreach(Dictionary<string, object> subnode in outboundNode[entry.Key] as object[])
                                {
                                    AddMissingKeysForVmess(subnode, subtemplate);
                                }
                            }
                        }
                    }
                }
            }
        }


        public static void AddMissingKeysForVmess(Dictionary<string, object> outbound)
        {
            var template = Utilities.VmessOutboundTemplate();
            AddMissingKeysForVmess(outbound, template);
        }

        public static bool IsWindows10()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            string productName = (string)reg.GetValue("ProductName");

            return productName.StartsWith("Windows 10");
        }

        public static List<string> DOMAIN_STRATEGY_LIST = new List<string> { @"AsIs", @"IPIfNonMatch", @"IPOnDemand" };

        public static string corePath = AppDomain.CurrentDomain.BaseDirectory + @"v2ray-core\v2ray.exe";

        public static string[] necessaryFiles = new string[] {"v2ray.exe", "v2ctl.exe", "geoip.dat", "geosite.dat" };

        public static string suggestedCore = "V2Ray 4.14.0 (Po) 20190131";
        public static List<string> PROTOCOL_LIST = new List<string> { @"vmess", @"vless" };
        public static List<string> PROTOCOL_LIST_UI = new List<string> { @"VMess", @"VLESS" };
        public static List<string> VMESS_SECURITY_LIST = new List<string> { @"auto", @"aes-128-gcm", @"chacha20-poly1305", @"none" };
        public static List<string> RESERVED_TAGS = new List<string> { @"main", @"direct", @"decline", @"balance" };
        public static List<string> NETWORK_LIST = new List<string> { @"tcp", @"kcp", @"ws", @"http", @"quic" };
        public static List<string> OBFU_LIST = new List<string> { @"none", @"srtp", @"utp", @"wechat-video", @"dtls", @"wireguard" };
        public static List<string> QUIC_SECURITY_LIST = new List<string> { @"none", @"aes-128-gcm", @"chacha20-poly1305" };

        public static List<string> LOG_LEVEL_LIST = new List<string> { "debug", "info", "warning", "error", "none" };

        public static Dictionary<string, object> configTemplate = new Dictionary<string, object>
        {
            {
                "dns",
                new Dictionary<string, object>
                {
                    { "servers", new List<string> { "localhost" } }
                }
            },
            {
                "inbounds", new List<Dictionary<string, object>>()
            },
            {
                "log", new Dictionary<string, object>()
            },
            {
                "outbounds", new List<Dictionary<string, object>>()
            },
            {
                "routing", new Dictionary<string, object>()
            }
        };

        public static Dictionary<string, object> VmessOutboundTemplate()
        {
            return javaScriptSerializer.Deserialize<dynamic>(V2RayW.Properties.Resources.vmessjson);
        }

        public static Dictionary<string, object> VmessOutboundTemplateNew()
        {
            return javaScriptSerializer.Deserialize<dynamic>(V2RayW.Properties.Resources.vmesstemplate);
        }
        public static Dictionary<string, object> ShadowsocksOutboundTemplateNew()
        {
            return javaScriptSerializer.Deserialize<dynamic>(V2RayW.Properties.Resources.shadowsockstemplate);
        }

        public static Dictionary<string, object> outboundTemplate = new Dictionary<string, object>
        {
            {"sendThrough", "0.0.0.0" },
            {"mux", new Dictionary<string, object>() },
            {"protocol", "name" },
            {"settings", new Dictionary<string, object>() },
            {"tag", "" },
            {"streamSettings", new Dictionary<string, object>() }
        };


        #region routing rules
        public static Dictionary<string, object> ROUTING_DIRECT = new Dictionary<string, object> {
            {"name",Strings.rulenamedirect },
            { @"domainStrategy", @"AsIs" },
            { "rules",
                new List<object>
                    {
                  new Dictionary<string, object>
                {
                    { @"type", @"field" },
                    { @"port", @"0-65535" },
                    { @"outboundTag", @"direct"}
                }
                }

            } };

        public static Dictionary<string, object> ROUTING_GLOBAL = new Dictionary<string, object> {
            {"name", Strings.rulenameglobal },
            { @"domainStrategy", @"AsIs" },
            { "rules",
                new List<object>
                    {
                  new Dictionary<string, object>
                {
                    { @"type", @"field" },
                    { @"port", @"0-65535" },
                    { @"outboundTag", @"main"}
                }
                }

            } };

        public static Dictionary<string, object> ROUTING_BYPASSCN_PRIVATE_APPLE = new Dictionary<string, object> {
            { "name", Strings.rulenamebypass },
            { "domainStrategy", "IPIfNonMatch" },
            {
                "rules",
                new List<object>
                {
                    new Dictionary<string, object>
                    {
                        { "type", "field" },
                        { "outboundTag", "direct" },
                        {
                            "domain",
                            new object[]
                            {
                                "localhost",
                                "geosite:cn"
                            }
                        }
                    },
                    new Dictionary<string, object>
                    {
                        { "type", "field" },
                        { "outboundTag", "direct" },
                        {
                            "ip",
                            new object[] {
                                "geoip:private",
                                "geoip:cn"
                            }
                        }
                    },
                    new Dictionary<string, object>
                    {
                        { "type" , "field" },
                        { "outboundTag" , "main" },
                        { "port" , "0-65535" }
                    }
                }
            }
        };

        public static Dictionary<string, object> singleRule = new Dictionary<string, object>
                    {
                        { "type" , "field" },
                        { "outboundTag" , "main" },
                        { "port" , "0-65535" }
                    };
        #endregion

        #region outbounds
        public static Dictionary<string, object> OUTBOUND_DIRECT = new Dictionary<string, object>
        {
            { "protocol", "freedom" },
            { "tag",  "direct" }
        };

        public static Dictionary<string, object> OUTBOUND_DECLINE = new Dictionary<string, object>
        {
            { "protocol", "blackhole" },
            { "tag", "decline" }
        };


        #endregion

        public static List<string> routingNetwork = new List<string> { "tcp", "udp", "tcp,udp" };
    }
}
