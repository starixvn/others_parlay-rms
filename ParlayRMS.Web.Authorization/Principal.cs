using System.Security.Principal;

namespace ParlayRMS.Web.Authorization
{
    public class Principal : IPrincipal
    {
        private readonly IIdentity _identity;
        private readonly AuthInfo _authInfo;

        public Principal(IIdentity identity, AuthInfo authInfo)
        {
            _identity = identity;
            _authInfo = authInfo;
        }

        public AuthInfo AuthInfo => _authInfo;

        public IIdentity Identity => _identity;

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
