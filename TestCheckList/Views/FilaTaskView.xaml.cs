using System.ComponentModel;
using Microsoft.Maui.Controls;
using TestCheckList.Interfaces;

namespace TestCheckList.Views;

public partial class FilaTaskView : ContentView {
	public FilaTaskView() {
		InitializeComponent();
		this.BindingContextChanged += OnBindingContextChanged;
	}

	private void OnBindingContextChanged(object sender, EventArgs e) {
		if (BindingContext is IFilaTaskViewModel vm) {
			vm.PropertyChanged -= OnViewModelPropertyChanged;
			vm.PropertyChanged += OnViewModelPropertyChanged;
		}
	}

	private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) {
		if (e.PropertyName == "IsEditing") {
			if (BindingContext is IFilaTaskViewModel vm && vm.IsEditing) {
				// Find Editor and focus
				// Since this is a simple template, we can traverse or name the element.
				// In XAML I didn't verify x:Name. I should add x:Name to Editor.
				// But I can find it by type.
				var editor = this.GetVisualTreeDescendants().OfType<Editor>().FirstOrDefault();
				editor?.Focus();
			}
		}
	}
}

