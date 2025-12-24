
using System.Collections.ObjectModel;
using Microsoft.Maui.Devices;
using TestCheckList.ViewModels;

namespace TestCheckList.Views.Maui;

public partial class TaskListView : ContentView {

	#region Variables

	private int _selectedIndex = -1;

	private ITaskListViewModel? _viewModel;

	#endregion

	#region Funciones públicas

	public TaskListView() {

		InitializeComponent();
		
	}

	public void SetViewModel(ITaskListViewModel viewModel) {
		_viewModel = viewModel;
		RenderizarFilas();
	}

	public void RenderizarFilas() {

		if (_viewModel is null) return;

		_tasksView.Children.Clear();
		

		foreach (var rowVm in _viewModel.Rows) {
			FilaPasoView fila = new();
			fila.SetViewModel(rowVm);

			fila.KeyPressed = (key) => {
				if (key.Key == UniversalKey.ArrowDown) SetFocusedItem(rowVm.Index + 1);
				if (key.Key == UniversalKey.ArrowUp) SetFocusedItem(rowVm.Index - 1);
			};

			_tasksView.Children.Add(fila);
		}

		if (_tasksView.Children.Count > 0) {
			SetFocusedItem(0);
		}
	}

	#endregion

	#region Funciones internas

	///<summary>Desplaza el ScrollView para asegurar que el elemento sea visible</summary>
	private async void AsegurarVisibilidad(VisualElement element) {
		// Esperamos un breve momento para que el layout se actualice si hubo expansion
		await Task.Delay(100);
		await _mainScroll.ScrollToAsync(element, ScrollToPosition.Start, true);
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