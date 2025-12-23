// 2025-12-23
using System.Collections.ObjectModel;
using TestCheckList.Core.Source.ViewModels.Interfaces;
using TestCheckList.Models;

namespace TestCheckList.ViewModels;

///<summary>Implementacion del ViewModel principal para la gestion de la lista de tareas</summary>
public class TaskListViewModel : ITaskListViewModel {

	#region Variables
	private ObservableCollection<IFilaPasoViewModel> _rows = new();
	#endregion

	#region Funciones Externas
	///<summary>Coleccion de ViewModels de fila para renderizado manual</summary>
	public ObservableCollection<IFilaPasoViewModel> Rows => _rows;

	///<summary>Carga registros de prueba iniciales</summary>
	public void Initialize() {
		_rows.Clear();

		// Datos de prueba iniciales
		AgregarTareaPrueba(0, "Configurar entorno de desarrollo", TaskState.Success);
		AgregarTareaPrueba(1, "Implementar KeyListener nativo", TaskState.Success);
		AgregarTareaPrueba(2, "Corregir desplazamiento de ScrollView", TaskState.Pending);
		AgregarTareaPrueba(3, "Refactorizar a arquitectura ViewModel", TaskState.Pending);
		AgregarTareaPrueba(4, "Persistir cambios en documento", TaskState.Pending);
		AgregarTareaPrueba(5, "Verificar consistencia en Mac Catalyst", TaskState.Failed);
	}

	///<summary>Procesa el cambio de estado de una fila y prepara la persistencia</summary>
	public void HandleRowStateChanged(int id, TaskState newState) {
		var row = _rows.FirstOrDefault(r => r.Id == id);
		if (row != null) {
			row.State = newState;
			// TODO: Invocar capa de negocio para guardar el documento persistente
			System.Diagnostics.Debug.WriteLine($"Logica de Negocio: Guardando Tarea {id} con estado {newState}");
		}
	}
	#endregion

	#region Funciones internas
	///<summary>Metodo auxiliar para instanciar y configurar ViewModels de fila</summary>
	private void AgregarTareaPrueba(int id, string titulo, TaskState estado) {
		FilaPasoViewModel rowVm = new(id, titulo, estado);
		rowVm.Index = _rows.Count;

		// Nos suscribimos a cambios en la fila para reaccionar desde la lista si es necesario
		rowVm.OnPropertyChanged += (propName) => {
			if (propName == nameof(IFilaPasoViewModel.State)) {
				HandleRowStateChanged(rowVm.Id, rowVm.State);
			}
		};

		_rows.Add(rowVm);
	}
	#endregion

}