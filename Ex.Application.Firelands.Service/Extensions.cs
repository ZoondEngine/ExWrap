using System;

namespace Ex.Application.Firelands.Service
{
    public static class Extensions
    {
        public static bool Zero( this Version ver )
            => ver.ToString() == "0.0.0.0";
    }
}
