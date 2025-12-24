// 2025-12-23
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestCheckList.ViewModels;
using TestCheckList.Models;

namespace TestCheckList.ViewModels;

///<summary>ViewModel de aplicacion que procesa datos de forma agnostica a la plataforma</summary>
public class MainAppPageViewModel : IMainAppPageViewModel {

	#region Variables

	private ITaskListViewModel _taskListViewModel;
	public event Action? OnDataLoaded;

	#endregion

	#region Funciones Externas
	public MainAppPageViewModel() {
		_taskListViewModel = new TaskListViewModel();
	}

	public ITaskListViewModel TaskListViewModel => _taskListViewModel;

	///<summary>Lee el flujo y vincula cada fila al sistema de autoguardado del TaskList</summary>
	public async Task LoadDataAsync(Stream fileStream, string path) {
		try {
			_taskListViewModel.CurrentFilePath = path;
			using var reader = new StreamReader(fileStream, Encoding.UTF8);
			string content = await reader.ReadToEndAsync();

			List<TaskItem> tasks = ParseSCheckContent(content);

			_taskListViewModel.Rows.Clear();
			foreach (TaskItem task in tasks) {
				var rowVm = new FilaPasoViewModel(task.Id, task.Title, task.State) {
					Index = _taskListViewModel.Rows.Count,
					Comentario = task.Comment
				};

				// VINCULO CRITICO: Suscribimos la fila al sistema de guardado del padre
				((TaskListViewModel)_taskListViewModel).ConfigurarFila(rowVm);

				_taskListViewModel.Rows.Add(rowVm);
			}

			OnDataLoaded?.Invoke();
		} catch (Exception ex) {
			System.Diagnostics.Debug.WriteLine($"Error Core: {ex.Message}");
		} finally {
			fileStream.Dispose();
		}
	}

	#endregion

	#region Funciones internas

	///<summary>Analiza el contenido buscando bloques @task y @endtask</summary>
	private List<TaskItem> ParseSCheckContent(string content) {
		
		List<TaskItem> tasks = new(); 
        using StringReader reader = new(content);

		string? line;
		TaskItem? currentTask = null;
		bool enMetadatos = false;

		while ((line = reader.ReadLine()) != null) {
			line = line.Trim();
			if (string.IsNullOrEmpty(line)) continue;
			if (line.Equals("@metadatos", StringComparison.OrdinalIgnoreCase)) {
				enMetadatos = true;
				continue;
			}
			if (line.Equals("@endmetadatos", StringComparison.OrdinalIgnoreCase)) {
				enMetadatos = false;
				continue;
			}
			if (enMetadatos) {
				var parts = line.Split(':', 2);
				if (parts.Length == 2) {
					_taskListViewModel.Metadata[parts[0].Trim()] = parts[1].Trim();
				}
				continue;
			}
			if (line.Equals("@task", StringComparison.OrdinalIgnoreCase)) {
				currentTask = new TaskItem (tasks.Count);
			} else if (line.Equals("@endtask", StringComparison.OrdinalIgnoreCase) && currentTask != null) {
				tasks.Add(currentTask);
				currentTask = null;
			} else if (currentTask != null) {
				ParseTaskLine(line, currentTask);
			}
		}
		return tasks;
	}

	///<summary>Mapea los atributos @title, @state y @comment al objeto TaskItem</summary>
	private void ParseTaskLine(string line, TaskItem task) {
		if (line.StartsWith("@title ", StringComparison.OrdinalIgnoreCase)) {
			task.Title = line.Substring(7).Trim();
		} else if (line.StartsWith("@state ", StringComparison.OrdinalIgnoreCase)) {
			string stateStr = line.Substring(7).Trim().ToLower();
			// Mapeo flexible para soportar 'pendent' o 'pending' 
			task.State = stateStr switch {
				"success" => TaskState.Success,
				"failed" => TaskState.Failed,
				_ => TaskState.Pending
			};
		} else if (line.StartsWith("@comment ", StringComparison.OrdinalIgnoreCase)) {
			// Soporte para comentarios persistentes
			task.Comment = line.Substring(9).Trim();
		}
	}
	
	#endregion

}