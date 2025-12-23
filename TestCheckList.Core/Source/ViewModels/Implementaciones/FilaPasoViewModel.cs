// 2025-12-23
using TestCheckList.Models;

namespace TestCheckList.ViewModels;

///<summary>Implementacion concreta del ViewModel para una fila</summary>
public class FilaPasoViewModel : IFilaPasoViewModel {

	#region Variables
	private TaskState _state;
	private string? _comentario;
	private int _id;
	private string _titulo;
	private int _index;
	public event Action<string>? OnPropertyChanged;
	#endregion

	#region Funciones Externas
	public FilaPasoViewModel(int id, string titulo, TaskState state) {
		_id = id;
		_titulo = titulo;
		_state = state;
	}

	public int Id => _id;
	public string Titulo => _titulo;

	public int Index {
		get => _index;
		set { _index = value; OnPropertyChanged?.Invoke(nameof(Index)); }
	}

	public TaskState State {
		get => _state;
		set {
			if (_state != value) {
				_state = value;
				OnPropertyChanged?.Invoke(nameof(State));
			}
		}
	}

	public string? Comentario {
		get => _comentario;
		set {
			if (_comentario != value) {
				_comentario = value;
				OnPropertyChanged?.Invoke(nameof(Comentario));
			}
		}
	}
	#endregion
}