
using Microsoft.Maui.Handlers;
using TestCheckList.Models;

namespace TestCheckList.Views.Maui;

public partial class FilaPasoView : ContentView {

	public Action<KeyPressedInfo>? KeyPressed { get; set; }

	private int _index;

	private IKeyListenerView _keyListenerView;

	private TaskState _state = TaskState.Pending;
	private string? _titulo = null;

	public FilaPasoView() {
		InitializeComponent();
#if MACCATALYST
		_keyListenerView = new MacKeyListenerView();
#endif
		Loaded += OnLoaded;
	}

	private void OnLoaded(object? sender, EventArgs e) {
		//System.Diagnostics.Debug.WriteLine("Fila Loaded");
		var handler = Handler;
		_keyListenerView.SetupHandler(handler);		
		_keyListenerView.Focusing = () => {
			System.Diagnostics.Debug.WriteLine($"Fila {Index} focusing");
			_mainBorder.Stroke = Colors.Blue; // Usando tu color de acento [cite: 2025-12-13]
			_mainBorder.StrokeThickness = 4;
		};
		_keyListenerView.Unfocusing = () => {
			System.Diagnostics.Debug.WriteLine($"Fila {Index} unfocusing");
			_mainBorder.Stroke = Colors.Transparent;
			_mainBorder.StrokeThickness = 1;
		};
		_keyListenerView.OnKeyPressed = (keyInfo) => OnKeyPressed(keyInfo);
		
	}

	private void OnKeyPressed(KeyPressedInfo keyInfo) {
		System.Diagnostics.Debug.WriteLine($"Key pressed: {keyInfo.KeyCode}");
		if (keyInfo.KeyCode == 81 || keyInfo.KeyCode == 82)
			KeyPressed?.Invoke(keyInfo);
		else {
			switch (keyInfo.KeyCode) {
				case 9:
					State = TaskState.Failed;
					break;
				case 22:
					State = TaskState.Success;
					break;
				case 7:
					State = TaskState.Pending;
					break;
				default:
					break;
			}
		}
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
				TaskState.Pending => "check_blank",
				TaskState.Success => "check_success",
				TaskState.Failed => "check_failed",
				_ => throw new NotImplementedException(),
			}; 
		}
	}
	
}