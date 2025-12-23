// 2025-12-22

#if WINDOWS
using Microsoft.Maui.Handlers;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using System;
using TestCheckList.Models;
using Windows.System;


///<summary>Implementacion del listener de teclado para Windows WinUI 3</summary>
public class WinKeyListenerView : IKeyListenerView {

	#region Variables
	private FrameworkElement? _nativeView;
	public Action<KeyPressedInfo>? OnKeyPressed { get; set; }
	public Action? Focusing { get; set; }
	public Action? Unfocusing { get; set; }
	#endregion

	#region Funciones Externas
	public void Focus() {
		_nativeView?.Focus(FocusState.Programmatic);
	}

	public void SetupHandler(IViewHandler? handler) {
		_nativeView = handler?.PlatformView as FrameworkElement;
		if (_nativeView != null) {
			_nativeView.IsTabStop = true;
			_nativeView.KeyUp += OnWinKeyUp;
			_nativeView.GotFocus += (s, e) => Focusing?.Invoke();
			_nativeView.LostFocus += (s, e) => Unfocusing?.Invoke();
		}
	}
	#endregion

	#region Funciones internas

	private void OnWinKeyUp(object sender, KeyRoutedEventArgs e) {

		UniversalKey uKey = e.Key.ToUniversalKey();
		OnKeyPressed?.Invoke(new KeyPressedInfo(e.Key.ToString(), uKey, InputKeyboardSourceExtensions.GetModifiers()));
		e.Handled = true;
	}

	#endregion

}
#endif