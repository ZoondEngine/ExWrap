namespace Ex.Plugin.Arg.Core
{
    public class Arg
    {
        private string m_Raw;
        private int m_ComparisonIndex;

        public Arg(string raw, int idx)
        {
            m_Raw = raw;
            m_ComparisonIndex = idx;
        }

        public static Arg Parse( string raw, int idx )
            => new Arg( raw.Replace( "-", "" ), idx );

        public T Convert<T>()
            => (T)System.Convert.ChangeType( m_Raw, typeof( T ) );

        public string Raw()
            => m_Raw;
        public int Index()
            => m_ComparisonIndex;
    }
}
