// 2025-12-23
using AppKit;
using CoreGraphics;
using Foundation;
using System;
using System.IO;
using TestCheckList.ViewModels;

namespace TestCheckList.Views.Mac;

///<summary>Controlador principal con carga automatica de archivo para pruebas en macOS</summary>
public class MainViewController : NSViewController {

	#region Variables
	private readonly IMainAppPageViewModel _viewModel;
	private readonly TaskListViewController _listController = new();
	private readonly NSView _headerView = new();
	private readonly NSStackView _lytSavingIndicator = new();
	private readonly NSButton _btnOpen;
	private readonly NSTextField _lblEmptyState;
	#endregion

	#region Funciones internas
	///<summary>Configura la jerarquia de vistas e inicia la carga de prueba</summary>
	public override void LoadView() {
		View = new NSView(new CGRect(0, 0, 800, 600)) {
			WantsLayer = true
		};
		if (View.Layer is not null) {
			View.Layer.BackgroundColor = NSColor.FromRgb(248, 249, 250).CGColor;
		}

		ConfigurarHeader();
		ConfigurarContenido();
		ConfigurarConstraints();

		// Iniciamos la carga asincrona sin bloquear el hilo principal de la UI
		CargarArchivoPruebaAsync();
	}

	///<summary>Metodo temporal para bypass del picker durante desarrollo</summary>
	private async void CargarArchivoPruebaAsync() {
		var path = "/Users/luis_sonoma/Desktop/testcopy.scheck";

		if (File.Exists(path)) {
			try {
				using var stream = File.OpenRead(path);
				// Esperamos a que el Core termine de parsear las tareas
				await _viewModel.LoadDataAsync(stream, path);

				InvokeOnMainThread(() => {
					_listController.View.Hidden = false;
					_lblEmptyState.Hidden = true;
					_listController.SetViewModel(_viewModel.TaskListViewModel);

					// Seleccionamos la primera fila y damos foco a la tabla
					// CORRECCION: Casteamos View a NSScrollView para llegar a la tabla
					var scrollView = _listController.View as NSScrollView;
					if (scrollView?.DocumentView is NSTableView tableView) {
						tableView.SelectRow(0, false);
						View.Window?.MakeFirstResponder(tableView);
					}
				});
			} catch (Exception ex) {
				Console.WriteLine($"DEBUG: Error carga automatica: {ex.Message}");
			}
		} else {
			Console.WriteLine("DEBUG: No se encontro el archivo de prueba en el Desktop");
		}
	}

	///<summary>Crea la barra superior con el estilo gris oscuro</summary>
	private void ConfigurarHeader() {
		_headerView.WantsLayer = true;
		if (_headerView.Layer is not null) {
			_headerView.Layer.BackgroundColor = NSColor.FromRgb(51, 51, 51).CGColor;
		}
		_headerView.TranslatesAutoresizingMaskIntoConstraints = false;

		_btnOpen.BezelStyle = NSBezelStyle.TexturedRounded;
		_btnOpen.WantsLayer = true;
		if (_btnOpen.Layer is not null) {
			_btnOpen.Layer.BackgroundColor = NSColor.FromRgb(69, 123, 157).CGColor;
		}
		_btnOpen.TranslatesAutoresizingMaskIntoConstraints = false;

		ConfigurarIndicadorGuardado();

		_headerView.AddSubview(_btnOpen);
		_headerView.AddSubview(_lytSavingIndicator);
		View.AddSubview(_headerView);
	}

	///<summary>Configura los componentes visuales del indicador de guardado</summary>
	private void ConfigurarIndicadorGuardado() {
		_lytSavingIndicator.Orientation = NSUserInterfaceLayoutOrientation.Horizontal;
		_lytSavingIndicator.Spacing = 8;
		_lytSavingIndicator.AlphaValue = 0;
		_lytSavingIndicator.TranslatesAutoresizingMaskIntoConstraints = false;

		NSTextField lblSaving = NSTextField.CreateLabel("Guardando...");
		lblSaving.TextColor = NSColor.FromRgb(69, 123, 157);

		NSProgressIndicator spinner = new(new CGRect(0, 0, 16, 16)) {
			Style = NSProgressIndicatorStyle.Spinning,
			ControlSize = NSControlSize.Small
		};
		spinner.StartAnimation(this);

		_lytSavingIndicator.AddArrangedSubview(lblSaving);
		_lytSavingIndicator.AddArrangedSubview(spinner);
	}

	///<summary>Inicializa el controlador de lista nativo</summary>
	private void ConfigurarContenido() {
		_listController.View.TranslatesAutoresizingMaskIntoConstraints = false;
		_listController.View.Hidden = true;

		_lblEmptyState.Alignment = NSTextAlignment.Center;
		_lblEmptyState.TextColor = NSColor.FromRgb(51, 51, 51);
		_lblEmptyState.TranslatesAutoresizingMaskIntoConstraints = false;

		AddChildViewController(_listController);
		View.AddSubview(_listController.View);
		View.AddSubview(_lblEmptyState);
	}

	private void ConfigurarConstraints() {
		NSLayoutConstraint.ActivateConstraints(new[] {
			_headerView.TopAnchor.ConstraintEqualToAnchor(View.TopAnchor),
			_headerView.LeadingAnchor.ConstraintEqualToAnchor(View.LeadingAnchor),
			_headerView.TrailingAnchor.ConstraintEqualToAnchor(View.TrailingAnchor),
			_headerView.HeightAnchor.ConstraintEqualToConstant(60),

			_btnOpen.CenterYAnchor.ConstraintEqualToAnchor(_headerView.CenterYAnchor),
			_btnOpen.TrailingAnchor.ConstraintEqualToAnchor(_headerView.TrailingAnchor, -20),
			_btnOpen.WidthAnchor.ConstraintEqualToConstant(80),

			_lytSavingIndicator.CenterYAnchor.ConstraintEqualToAnchor(_headerView.CenterYAnchor),
			_lytSavingIndicator.TrailingAnchor.ConstraintEqualToAnchor(_btnOpen.LeadingAnchor, -20),

			_listController.View.TopAnchor.ConstraintEqualToAnchor(_headerView.BottomAnchor),
			_listController.View.LeadingAnchor.ConstraintEqualToAnchor(View.LeadingAnchor),
			_listController.View.TrailingAnchor.ConstraintEqualToAnchor(View.TrailingAnchor),
			_listController.View.BottomAnchor.ConstraintEqualToAnchor(View.BottomAnchor),

			_lblEmptyState.CenterXAnchor.ConstraintEqualToAnchor(View.CenterXAnchor),
			_lblEmptyState.CenterYAnchor.ConstraintEqualToAnchor(View.CenterYAnchor)
		});
	}

	///<summary>Anima la opacidad del indicador segun el estado IsSaving del Core</summary>
	private void SincronizarUi(string? propertyName) {
		if (propertyName == nameof(ITaskListViewModel.IsSaving)) {
			bool isSaving = _viewModel.TaskListViewModel.IsSaving;
			NSAnimationContext.RunAnimation((context) => {
				context.Duration = 0.5;
				//_lytSavingIndicator.Animator.AlphaValue = isSaving ? 1 : 0;
			});
		}
	}

	private async void OnOpenClicked() {
		NSOpenPanel panel = new() {
			Title = "Abrir documento .scheck",
			AllowedFileTypes = new[] { "scheck", "txt" },
			CanChooseFiles = true
		};

		if (panel.RunModal() == 1 && panel.Url?.Path is not null) {
			string path = panel.Url.Path;
			using var stream = File.OpenRead(path);
			await _viewModel.LoadDataAsync(stream, path);

			_listController.View.Hidden = false;
			_lblEmptyState.Hidden = true;
			_listController.SetViewModel(_viewModel.TaskListViewModel);
		}
	}
	#endregion

	#region Funciones Externas
	public MainViewController(IMainAppPageViewModel viewModel) {
		_btnOpen = NSButton.CreateButton("Abrir", OnOpenClicked);
		_lblEmptyState = NSTextField.CreateLabel("Presione abrir para cargar un archivo");

		_viewModel = viewModel;
		// Suscripcion a cambios en el ViewModel para el indicador de guardado
		_viewModel.TaskListViewModel.OnPropertyChanged += (prop) => InvokeOnMainThread(() => SincronizarUi(prop));
	}
	#endregion

}