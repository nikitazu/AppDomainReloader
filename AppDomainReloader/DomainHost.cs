using System;

namespace AppDomainReloader
{
    public class DomainHost : IDisposable
    {
        private string _friendlyName;
        private AppDomain _domain;

        public bool IsDomainLoaded
        {
            get { return _domain != null; }
        }

        public DomainHost(string friendlyName)
        {
            _friendlyName = friendlyName;
            LoadDomain();
        }

        public void ReloadDomain()
        {
            UnloadDomain();
            LoadDomain();
        }

        public void LoadDomain()
        {
            if (!IsDomainLoaded)
            {
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = "";
                setup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

                _domain = AppDomain.CreateDomain(_friendlyName, null, setup);
            }
        }

        public void UnloadDomain()
        {
            if (IsDomainLoaded)
            {
                AppDomain.Unload(_domain);
                _domain = null;
            }
        }

        public IEntryPoint CreateEntryPoint(string assemblyFileName, string typeFullName)
        {
            return (IEntryPoint)_domain.CreateInstanceAndUnwrap(assemblyFileName, typeFullName);
        }

        public dynamic CreateDynamicEntryPoint(string assemblyFileName, string typeFullName)
        {
            return _domain.CreateInstanceAndUnwrap(assemblyFileName, typeFullName);
        }

        public void Dispose()
        {
            UnloadDomain();
        }
    }
}
