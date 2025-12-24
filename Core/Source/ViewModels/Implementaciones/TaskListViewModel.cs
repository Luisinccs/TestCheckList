// 2025-12-23
using System.Collections.ObjectModel;
using TestCheckList.Models;
using TestCheckList.Parsers;

namespace TestCheckList.ViewModels;

///<summary>Implementacion del ViewModel principal con soporte para suscripcion de eventos</summary>
public class TaskListViewModel : ITaskListViewModel {

	#region Variables

	private bool _isSaving;
	private ObservableCollection<IFilaPasoViewModel> _rows = new();
	private string? _currentFilePath;
	private CancellationTokenSource? _debounceTokenSource;
	private readonly CheckDocSerializer _serializer = new();
	public event Action<string>? OnPropertyChanged;

	#endregion

	#region Funciones Externas

	public ObservableCollection<IFilaPasoViewModel> Rows => _rows;

	public string? CurrentFilePath {
		get => _currentFilePath;
		set => _currentFilePath = value;
	}

	public bool IsSaving {
		get => _isSaving;
		private set { _isSaving = value; OnPropertyChanged?.Invoke(nameof(IsSaving)); }
	}

	public Dictionary<string, string> Metadata { get; set; } = new();

	///<summary>Configura la escucha de cambios en una fila para disparar el guardado</summary>
	public void ConfigurarFila(IFilaPasoViewModel rowVm) {
		rowVm.OnPropertyChanged += async (prop) => {
			if (prop == nameof(IFilaPasoViewModel.State)) {
				await SaveAsync(); // Guardado inmediato para cambios de estado
			} else if (prop == nameof(IFilaPasoViewModel.Comentario)) {
				await PlanificarGuardadoConDebounce(); // Guardado diferido para texto
			}
		};
	}

	///<summary>Serializa y guarda el documento en la ruta actual</summary>
	public async Task SaveAsync() {
		if (string.IsNullOrEmpty(_currentFilePath)) return;

		IsSaving = true;
		try {
			Document doc = new() {
				FilePath = _currentFilePath,
				Tasks = new ObservableCollection<TaskItem>(),
				Metadata = this.Metadata
			};

			foreach (var row in _rows) {
				doc.Tasks.Add(new TaskItem(row.Id) {
					Title = row.Titulo,
					State = row.State,
					Comment = row.Comentario
				});
			}
			await _serializer.SaveAsync(doc);
		} finally {
			await Task.Delay(500); // Respiro visual para el indicador
			IsSaving = false;
		}
	}

	#endregion

	#region Funciones internas

	///<summary>Espera a que el usuario deje de escribir para persistir los cambios</summary>
	private async Task PlanificarGuardadoConDebounce() {
		_debounceTokenSource?.Cancel();
		_debounceTokenSource = new CancellationTokenSource();

		IsSaving = true;
		try {
			await Task.Delay(1500, _debounceTokenSource.Token);
			await SaveAsync();
		} catch (TaskCanceledException) {
			// El usuario sigue escribiendo
		}
	}

	#endregion

}