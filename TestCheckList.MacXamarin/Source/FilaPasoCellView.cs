// 2025-12-23
using System;
using AppKit;
using CoreGraphics;
using Foundation;
using TestCheckList.Models;
using TestCheckList.ViewModels;

namespace TestCheckList.Views.Mac;

///<summary>Celda nativa para representar un paso del checklist utilizando AppKit</summary>
public class FilaPasoCellView : NSTableCellView {

	#region Variables

	private IFilaPasoViewModel? _viewModel;
	private readonly NSImageView _checkIcon = new();
	private readonly NSTextField _lblTitulo = new();
	private readonly NSTextField _lblComentario = new();
	private readonly NSTextField _txtComentario = new();
	private readonly NSStackView _mainStack = new();
	private readonly NSStackView _headerStack = new();
	private readonly NSStackView _commentStack = new();

	#endregion

	#region Funciones internas

	///<summary>Inicializa la jerarquia de vistas y aplica restricciones de Auto Layout</summary>
	private void InicializarComponentes() {
		
		WantsLayer = true;
		_mainStack.Orientation = NSUserInterfaceLayoutOrientation.Vertical;
		_mainStack.Alignment = NSLayoutAttribute.Leading;
		_mainStack.Spacing = 5;
		_mainStack.TranslatesAutoresizingMaskIntoConstraints = false;

		ConfigurarHeader();
		ConfigurarEditorComentario();

		_mainStack.AddArrangedSubview(_headerStack);
		_mainStack.AddArrangedSubview(_commentStack);
		AddSubview(_mainStack);

		NSLayoutConstraint.ActivateConstraints(new[] {
			_mainStack.TopAnchor.ConstraintEqualToAnchor(TopAnchor, 5),
			_mainStack.BottomAnchor.ConstraintEqualToAnchor(BottomAnchor, -5),
			_mainStack.LeadingAnchor.ConstraintEqualToAnchor(LeadingAnchor, 10),
			_mainStack.TrailingAnchor.ConstraintEqualToAnchor(TrailingAnchor, -10),
			_checkIcon.WidthAnchor.ConstraintEqualToConstant(24),
			_checkIcon.HeightAnchor.ConstraintEqualToConstant(24)
		});
	}

	///<summary>Configura la fila superior con el titulo y el icono de estado</summary>
	private void ConfigurarHeader() {
		_headerStack.Orientation = NSUserInterfaceLayoutOrientation.Horizontal;
		_headerStack.Alignment = NSLayoutAttribute.CenterY;
		_headerStack.Spacing = 10;

		_lblTitulo.Editable = false;
		_lblTitulo.Bordered = false;
		_lblTitulo.DrawsBackground = false;
		NSFont? font = NSFont.SystemFontOfSize(16);
		if(font is not null)
			_lblTitulo.Font = font;
		_lblTitulo.TextColor = NSColor.FromRgb(51, 51, 51); // #333 [cite: 2025-12-13]

		_headerStack.AddArrangedSubview(_lblTitulo);
		_headerStack.AddArrangedSubview(_checkIcon);
	}

	///<summary>Configura la seccion expandible para comentarios</summary>
	private void ConfigurarEditorComentario() {
		_commentStack.Orientation = NSUserInterfaceLayoutOrientation.Horizontal;
		_commentStack.Spacing = 10;
		_commentStack.Hidden = true;

		_lblComentario.StringValue = "Comentario";
		_lblComentario.Editable = false;
		_lblComentario.Bordered = false;
		_lblComentario.DrawsBackground = true;
		_lblComentario.BackgroundColor = NSColor.FromRgb(68, 68, 68); // #444 [cite: 13]
		_lblComentario.TextColor = NSColor.White;

		_txtComentario.PlaceholderString = "Escriba aqui...";
		_txtComentario.BackgroundColor = NSColor.FromRgb(48, 48, 48); // #303030 [cite: 14]
		_txtComentario.TextColor = NSColor.White;

		_commentStack.AddArrangedSubview(_lblComentario);
		_commentStack.AddArrangedSubview(_txtComentario);
	}

	///<summary>Sincroniza los datos del ViewModel con los controles de AppKit</summary>
	private void SincronizarUi() {
		if (_viewModel == null) return;

		_lblTitulo.StringValue = _viewModel.Titulo ?? string.Empty;
		_txtComentario.StringValue = _viewModel.Comentario ?? string.Empty;
		_commentStack.Hidden = string.IsNullOrEmpty(_viewModel.Comentario);

		string iconName = _viewModel.State switch {
			TaskState.Success => "check_success",
			TaskState.Failed => "check_failed",
			_ => "check_blank"
		};
		_checkIcon.Image = NSImage.ImageNamed(iconName);
		if (_checkIcon.Image is null)
			throw new Exception($"Null Icon {iconName}");
			//_checkIcon.Image.Template = true; // Asegura que se trate como plantilla
		//_checkIcon.ContentTintColor = NSColor.FromRgb(69, 123, 157);
	}

	#endregion

	#region Funciones Externas

	public FilaPasoCellView(IntPtr handle) : base(handle) {
		InicializarComponentes();
	}

	[Export("initWithFrame:")]
	public FilaPasoCellView(CGRect frameRect) : base(frameRect) {
		InicializarComponentes();
	}

	///<summary>Aplica un borde de resaltado si la fila tiene el foco/seleccion</summary>
	public void ActualizarResaltado(bool enfocado) {
		if (Layer is null)
			throw new Exception($"Null Layer in FilaPasoCellView");
		if (enfocado) {
			Layer.BorderColor = NSColor.FromRgb(69, 123, 157).CGColor; // #457b9d [cite: 2025-12-13]
			Layer.BorderWidth = 3;
		} else {
			Layer.BorderWidth = 0;
		}
	}

	///<summary>Asigna el ViewModel y suscribe la actualizacion de la celda</summary>
	public void SetViewModel(IFilaPasoViewModel vm) {
		_viewModel = vm;
		_viewModel.OnPropertyChanged += (prop) => {
			InvokeOnMainThread(() => SincronizarUi());
		};
		SincronizarUi();
	}

	#endregion

}