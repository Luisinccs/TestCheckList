using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using TestCheckList.Models;

namespace TestCheckList.Interfaces;

public interface IFilaTaskViewModel : INotifyPropertyChanged
{
	TaskItem Task { get; }
	bool IsFocused { get; set; }
	bool IsEditing { get; set; }
	IRelayCommand SuccessCommand { get; }
	IRelayCommand FailedCommand { get; }
	IRelayCommand PendingCommand { get; } // 'D' key
	IRelayCommand EditSuccessCommand { get; } // 'E' key
	IRelayCommand EnterEditCommand { get; }
	IRelayCommand CancelEditCommand { get; }
	IRelayCommand SaveEditCommand { get; }
}

public interface ITaskListViewModel : INotifyPropertyChanged
{
	ObservableCollection<IFilaTaskViewModel> Rows { get; }
	IFilaTaskViewModel SelectedRow { get; set; }
	void LoadTasks(IEnumerable<TaskItem> tasks);
	void MoveFocusNext();
	void MoveFocusPrevious();
	void RestartTasks();
}

public interface IMainAppViewModel : INotifyPropertyChanged
{
	ITaskListViewModel TaskList { get; }
	bool IsFileLoaded { get; }
	IAsyncRelayCommand OpenFileCommand { get; }

	// Keyboard Shortcuts Proxies
	IRelayCommand MarkSuccessCommand { get; }
	IRelayCommand MarkFailedCommand { get; }
	IRelayCommand MarkPendingCommand { get; }
	IRelayCommand MarkEditSuccessCommand { get; }
	IRelayCommand EnterEditCommand { get; }
	IRelayCommand CancelEditCommand { get; }
}

