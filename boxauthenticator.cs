using System;
using System.Net;
using Box.V2.JWTAuth;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Box.V2;
using Box.V2.Config;
using Box.V2.Auth;
using Box.V2.Models;
using BoxToLocal.Infrastructure;
using BoxToLocal.BusinessLogic.Interfaces;




namespace BoxToLocal.ServiceIntegrations.BoxService
{
    // Return Box auth token
    public class BoxAuthenticator : IBoxAuthenticator
    {
        // Dependency Injection 
        private readonly BoxServiceConfig _boxServiceConfig;
        private readonly ProxyConfig _proxyConfig;

        public BoxAuthenticator(IOptions<BoxServiceConfig> boxServiceConfig, IOptions<ProxyConfig> proxyConfig)
        {
            _boxServiceConfig = boxServiceConfig?.Value ?? throw new ArgumentNullException(nameof(boxServiceConfig));
            _proxyConfig = proxyConfig?.Value;
        }

        // pass box/proxy config return client
        public async Task<BoxClient> GetAdminClientAsync()
        {
            try
            {
                // initialize configBuilder
                var configBuilder = new BoxConfigBuilder(
                        _boxServiceConfig.ClientID,
                        _boxServiceConfig.ClientSecret,
                        _boxServiceConfig.EnterpriseID,
                        _boxServiceConfig.PrivateKey,
                        _boxServiceConfig.Passphrase,
                        _boxServiceConfig.PublicKeyID);

                // Configure proxy if proxy is available 
                if (_proxyConfig != null) //   TODO : ADD VALIDATION FOR PROXY
                {
                    var proxy = new WebProxy
                    {
                        Credentials = new NetworkCredential
                        {
                            Domain  = _proxyConfig.ProxyDomain,
                            UserName = _proxyConfig.ProxyUser,
                            Password = _proxyConfig.ProxyPassword
                        }
                    };

                    // add proxy credentials to builder if exist
                    configBuilder.SetWebProxy(proxy);
                }

                // pass config object to authenticator with proxy if exists
                var config = configBuilder.Build();
                var boxJWT = new BoxJWTAuth(config);
                var adminToken = await boxJWT.AdminTokenAsync();
                return boxJWT.AdminClient(adminToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
