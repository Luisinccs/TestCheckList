// 2025-12-22

#if WINDOWS
using Microsoft.UI.Input;
using TestCheckList.Models;
using Windows.System;
using Windows.UI.Core;


///<summary>Extensiones para la captura de modificadores en el entorno de Windows</summary>
public static class InputKeyboardSourceExtensions {

	#region Funciones Externas
	///<summary>Extrae los modificadores activos y los mapea a la enumeracion universal</summary>
	public static UniversalModifier GetModifiers() {
		UniversalModifier modifiers = UniversalModifier.None;
		var getKeyState = InputKeyboardSource.GetKeyStateForCurrentThread;

		if (IsKeyDown(getKeyState(VirtualKey.Shift))) {
			modifiers |= UniversalModifier.Shift;
		}
		if (IsKeyDown(getKeyState(VirtualKey.Control))) {
			modifiers |= UniversalModifier.Control;
		}
		if (IsKeyDown(getKeyState(VirtualKey.Menu))) {
			modifiers |= UniversalModifier.Alt;
		}
		if (IsKeyDown(getKeyState(VirtualKey.LeftWindows)) || IsKeyDown(getKeyState(VirtualKey.RightWindows))) {
			modifiers |= UniversalModifier.Command;
		}

		return modifiers;
	}
	#endregion

	#region Funciones internas
	///<summary>Determina si el estado de una tecla virtual corresponde a estar presionada</summary>
	private static bool IsKeyDown(CoreVirtualKeyStates state) {
		return state.HasFlag(CoreVirtualKeyStates.Down);
	}
	#endregion

}

#endif