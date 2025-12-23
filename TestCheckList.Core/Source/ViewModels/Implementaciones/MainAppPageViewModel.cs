// 2025-12-23
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestCheckList.Core.Source.ViewModels.Interfaces;
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

	///<summary>Lee el stream y construye la lista de tareas con sus comentarios</summary>
	public async Task LoadDataAsync(Stream fileStream) {
		try {
			using var reader = new StreamReader(fileStream, Encoding.UTF8);
			string content = await reader.ReadToEndAsync();

			var tasks = ParseSCheckContent(content);

			_taskListViewModel.Rows.Clear();
			foreach (var task in tasks) {
				_taskListViewModel.Rows.Add(new FilaPasoViewModel(task.Id, task.Title, task.State) {
					Index = _taskListViewModel.Rows.Count,
					Comentario = task.Comment // Se asigna el comentario parseado
				});
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
	///<summary>Analiza el texto linea a linea buscando etiquetas de tarea</summary>
	private List<TaskItemDto> ParseSCheckContent(string content) {
		var tasks = new List<TaskItemDto>();
		using var reader = new StringReader(content);
		string? line;
		TaskItemDto? currentTask = null;

		while ((line = reader.ReadLine()) != null) {
			line = line.Trim();
			if (string.IsNullOrEmpty(line)) continue;

			if (line.Equals("@task", StringComparison.OrdinalIgnoreCase)) {
				currentTask = new TaskItemDto { Index = tasks.Count };
			} else if (line.Equals("@endtask", StringComparison.OrdinalIgnoreCase) && currentTask != null) {
				tasks.Add(currentTask);
				currentTask = null;
			} else if (currentTask != null) {
				ParseTaskLine(line, currentTask);
			}
		}
		return tasks;
	}

	///<summary>Mapea atributos especificos incluyendo el soporte para @comment</summary>
	private void ParseTaskLine(string line, TaskItemDto task) {
		if (line.StartsWith("@title ", StringComparison.OrdinalIgnoreCase)) {
			task.Titulo = line.Substring(7).Trim();
		} else if (line.StartsWith("@state ", StringComparison.OrdinalIgnoreCase)) {
			string stateStr = line.Substring(7).Trim().ToLower();
			task.Estado = stateStr switch {
				"success" => TaskState.Success,
				"failed" => TaskState.Failed,
				 _ => TaskState.Pending // Mapeo de 'pendent' o desconocido 
			};
		} else if (line.StartsWith("@comment ", StringComparison.OrdinalIgnoreCase)) {
			// Captura el texto despues de la etiqueta @comment
			task.Comment = line.Substring(9).Trim();
		}
	}
	#endregion

}