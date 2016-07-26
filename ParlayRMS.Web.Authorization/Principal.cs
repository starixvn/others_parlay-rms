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

        public AuthInfo AuthInfo
        {
            get { return _authInfo; }
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <param name="role">The name of the role for which to check membership.</param>
        /// <returns>True if the current principal is a member of the specified role; otherwise, false.</returns>
        public bool IsInRole(string role)
        {
            return true;
        }
    }
}