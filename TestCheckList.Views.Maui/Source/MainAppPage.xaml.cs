// 2025-12-23

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
				// Suscribirse a los cambios de propiedades del ViewModel de la lista
				_viewModel.TaskListViewModel.OnPropertyChanged += (prop) => {
					MainThread.BeginInvokeOnMainThread(() => SincronizarUi(prop));
				};
				SincronizarUi();
			});
		};
	}	

	///<summary>Gestiona la seleccion de archivos nativa y pasa el flujo al ViewModel</summary>
	private async void OnOpenClicked(object? sender, EventArgs e) {
		try {
			_btnOpen.IsEnabled = false; // Bloqueamos para evitar doble ejecucion
			System.Diagnostics.Debug.WriteLine($"OnOpenCLicked. Plataforma: ");
			var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
				{ DevicePlatform.WinUI, new[] { ".scheck", ".txt" } },
				{ DevicePlatform.MacCatalyst, new[] { "public.text", "public.data", "public.item" } } // UTI para archivos de texto plano
            });

			var result = await FilePicker.Default.PickAsync(new PickOptions {
				PickerTitle = "Abrir documento CheckList",
				FileTypes = customFileType
			});

			if (result != null) {
				Console.WriteLine($"DEBUG: Archivo seleccionado: {result.FileName}");
				System.Diagnostics.Debug.WriteLine($"Archivo seleccionado: {result.FileName}");
				var stream = await result.OpenReadAsync();
				await _viewModel.LoadDataAsync(stream, result.FullPath);
			} else {
				Console.WriteLine($"DEBUG: Archivo seleccionado: nulo");
			}
		} catch (Exception ex) {
			System.Diagnostics.Debug.WriteLine($"AExcepcion: {ex}");
			await DisplayAlert("Error", "No se pudo abrir el archivo", "OK");
		} finally {
			_btnOpen.IsEnabled = true;
		}
	}

	///<summary>Actualiza la visibilidad de los componentes segun el estado</summary>
	private void SincronizarUi(string? propertyName = null) {
		
		
		int count = _viewModel.TaskListViewModel.Rows.Count;
		System.Diagnostics.Debug.WriteLine($"Sincronizando UI. Filas encontradas: {count}");

		bool tieneDatos = count > 0;
		_taskListView.IsVisible = tieneDatos;
		_lblEmptyState.IsVisible = !tieneDatos;

		if (tieneDatos) {
			// Pasamos el ViewModel al componente de lista (asumiendo refactor de TaskListView)
			_taskListView.SetViewModel(_viewModel.TaskListViewModel);
		}

		if (propertyName == nameof(ITaskListViewModel.IsSaving)) {
			ActualizarEstadoGuardado();
		}

	}

	///<summary>Realiza una transicion suave del indicador de guardado</summary>
	private async void ActualizarEstadoGuardado() {
		bool isSaving = _viewModel.TaskListViewModel.IsSaving;

		// Animacion de opacidad para no ser abruptos
		if (isSaving) {
			await _lytSavingIndicator.FadeTo(1, 250, Easing.CubicIn);
		} else {
			await _lytSavingIndicator.FadeTo(0, 500, Easing.CubicOut);
		}
	}

	#endregion

}