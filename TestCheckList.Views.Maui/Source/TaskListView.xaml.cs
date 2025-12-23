
using System.Collections.ObjectModel;
using TestCheckList.ViewModels;
using Microsoft.Maui.Devices;

namespace TestCheckList.Views.Maui;

public partial class TaskListView : ContentView {

	#region Variables

	private int _selectedIndex = -1;

	private readonly ObservableCollection<TaskItemDto> _items = new() {
		new(0, "Tarea 1", Models.TaskState.Pending),
		new(1, "Tarea 2", Models.TaskState.Pending),
		new(2, "Tarea 3", Models.TaskState.Pending),
		new(3, "Tarea 4", Models.TaskState.Pending),
		new(4, "Tarea 5", Models.TaskState.Pending),
		new(5, "Tarea 6", Models.TaskState.Pending)
	};

	#endregion

	public TaskListView() {

		InitializeComponent();
		CargarTareasManual();
		Loaded += OnViewLoaded;

	}

	#region Funciones internas

	///<summary>Desplaza el ScrollView para asegurar que el elemento sea visible</summary>
	private async void AsegurarVisibilidad(VisualElement element) {
		// Esperamos un breve momento para que el layout se actualice si hubo expansion
		await Task.Delay(100);
		await _mainScroll.ScrollToAsync(element, ScrollToPosition.Start, true);
	}

	///<summary>Establece el foco inicial al cargar la vista</summary>
	private void OnViewLoaded(object? sender, EventArgs e) {
		if (_tasksView.Children.Count > 0) {
			SetFocusedItem(0);
		}
	}

	private void CargarTareasManual() {
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

	///<summary>Mueve el foco visual y lógico a una fila específica</summary>
	private void SetFocusedItem(int index) {
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

	#endregion
}