using System;

namespace AppDomainReloader
{
    public class DomainHost : IDisposable
    {
        private string _friendlyName;
        private AppDomain _domain;
        
        public DomainHost(string friendlyName)
        {
            _friendlyName = friendlyName;
            StartDomain();
        }

        public void RestartDomain()
        {
            StopDomain();
            StartDomain();
        }

        public void StartDomain()
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = "";
            setup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            _domain = AppDomain.CreateDomain(_friendlyName, null, setup);
        }

        public void StopDomain()
        {
            AppDomain.Unload(_domain);
            _domain = null;
        }

        public IEntryPoint CreateEntryPoint(string assemblyFileName, string typeFullName)
        {
            return (IEntryPoint)_domain.CreateInstanceAndUnwrap(assemblyFileName, typeFullName);
        }

        public void Dispose()
        {
            StopDomain();
        }
    }
}
