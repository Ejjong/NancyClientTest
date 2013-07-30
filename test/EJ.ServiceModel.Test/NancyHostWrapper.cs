using System;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;

namespace EJ.ServiceModel.Test
{
    public class NancyHostWrapper : IDisposable
    {
        private readonly NancyHost host;

        public NancyHostWrapper(NancyHost host)
        {
            this.host = host;
        }

        public void Dispose()
        {
            host.Stop();
        }
    }

    public class NancyHostUtil
    {
        private static readonly Uri BaseUri = new Uri("http://localhost:1234/base/");
        
        public static NancyHostWrapper CreateAndOpenSelfHost(INancyBootstrapper nancyBootstrapper = null, HostConfiguration configuration = null)
        {
            if (nancyBootstrapper == null)
            {
                nancyBootstrapper = new DefaultNancyBootstrapper();
            }

            var host = new NancyHost(nancyBootstrapper, configuration, BaseUri);

            try
            {
                host.Start();
            }
            catch
            {
                throw new Exception();
            }

            return new NancyHostWrapper(host);
        }
    }


}
