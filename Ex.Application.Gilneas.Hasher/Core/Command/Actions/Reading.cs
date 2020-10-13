using System;

namespace Ex.Application.Gilneas.Hasher.Core.Command.Actions
{
    public class Reading
    {
        public string AsString()
            => Console.ReadLine();
        public bool AsBool()
            => bool.Parse( Console.ReadLine() );
        public int AsInt()
            => int.Parse( Console.ReadLine() );
        public uint AsUInt()
            => uint.Parse( Console.ReadLine() );
        public long AsLong()
            => long.Parse( Console.ReadLine() );
        public ulong AsULong()
            => ulong.Parse( Console.ReadLine() );
    }
}
