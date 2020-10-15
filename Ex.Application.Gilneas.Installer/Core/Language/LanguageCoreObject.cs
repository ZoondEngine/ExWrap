using Ex.Application.Gilneas.Installer.Core.Language.Behaviours;
using Ex.Attributes;

using System.Globalization;
using System.Threading;

namespace Ex.Application.Gilneas.Installer.Core.Language
{
    [RequiredBehaviour(typeof(LangStorageBehaviour))]
    public class LanguageCoreObject : ExObject
    {
        public string Word( string locale, string key )
            => Storage().Word( locale, key );

        public string DefWord( string key )
            => Storage().Word( Language(), key );

        public static string Replace( string where, string what, string @new )
            => where.Replace( what, @new );

        public CultureInfo Culture()
            => Thread.CurrentThread.CurrentCulture;

        public string Language()
            => Culture().TwoLetterISOLanguageName;
        
        public LangStorageBehaviour Storage()
            => GetComponent<LangStorageBehaviour>();
    }
}
