
using System.Collections.ObjectModel;
using Microsoft.Maui.Devices;
using TestCheckList.Core.Source.ViewModels.Interfaces;

namespace TestCheckList.Views.Maui;

public partial class TaskListView : ContentView {

	#region Variables

	private int _selectedIndex = -1;

	private ITaskListViewModel? _viewModel;

	#endregion

	#region Funciones públicas

	public TaskListView() {

		InitializeComponent();
		
		
		RenderizarFilas();
		Loaded += (s, e) => SetFocusedItem(0);
	}

	public void SetViewModel(ITaskListViewModel viewModel) {
		_viewModel = viewModel;
		_viewModel.Initialize();
	}

	#endregion

	#region Funciones internas

	///<summary>Desplaza el ScrollView para asegurar que el elemento sea visible</summary>
	private async void AsegurarVisibilidad(VisualElement element) {
		// Esperamos un breve momento para que el layout se actualice si hubo expansion
		await Task.Delay(100);
		await _mainScroll.ScrollToAsync(element, ScrollToPosition.Start, true);
	}

	private void RenderizarFilas() {
		_tasksView.Children.Clear();
		if (_viewModel is null) return;

		foreach (var rowVm in _viewModel.Rows) {
			FilaPasoView fila = new();
			fila.SetViewModel(rowVm);

			fila.KeyPressed = (key) => {
				if (key.Key == UniversalKey.ArrowDown) SetFocusedItem(rowVm.Index + 1);
				if (key.Key == UniversalKey.ArrowUp) SetFocusedItem(rowVm.Index - 1);
			};

			_tasksView.Children.Add(fila);
		}
	}

	///<summary>Mueve el foco visual y lógico a una fila específica</summary>
	private void SetFocusedItem(int index) {
		if (index < 0 || index >= _tasksView.Children.Count) return;

		_selectedIndex = index;
		var target = (FilaPasoView)_tasksView.Children[_selectedIndex];
		target.SetFocus();
		AsegurarVisibilidad(target);
	}

	#endregion

}
/*
 * private void CargarTareasManual() {
		_tasksView.Children.Clear();

		foreach (var item in _items) {
			FilaPasoView fila = new() {
				Titulo = item.Titulo,
				State = item.Estado,
				Index = _items.IndexOf(item)
			};
			_tasksView.Children.Add(fila);
			fila.KeyPressed = (key) => {
				if (key.Key == UniversalKey.ArrowDown)
					SetFocusedItem(fila.Index + 1);
				if (key.Key == UniversalKey.ArrowUp)
					SetFocusedItem(fila.Index - 1);
			};
		}
	}

 * private void SetFocusedItem(int index) {
		if (index < 0 || index >= _tasksView.Children.Count) {
#if MACCATALYST
			AudioToolbox.SystemSound.Vibrate.PlaySystemSound();
#endif
			return; 
		}

		// Quitar foco al anterior
		if (_selectedIndex >= 0) {
			((FilaPasoView)_tasksView.Children[_selectedIndex]).Unfocus();
		}

		_selectedIndex = index;
		var target = (FilaPasoView)_tasksView.Children[_selectedIndex];

		target.SetFocus();
		AsegurarVisibilidad(target);
	}
 * private readonly ObservableCollection<TaskItemDto> _items = new() {
		new(0, "Tarea 1", Models.TaskState.Pending),
		new(1, "Tarea 2", Models.TaskState.Pending),
		new(2, "Tarea 3", Models.TaskState.Pending),
		new(3, "Tarea 4", Models.TaskState.Pending),
		new(4, "Tarea 5", Models.TaskState.Pending),
		new(5, "Tarea 6", Models.TaskState.Pending)
	};
 */