using Ex.Attributes;
using Ex.Exceptions;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ex
{
    internal class ExRequiredAttributeController
    {
        public static void Inject( ExObject obj )
           => Inject( obj, ParseRequiredObjects( obj ) );
        public static void Inject( ExObject obj, Type[] requires )
        {
            foreach ( var require in requires )
            {
                var storedRequire = ExObjectManager.GetObjectByTypeInstance<ExObject>( require );
                if ( storedRequire == null )
                {
                    storedRequire = ( ExBehaviour )Activator.CreateInstance( require );
                }

                var method = typeof( ExObject ).GetMethod( "AddComponentByTypeInstance_Internal", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof( ExBehaviour ) }, null );
                if ( method != null )
                {
                    method.Invoke( obj, new object[] { storedRequire } );
                }
                else
                {
                    throw new RequiredControllerException( $"{MethodBase.GetCurrentMethod().Name}: Can't inject required component '{require.FullName}'. Method 'AddComponentByTypeInstance_Internal' not found!" );
                }
            }
        }
        public static Type[] ParseRequiredObjects( ExObject obj )
        {
            List<Type> requires = new List<Type>();

            var array = ( RequiredBehaviourAttribute[] )obj.GetType().GetCustomAttributes( typeof( RequiredBehaviourAttribute ), true );
            foreach ( var require in array )
            {
                requires.Add( require.RequiredObjectType );
            }

            return requires.ToArray();
        }
    }
}
