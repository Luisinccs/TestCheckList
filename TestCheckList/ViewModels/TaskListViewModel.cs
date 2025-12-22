using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using TestCheckList.Interfaces;
using TestCheckList.Models;

namespace TestCheckList.ViewModels;

public partial class TaskListViewModel : ObservableObject, ITaskListViewModel
{
	private readonly Action _onSaveRequest;

	[ObservableProperty]
	private ObservableCollection<IFilaTaskViewModel> _rows = new();

	[ObservableProperty]
	private IFilaTaskViewModel _selectedRow; // Bound to UI

	private int _focusedIndex = -1;

	public TaskListViewModel(Action onSaveRequest)
	{
		_onSaveRequest = onSaveRequest;
	}

	public void LoadTasks(IEnumerable<TaskItem> tasks)
	{
		Rows.Clear();
		int i = 0;
		foreach (var task in tasks)
		{
			var vm = new FilaTaskViewModel(task, _onSaveRequest, OnRequestNext)
			{
				IsEven = i % 2 == 0,
			};
			Rows.Add(vm);
			i++;
		}

		if (Rows.Any())
		{
			SelectedRow = Rows[0]; // Logic triggers via PropertyChanged
		}
	}

	partial void OnSelectedRowChanged(IFilaTaskViewModel value)
	{
		// UI changed selection (User clicked or arrow keys)
		// OR logic changed selection
		if (value == null)
			return;

		var index = Rows.IndexOf(value);
		if (index >= 0)
		{
			UpdateFocusVisuals(index);
		}
	}

	public void RestartTasks()
	{
		foreach (var row in Rows)
		{
			row.Task.State = TaskState.Pending;
		}
		_onSaveRequest?.Invoke();
		if (Rows.Any())
			SelectedRow = Rows[0];
	}

	public void MoveFocusNext()
	{
		if (_focusedIndex < Rows.Count - 1)
		{
			SelectedRow = Rows[_focusedIndex + 1];
		}
	}

	public void MoveFocusPrevious()
	{
		if (_focusedIndex > 0)
		{
			SelectedRow = Rows[_focusedIndex - 1];
		}
	}

	private void OnRequestNext(FilaTaskViewModel sender)
	{
		int index = Rows.IndexOf(sender);
		if (index >= 0 && index < Rows.Count - 1)
		{
			SelectedRow = Rows[index + 1];
		}
	}

	private void UpdateFocusVisuals(int index)
	{
		if (_focusedIndex >= 0 && _focusedIndex < Rows.Count)
		{
			Rows[_focusedIndex].IsFocused = false;
		}

		_focusedIndex = index;

		if (_focusedIndex >= 0 && _focusedIndex < Rows.Count)
		{
			Rows[_focusedIndex].IsFocused = true;
		}
	}
}

