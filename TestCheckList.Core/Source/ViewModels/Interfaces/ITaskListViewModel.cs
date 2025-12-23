// 2025-12-23
using System.Collections.ObjectModel;
using TestCheckList.ViewModels;

namespace TestCheckList.Core.Source.ViewModels.Interfaces;

///<summary>Interfaz para la gestion de la lista completa de tareas</summary>
public interface ITaskListViewModel {

	ObservableCollection<IFilaPasoViewModel> Rows { get; }
	void Initialize();
	void HandleRowStateChanged(int id, Models.TaskState newState);

}