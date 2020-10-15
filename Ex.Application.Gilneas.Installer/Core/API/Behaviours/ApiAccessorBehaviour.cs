using Ex.Application.Gilneas.Installer.Core.API.Behaviours.Utilities;

namespace Ex.Application.Gilneas.Installer.Core.API.Behaviours
{
    public class ApiAccessorBehaviour : ExBehaviour
    {
        private ApiCoreObject m_ApiCoreObject;

        public override void Awake()
        {
            m_ApiCoreObject = Unbox<ApiCoreObject>( ParentObject );
        }

        public Builder<T> Builder<T>()
            => new Builder<T>();

        public bool Empty( string response )
            => string.IsNullOrEmpty( response ) && string.IsNullOrWhiteSpace( response );

        public ApiCoreObject Parent()
            => m_ApiCoreObject;
    }
}
