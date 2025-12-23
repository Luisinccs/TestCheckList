// 2025-12-16

using Microsoft.Maui;using System;

///<summary>Interfaz para la gestion de eventos de teclado nativos por plataforma</summary>
public interface IKeyListenerView {    ///<summary>Evento disparado al detectar una tecla</summary>    Action<KeyPressedInfo>? OnKeyPressed { get; set; }
    ///<summary>Evento disparado al recibir el foco</summary>    Action? Focusing { get; set; }
    ///<summary>Evento disparado al perder el foco</summary>    Action? Unfocusing { get; set; }    ///<summary>Solicita el foco al componente nativo</summary>    void Focus();
    ///<summary>Configura la infraestructura sobre el handler de MAUI</summary>    void SetupHandler(IViewHandler? handler);    
}

///<summary>Fabrica estatica para obtener la instancia correcta del listener segun plataforma</summary>
public static class KeyListenerFactory {

	#region Funciones Externas
	///<summary>Crea una nueva instancia del listener para la plataforma actual</summary>
	public static IKeyListenerView Create() {
#if WINDOWS
		return new WinKeyListenerView();
#elif MACCATALYST
        return new MacKeyListenerView();
#else
        throw new PlatformNotSupportedException("Plataforma no soportada para KeyListener");
#endif
	}
	#endregion

}