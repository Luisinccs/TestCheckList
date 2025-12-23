
using Microsoft.Maui.Handlers;
using TestCheckList.Models;

namespace TestCheckList.Views.Maui;

public partial class FilaPasoView : ContentView {

	#region Variables

	/// <summary>controla si estamos en modo expansión para evitar que otras teclas (como S o F) interfieran mientras el usuario escribe</summary>
	private bool _isEditing = false;
	private int _index;
	private IKeyListenerView _keyListenerView;
	private TaskState _state = TaskState.Pending;
	private string? _titulo = null;

	#endregion

	#region Funciones internas

	///<summary>Controla visualmente el borde segun el estado de la fila</summary>
	private void ActualizarResaltado(bool enfocado) {
		System.Diagnostics.Debug.WriteLine($"Actualizando resaltado de la fila {Index}. Enfocado: {enfocado}, Editando: {_isEditing}");
		// Mantenemos el resalte si estamos editando o si el listener tiene el foco
		if (enfocado || _isEditing) {
			_mainBorder.Stroke = Colors.Blue;
			_mainBorder.StrokeThickness = 4;
		} else {
			_mainBorder.Stroke = Colors.Transparent;
			_mainBorder.StrokeThickness = 1;
		}
	}

	///<summary>Expande la seccion de comentario y traslada el foco al editor</summary>
	private async void AlternarEdicion() {
		_isEditing = true;
		_frameComentario.IsVisible = true;
		ActualizarResaltado(true);

		// Esperamos un frame para que el layout se estabilice
		await Task.Yield();

		// Enfocamos el editor para empezar a escribir inmediatamente
		_txtComentario.Focus();
	}

	private void ConfigurarEditor() {
		_txtComentario.Completed += (s, e) => {
			_isEditing = false;
			// todo: Si tiene texto se mantiene visible, de lo contrario se oculta
			_frameComentario.IsVisible = false; // O mantenerlo visible si prefieres
			SetFocus(); // Devolvemos el foco al listener de la fila
		};
	}

	private void OnLoaded(object? sender, EventArgs e) {
		//System.Diagnostics.Debug.WriteLine("Fila Loaded");
		var handler = Handler;
		_keyListenerView.SetupHandler(handler);
		_keyListenerView.Focusing = () => ActualizarResaltado(true);
		_keyListenerView.Unfocusing = () => ActualizarResaltado(_txtComentario.IsFocused);
		_keyListenerView.OnKeyPressed = (keyInfo) => OnKeyPressed(keyInfo);

		ConfigurarEditor();
	}

	private void OnKeyPressed(KeyPressedInfo keyInfo) {
		System.Diagnostics.Debug.WriteLine($"Key pressed: {keyInfo.Key}");

		if (keyInfo.Key == UniversalKey.ArrowDown || (keyInfo.Key == UniversalKey.ArrowUp))
			KeyPressed?.Invoke(keyInfo);
		else {
			switch (keyInfo.Key) {
				case UniversalKey.F:
					State = TaskState.Failed;
					break;
				case UniversalKey.S:
					State = TaskState.Success;
					break;
				case UniversalKey.D:
					State = TaskState.Pending;
					break;
				case UniversalKey.Enter:
					AlternarEdicion();
					break;
				default:
					break;
			}
		}
	}


	#endregion

	#region Funciones observables

	public Action<KeyPressedInfo>? KeyPressed { get; set; }

	public FilaPasoView() {
		InitializeComponent();

		_keyListenerView = KeyListenerFactory.Create();

		// El editor debe notificar cuando gana/pierde foco para mantener el borde
		_txtComentario.Focused += (s, e) => ActualizarResaltado(true);
		_txtComentario.Unfocused += (s, e) => ActualizarResaltado(false);
		Loaded += OnLoaded;
	}

	public void SetFocus() {
		//System.Diagnostics.Debug.WriteLine("Set Focus");
		_keyListenerView.Focus();
	}

	public int Index {
		get => _index;
		set {
			_index = value;
			_mainBorder.BackgroundColor = (value % 2 == 0)
			? Color.FromArgb("#333333") // Gris oscuro [cite: 2025-12-13]
			: Color.FromArgb("#444444");
		}
	}

	public string? Titulo {
		get => _titulo;
		set {
			_titulo = value;
			_lblTitulo.Text = _titulo;
		}
	}

	///<summary></summary>
	public TaskState State {
		get => _state;
		set {
			_state = value;
			_checkIcon.Source = _state switch {
				TaskState.Pending => "check_blank.png",
				TaskState.Success => "check_success.png",
				TaskState.Failed => "check_failed.png",
				_ => throw new NotImplementedException(),
			};
		}
	}

	#endregion

}