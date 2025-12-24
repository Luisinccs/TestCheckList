// 2025-12-23
using System.Collections.ObjectModel;
using TestCheckList.ViewModels;

namespace TestCheckList.ViewModels;

///<summary>Interfaz para la gestion de la lista completa de tareas</summary>
public interface ITaskListViewModel {

	Dictionary<string, string> Metadata { get; set; }

	ObservableCollection<IFilaPasoViewModel> Rows { get; }

	string? CurrentFilePath { get; set; }

	//void Initialize();

	Task SaveAsync();

	bool IsSaving { get; }
	
	event Action<string>? OnPropertyChanged;

}