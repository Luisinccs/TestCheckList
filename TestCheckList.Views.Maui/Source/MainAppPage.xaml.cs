// 2025-12-23
using System;
using TestCheckList.ViewModels;

namespace TestCheckList.Views.Maui;

///<summary>Pagina principal con enlace de datos manual</summary>
public partial class MainAppPage : ContentPage {

	#region Variables
	private IMainAppPageViewModel _viewModel;
	#endregion

	#region Funciones internas

	///<summary>Constructor</summary>
	public MainAppPage() {
		InitializeComponent();
		_viewModel = new MainAppPageViewModel();
		ConfigurarSuscripciones();
		//ConfigurarEventos();
	}

	private void ConfigurarSuscripciones() {
		_btnOpen.Clicked += OnOpenClicked;
		_viewModel.OnDataLoaded += () => {
			MainThread.BeginInvokeOnMainThread(() => {
				SincronizarUi();
			});
		};
	}

	///<summary>Configura los manejadores de eventos y suscripciones al ViewModel</summary>
	private void ConfigurarEventos() {
		// Enlace manual del clic del boton
		_btnOpen.Clicked += OnOpenClicked;

		// Suscripcion a cambios en el ViewModel
		_viewModel.OnDataLoaded += () => {
			MainThread.BeginInvokeOnMainThread(() => {
				SincronizarUi();
			});
		};
	}

	///<summary>Gestiona la seleccion de archivos nativa y pasa el flujo al ViewModel</summary>
	private async void OnOpenClicked(object? sender, EventArgs e) {
		try {
			var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
				{ DevicePlatform.WinUI, new[] { ".scheck", ".txt" } },
				{ DevicePlatform.MacCatalyst, new[] { "public.text", "com.apple.ascii-art" } } // UTI para archivos de texto plano
            });

			var result = await FilePicker.Default.PickAsync(new PickOptions {
				PickerTitle = "Abrir documento CheckList",
				FileTypes = customFileType
			});

			if (result != null) {
				var stream = await result.OpenReadAsync();
				await _viewModel.LoadDataAsync(stream);
			}
		} catch (Exception ex) {
			await DisplayAlert("Error", "No se pudo abrir el archivo", "OK");
		} finally {
			_btnOpen.IsEnabled = true;
		}
	}

	///<summary>Actualiza la visibilidad de los componentes segun el estado</summary>
	private void SincronizarUi() {
		bool tieneDatos = _viewModel.TaskListViewModel.Rows.Count > 0;

		_taskListView.IsVisible = tieneDatos;
		_lblEmptyState.IsVisible = !tieneDatos;

		if (tieneDatos) {
			// Pasamos el ViewModel al componente de lista (asumiendo refactor de TaskListView)
			_taskListView.SetViewModel(_viewModel.TaskListViewModel);
		}

	}

	#endregion

}