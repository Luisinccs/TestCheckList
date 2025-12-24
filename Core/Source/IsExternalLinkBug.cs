
// Esto hay que agregarlo para silenciar el error:
// predefined type 'System.Runtime.CompilerServices.IsExternalInit' is not defined or imported
// ver: https://stackoverflow.com/questions/62648189/testing-c-sharp-9-0-in-vs2019-cs0518-isexternalinit-is-not-defined-or-imported

using System.ComponentModel;

namespace System.Runtime.CompilerServices {
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class IsExternalInit { }
}
