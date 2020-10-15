using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Headers = System.Collections.Generic.Dictionary<string, string>;

namespace Ex.Application.Gilneas.Installer.Core.API.Behaviours.Utilities
{
    public class Builder<T>
    {
        private List<T> m_Response { get; set; }
        private HttpClient m_HttpClient { get; set; }
        private Headers m_Request { get; set; }

        public Builder()
        {
            m_Response = new List<T>();
            m_HttpClient = new HttpClient();
            m_Request = new Headers();
        }

        public async Task<string> GetAsync( string uri )
        {
            if ( m_Request.Count > 0 )
            {
                foreach ( var header in m_Request )
                    m_HttpClient.DefaultRequestHeaders.Add( header.Key, header.Value );
            }

            return await m_HttpClient.GetAsync( uri ).GetAwaiter().GetResult().Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync( string uri, Headers body )
        {
            var content = new FormUrlEncodedContent( body );

            return await m_HttpClient.PostAsync( uri, content ).GetAwaiter().GetResult().Content.ReadAsStringAsync();
        }

        public Builder<T> Request( string uri, Method method = Method.GET, Dictionary<string, string> body = null )
        {
            if ( uri == "" )
            {
                return this;
            }

            try
            {
                Task<string> awaiter;

                if ( method == Method.GET )
                {
                    awaiter = GetAsync( uri );
                    awaiter.Wait();
                }
                else
                {
                    awaiter = PostAsync( uri, body );
                    awaiter.Wait();
                }

                try
                {
                    if ( m_Request.Count > 0 )
                    {
                        foreach ( var header in m_Request )
                            m_HttpClient.DefaultRequestHeaders.Remove( header.Key );

                        m_Request.Clear();
                    }

                    var response = JsonConvert.DeserializeObject<T[]>( awaiter.GetAwaiter().GetResult() );

                    foreach ( var item in response )
                    {
                        m_Response.Add( item );
                    }
                }
                catch ( Exception )
                {
                    var response = JsonConvert.DeserializeObject<T>( awaiter.GetAwaiter().GetResult() );

                    m_Response.Add( response );
                }
            }
            catch ( Exception ex )
            {
                throw ex;
            }

            return this;
        }

        public Builder<T> Header( string key, string val )
        {
            if ( !m_Request.ContainsKey( key ) )
            {
                m_Request.Add( key, val );
            }

            return this;
        }

        public List<T> Response()
        {
            return m_Response;
        }
        public T First()
        {
            if ( Response().Count > 0 )
                return Response()[ 0 ];

            return ( T ) ( object ) null;
        }

        public Builder<T> Modify( int index, Action<T> method )
        {
            if ( m_Response.Count > index )
            {
                method( m_Response[ index ] );
            }

            return this;
        }
        public Builder<T> ModifyAll( Action<T> method )
        {
            foreach ( var res in m_Response )
            {
                method( res );
            }

            return this;
        }
        public Builder<T> ModifyAllAsync( Action<T> method )
        {
            var task = new Task( () =>
            {
                foreach ( var res in m_Response )
                {
                    method( res );
                }
            } );

            task.Start();
            task.Wait();

            return this;
        }
    }
}
