// 2025-12-23
using TestCheckList.Models;

namespace TestCheckList.ViewModels;

///<summary>Interfaz para el estado y logica de una fila individual</summary>
public interface IFilaPasoViewModel {

	int Id { get; }
	string Titulo { get; }
	TaskState State { get; set; }
	string? Comentario { get; set; }
	int Index { get; set; }

	///<summary>Notifica que una propiedad ha cambiado para actualizacion manual de la UI</summary>
	event Action<string>? OnPropertyChanged;

}