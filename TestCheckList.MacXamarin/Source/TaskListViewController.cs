// 2025-12-23
using AppKit;
using CoreGraphics;
using Foundation;
using System;
using TestCheckList.Models;
using TestCheckList.ViewModels;

namespace TestCheckList.Views.Mac;

///<summary>Tabla nativa personalizada para capturar eventos de teclado especificos</summary>
public class TaskTableView : NSTableView {

	#region Funciones internas
	public override void KeyDown(NSEvent theEvent) {
		// Notificamos al delegado sobre la tecla presionada antes de procesar el comportamiento base
		if (Delegate is TaskListViewController controller) {
			if (controller.ProcesarTeclado(theEvent)) {
				return; // Si el controlador manejo la tecla (S, F, D, Enter), detenemos la propagacion
			}
		}
		base.KeyDown(theEvent);
	}

	///<summary>Asegura que la tabla pueda recibir el foco del teclado</summary>
	public override bool CanBecomeKeyView => true;
	#endregion

	#region Funciones Externas
	public TaskTableView() { }
	public TaskTableView(IntPtr handle) : base(handle) { }
	#endregion
}

///<summary>Controlador de vista nativo que encapsula una NSTableView y gestiona su comportamiento</summary>
public class TaskListViewController : NSViewController, INSTableViewDataSource, INSTableViewDelegate {

	#region Variables

	private ITaskListViewModel? _viewModel;
	private TaskTableView _tableView = new();
	private NSScrollView _scrollView = new();
	private const string _cellId = "FilaPasoCell";
	
	#endregion

	#region Funciones internas

	///<summary>Carga la jerarquia de vistas nativas y configura el layout de la tabla</summary>
	public override void LoadView() {

		_scrollView = new NSScrollView {
			HasVerticalScroller = true,
			AutohidesScrollers = true,
			DrawsBackground = false,
			TranslatesAutoresizingMaskIntoConstraints = false
		};

		_tableView = new TaskTableView {
			HeaderView = null,
			BackgroundColor = NSColor.Clear,
			DataSource = this,
			Delegate = this,
			SelectionHighlightStyle = NSTableViewSelectionHighlightStyle.None,
			TranslatesAutoresizingMaskIntoConstraints = false
		};

		// Configuracion de la columna principal unica para el listado
		NSTableColumn mainColumn = new(_cellId) {
			ResizingMask = NSTableColumnResizing.Autoresizing
		};
		_tableView.AddColumn(mainColumn);

		_scrollView.DocumentView = _tableView;
		View = _scrollView;
	}

	///<summary>Obtiene la cantidad de filas desde el ViewModel del Core</summary>
	[Export("numberOfRowsInTableView:")]
	public nint GetRowCount(NSTableView tableView) {
		return _viewModel?.Rows.Count ?? 0;
	}

	///<summary>Provee la celda nativa vinculada al ViewModel de la fila correspondiente</summary>
	[Export("tableView:viewForTableColumn:row:")]
	public NSView GetViewForItem(NSTableView tableView, NSTableColumn column, nint row) {
		if (tableView.MakeView(_cellId, this) is not FilaPasoCellView cell) {
			cell = new FilaPasoCellView(CGRect.Empty) {
				Identifier = _cellId
			};
		}

		if (_viewModel != null && (int)row < _viewModel.Rows.Count) {
			cell.SetViewModel(_viewModel.Rows[(int)row]);
			// Aplicar resaltado inicial basado en la seleccion actual
			cell.ActualizarResaltado(row == tableView.SelectedRow);
		}

		return cell;
	}

	///<summary>Calcula la altura de la fila basandose en el estado de expansion del ViewModel</summary>
	[Export("tableView:heightOfRow:")]
	public nfloat GetRowHeight(NSTableView tableView, nint row) {
		if (_viewModel != null && (int)row < _viewModel.Rows.Count) {
			var rowVm = _viewModel.Rows[(int)row];
			// Altura base 40px, expandida 86px si existe comentario [cite: 12]
			return string.IsNullOrEmpty(rowVm.Comentario) ? 40 : 86;
		}
		return 40;
	}

	///<summary>Se dispara al cambiar la seleccion para refrescar los bordes de las celdas</summary>
	[Export("tableViewSelectionDidChange:")]
	public void SelectionDidChange(NSNotification notification) {
		// Recorremos solo las filas visibles para optimizar rendimiento
		for (nint i = 0; i < _tableView.RowCount; i++) {
			// false indica que no cree la vista si no esta visible en pantalla
			var cell = _tableView.GetView(0, i, false) as FilaPasoCellView;
			cell?.ActualizarResaltado(i == _tableView.SelectedRow);
		}
	}

	///<summary>Mueve la seleccion a la siguiente fila automaticamente para agilizar la carga</summary>
	private void MoverSiguienteFila() {
		nint nextRow = _tableView.SelectedRow + 1;
		if (nextRow < _tableView.RowCount) {
			_tableView.SelectRow(nextRow, false);
			_tableView.ScrollRowToVisible(nextRow);
		}
	}

	#endregion

	#region Funciones Externas

	///<summary>Captura las teclas S, F y D para cambiar el estado de la tarea seleccionada</summary>
	public bool ProcesarTeclado(NSEvent theEvent) {
		nint selectedRow = _tableView.SelectedRow;
		if (selectedRow < 0 || _viewModel == null) return false;

		var rowVm = _viewModel.Rows[(int)selectedRow];
		string key = theEvent.CharactersIgnoringModifiers.ToLower();

		switch (key) {
			case "s":
				rowVm.State = TaskState.Success;
				MoverSiguienteFila();
				return true;
			case "f":
				rowVm.State = TaskState.Failed;
				MoverSiguienteFila();
				return true;
			case "d":
				rowVm.State = TaskState.Pending;
				MoverSiguienteFila();
				return true;
		}
		return false;
	}

	///<summary>Asigna el ViewModel de la lista y actualiza el renderizado nativo</summary>
	public void SetViewModel(ITaskListViewModel viewModel) {
		_viewModel = viewModel;

		// Nos aseguramos de que la tabla se refresque en el hilo principal
		InvokeOnMainThread(() => {
			_tableView?.ReloadData();
		});
	}

	#endregion

}