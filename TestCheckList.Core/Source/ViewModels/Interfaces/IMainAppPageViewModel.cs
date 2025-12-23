// 2025-12-23
using System;
using TestCheckList.Core.Source.ViewModels.Interfaces;

namespace TestCheckList.ViewModels;

///<summary>Interfaz para la logica de la pagina principal y gestion de archivos</summary>
public interface IMainAppPageViewModel {

	ITaskListViewModel TaskListViewModel { get; }
	///<summary>Procesa el flujo de datos del archivo seleccionado</summary>
	Task LoadDataAsync(Stream fileStream);
	///<summary>Notifica que los datos han sido procesados con exito</summary>
	event Action? OnDataLoaded;

}