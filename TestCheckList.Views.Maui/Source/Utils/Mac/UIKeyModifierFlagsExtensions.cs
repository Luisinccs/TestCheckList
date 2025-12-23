// 2025-12-22
#if MACCATALYST
using TestCheckList.Models;
using UIKit;


///<summary>Extensiones para mapear modificadores de UIKit a la enumeracion universal</summary>
public static class UIKeyModifierFlagsExtensions {

	#region Funciones Externas
    ///<summary>Convierte banderas de modificadores de iOS/Mac a UniversalModifier</summary>
    public static UniversalModifier ToUniversalModifier(this UIKeyModifierFlags flags) {
        UniversalModifier modifiers = UniversalModifier.None;

        if (flags.HasFlag(UIKeyModifierFlags.Shift)) modifiers |= UniversalModifier.Shift;
        if (flags.HasFlag(UIKeyModifierFlags.Control)) modifiers |= UniversalModifier.Control;
        if (flags.HasFlag(UIKeyModifierFlags.Alternate)) modifiers |= UniversalModifier.Alt;
        if (flags.HasFlag(UIKeyModifierFlags.Command)) modifiers |= UniversalModifier.Command;

        return modifiers;
    }
	#endregion

}
#endif