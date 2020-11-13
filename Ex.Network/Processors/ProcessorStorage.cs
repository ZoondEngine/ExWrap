using Ex.Network.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ex.Network.Processors
{
    internal class ProcessorStorage
    {
        private readonly List<INetworkProcessor> m_Processors;

        public ProcessorStorage()
            : this( new List<INetworkProcessor>() )
        { }
        public ProcessorStorage( List<INetworkProcessor> processors )
        {
            m_Processors = processors;
        }
        public ProcessorStorage( Assembly containingProcessorsAssembly )
        {
            m_Processors = containingProcessorsAssembly
                .GetTypes()
                .Where( m => m.GetInterfaces().Contains( typeof( INetworkProcessor ) ) )
                .Select( m => m.GetConstructor( Type.EmptyTypes ).Invoke( null ) as INetworkProcessor )
                .ToList();
        }

        public void ParseProcessorsFromAssembly( Assembly containingProcessorsAssembly )
        {
            var parsed = containingProcessorsAssembly
                .GetTypes()
                .Where( m => m.GetInterfaces().Contains( typeof( INetworkProcessor ) ) )
                .Select( m => m.GetConstructor( Type.EmptyTypes ).Invoke( null ) as INetworkProcessor )
                .ToList();

            m_Processors.AddRange( parsed );
        }

        public void AddProcessor<T>() where T : INetworkProcessor, new()
            => AddProcessor( new T() );
        public void AddProcessor( INetworkProcessor processor )
        {
            m_Processors.Add( processor );
        }

        public void RemoveProcessor( INetworkProcessor processor )
            => m_Processors.Remove( processor );

        public BaseNetworkEntity Handle( BaseNetworkEntity request )
        {
            if ( request.IsValid() )
            {
                for ( var it = 0; it < m_Processors.Count; it++ )
                {
                    var currentProcessor = m_Processors[ it ];
                    if ( currentProcessor != null )
                    {
                        if ( currentProcessor.IsHandlablePacket( request ) )
                        {
                            return currentProcessor.Handle( request );
                        }
                    }
                    else
                    {
#if DEBUG
                        throw new NetworkProcessingException( $"{MethodBase.GetCurrentMethod()}: Current processor has been a null!" );
#endif
                    }
                }
#if DEBUG
                throw new NetworkProcessingException( $"{MethodBase.GetCurrentMethod()}: Not found valid processor for request '{request.Identifier}'!" );
#endif
            }
            else
            {
#if DEBUG
                throw new NetworkProcessingException( $"{MethodBase.GetCurrentMethod()}: Request '{request.Identifier}' invalid!" );
#endif
            }

#if DEBUG
            throw new NetworkProcessingException( $"{MethodBase.GetCurrentMethod()}: Request '{request.Identifier}' not handle and return null!" );
#else
            return null;
#endif
        }
    }
}
