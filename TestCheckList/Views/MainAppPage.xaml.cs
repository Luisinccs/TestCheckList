using Microsoft.Maui.Controls;
using TestCheckList.ViewModels;

namespace TestCheckList.Views;

public partial class MainAppPage : ContentPage {
	
	private readonly MainAppViewModel _viewModel;

	public MainAppPage(MainAppViewModel viewModel) {
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	private async void OnDrop(object sender, DropEventArgs e) {
		Console.WriteLine("OnDrop triggered.");
		try {
			// Log all properties to find where the file path is hiding
			if (e.Data?.Properties != null) {
				foreach (var prop in e.Data.Properties) {
					Console.WriteLine($"Drop Prop: {prop.Key} = {prop.Value}");
				}
			}

			// 1. Text/Uri check
			var text = await e.Data.GetTextAsync();
			Console.WriteLine($"Drop GetTextAsync: {text}");

			if (!string.IsNullOrEmpty(text)) {
				if (text.StartsWith("file://"))
					text = new Uri(text).LocalPath;

				if (File.Exists(text)) {
					Console.WriteLine($"Found valid file: {text}");
					await _viewModel.LoadFileAsync(text);
					return;
				}
			}

			// Fallback for Mac: sometimes it's just implicit?
			// Or maybe we need to cast to Platform specific
		} catch (Exception ex) {
			Console.WriteLine($"Drop error: {ex}");
		}
	}

}

