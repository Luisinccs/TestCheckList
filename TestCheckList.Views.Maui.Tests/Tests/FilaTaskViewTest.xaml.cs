namespace TestCheckList.Views.Maui.Tests;

public partial class FilaTaskViewTest : ContentPage {

	public FilaTaskViewTest() {
		InitializeComponent();

		Loaded += OnLoaded;
		_fila0.KeyPressed = (keyInfo) => {
			if (keyInfo.KeyCode == 81) {
				System.Diagnostics.Debug.WriteLine("Key down pressed on fila 00");
				_fila1.SetFocus();
			}
		};

	}

	private void OnLoaded(object? sender, EventArgs e) {
		//System.Diagnostics.Debug.WriteLine("FilaTaskViewTest Loaded");
		_fila0.SetFocus();
	}

}