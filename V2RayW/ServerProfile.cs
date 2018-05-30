using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace V2RayW
{
    public enum KcpHeaderType
    {
        none = 0,
        srtp = 1,
        utp = 2,
        wechat_video = 3,
        dtls = 4
    }
    public enum SecurityType
    {
        aes_128_cfb = 0,
        aes_128_gcm = 1,
        chacha20_poly130 = 2,
        auto = 3,
        none = 4
    }

    public enum NetWorkType
    {
        tcp = 0,
        kcp = 1,
        ws = 2,
        http2 = 3
    }

    public class ServerProfile
    {
        public ServerProfile DeepCopy()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<ServerProfile>(js.Serialize(this));
        }

        public string address = "server.cc";
        public int port = 10086;
        public string userId = "00000000-0000-0000-0000-000000000000";
        public int alterId = 64;
        public int level = 0;
        public string remark = "test";
        public SecurityType security = SecurityType.aes_128_cfb;
        public NetWorkType network = NetWorkType.tcp;
        public string sendThrough = "0.0.0.0";
        public Dictionary<string, object> streamSettings = new Dictionary<string, object>
            {
                { "security", "none" },
                { "tlsSettings", new Dictionary<string, object> {
                    { "serverName", "server.cc" },
                    { "allowInsecure", false },
                    } },
                { "tcpSettings", new Dictionary<string, object> {
                    { "header", new Dictionary<string, object> {
                        { "type", "none" }
                    } } } },
                { "kcpSettings",  new Dictionary<string, object> {
                    { "mtu", 1350 },
                    { "tti", 20 },
                    { "uplinkCapacity", 5 },
                    { "downlinkCapacity", 20 },
                    { "congestion", false },
                    { "readBufferSize", 1 },
                    { "writeBufferSize", 1 },
                    { "header", new Dictionary<string, object> {
                        { "type", "none" }
                    } } } },
                { "wsSettings", new Dictionary<string, object> {
                    { "path", "" },
                    { "headers", new Dictionary<string, object> {
                        { "Host", "server.cc" }
                    } } } },
                { "httpSettings", new Dictionary<string, object> {
                    { "host", new string[] {"" } },
                    { "path", "" }
                } } };
        //public string proxyOutboundJson = "";
        public Dictionary<string, object> muxSettings = new Dictionary<string, object>
        {
            {"enabled", false },
            {"concurrency", 8 }
        };


        public Dictionary<string, object> OutboundProfile()
        {
            streamSettings["network"] = this.network.ToString();
            Dictionary<string, object> result = new Dictionary<string, object>
            {
                { "sendThrough", this.sendThrough },
                {"protocol", "vmess" },
                {"settings", new Dictionary<string, object>()
                {
                    { "vnext", new Dictionary<string, object>[]
                        {
                            new Dictionary<string, object>()
                            {
                                { "remark", remark },
                                { "address", address } ,
                                { "port", port },
                                { "users", new Dictionary<string, object>[]
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "id", userId },
                                        { "alterId", alterId },
                                        { "security", security.ToString().Replace('_', '-') }//,
                                        //{ "level", level }
                                    }
                                } } } } } } },

                { "streamSettings", streamSettings },
                /*
                { "proxySettings", new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>()
                    {
                        {"tag", "transit" },
                        {"outbound-proxy-config", proxyOutboundJson }
                    }
                } },*/
                {"mux", muxSettings }
            };
            return result;
        }
    }
}
