using System;

namespace BoxToLocal.Infrastructure
{
    public class BoxServiceConfig
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string EnterpriseID { get; set; }
        public string PublicKeyID { get; set; }
        public string PrivateKey { get; set; }
        public string Passphrase { get; set; }
    }
}
