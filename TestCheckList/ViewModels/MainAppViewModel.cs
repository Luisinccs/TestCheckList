using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TestCheckList.Interfaces;
using TestCheckList.Models;
using TestCheckList.Parsers;

namespace TestCheckList.ViewModels;

public partial class MainAppViewModel : ObservableObject, IMainAppViewModel {

	private readonly StepDocParser _parser;
	private readonly CheckDocSerializer _serializer;

	[ObservableProperty]
	private ITaskListViewModel _taskList;

	[ObservableProperty]
	private bool _isFileLoaded;

	[ObservableProperty]
	private string _currentFilePath;

	[ObservableProperty]
	private string _windowTitle = "TestCheckList";

	private Document _currentDocument;

	public MainAppViewModel() {
		_parser = new StepDocParser();
		_serializer = new CheckDocSerializer();
		TaskList = new TaskListViewModel(SaveCurrentDocument);
	}

	public async Task LoadFileAsync(string filePath) {
		Console.WriteLine($"LoadFileAsync called with: {filePath}");
		if (!File.Exists(filePath)) {
			Console.WriteLine("File does not exist.");
			return;
		}

		try {
			Console.WriteLine("Reading file text...");
			string content = await File.ReadAllTextAsync(filePath);
			Console.WriteLine($"File read. Length: {content.Length}");

			_currentDocument = _parser.Parse(content, filePath);
			Console.WriteLine($"Parsed document. Tasks: {_currentDocument.Tasks.Count}");

			CurrentFilePath = filePath;
			WindowTitle = $"TestCheckList - {Path.GetFileName(filePath)}";

			// Ensure UI update on Main Thread
			MainThread.BeginInvokeOnMainThread(() => {
				TaskList.LoadTasks(_currentDocument.Tasks);
				IsFileLoaded = true;
				Console.WriteLine("UI Updated.");
			});
		} catch (Exception ex) {
			Console.WriteLine($"Error loading file: {ex}");
		}
	}

	[RelayCommand]
	private async Task OpenFile() {
		Console.WriteLine("Command OpenFile triggered.");
		try {
			var result = await FilePicker.Default.PickAsync(
				new PickOptions { PickerTitle = "Select a .scheck file" }
			);

			if (result != null) {
				await LoadFileAsync(result.FullPath);
			}
		} catch (Exception ex) {
			Console.WriteLine($"Picker error: {ex.Message}");
		}
	}

	[RelayCommand]
	private void Restart() {
		TaskList.RestartTasks();
	}

	private void SaveCurrentDocument() {
		if (_currentDocument == null || string.IsNullOrEmpty(_currentDocument.FilePath))
			return;

		_serializer
			.SaveAsync(_currentDocument)
			.ContinueWith(t => {
				if (t.IsFaulted)
					Console.WriteLine($"Save failed: {t.Exception}");
			});
	}

	// Proxy Commands for Keyboard Shortcuts

	[RelayCommand]
	private void MarkSuccess() => ExecuteOnSelected(vm => vm.SuccessCommand);

	[RelayCommand]
	private void MarkFailed() => ExecuteOnSelected(vm => vm.FailedCommand);

	[RelayCommand]
	private void MarkPending() => ExecuteOnSelected(vm => vm.PendingCommand);

	[RelayCommand]
	private void MarkEditSuccess() => ExecuteOnSelected(vm => vm.EditSuccessCommand);

	[RelayCommand]
	private void EnterEdit() => ExecuteOnSelected(vm => vm.EnterEditCommand);

	[RelayCommand]
	private void CancelEdit() => ExecuteOnSelected(vm => vm.CancelEditCommand);

	private void ExecuteOnSelected(Func<IFilaTaskViewModel, ICommand> commandSelector) {
		var selected = TaskList?.SelectedRow;
		if (selected != null) {
			var cmd = commandSelector(selected);
			if (cmd?.CanExecute(null) == true) {
				cmd.Execute(null);
			}
		}
	}
}

