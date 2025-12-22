using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TestCheckList.Interfaces;
using TestCheckList.Models;

namespace TestCheckList.ViewModels;

public partial class FilaTaskViewModel : ObservableObject, IFilaTaskViewModel {

	private readonly Action _onSaveRequest;
	private readonly Action<FilaTaskViewModel> _onRequestNext;

	[ObservableProperty]
	private TaskItem _task;

	[ObservableProperty]
	private bool _isFocused;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsCommentVisible))]
	private bool _isEditing;

	[ObservableProperty]
	private bool _isEven;

	public bool IsCommentVisible => IsEditing || !string.IsNullOrWhiteSpace(Task.Comment);

	public string StateIcon =>
		Task.State switch {
			TaskState.Success => "check_success.svg",
			TaskState.Failed => "check_failed.svg",
			_ => "check_blank.svg",
		};

	public FilaTaskViewModel(
		TaskItem task,
		Action onSaveRequest,
		Action<FilaTaskViewModel> onRequestNext
	) {
		_task = task;
		_onSaveRequest = onSaveRequest;
		_onRequestNext = onRequestNext;

		_task.PropertyChanged += (s, e) => {
			if (e.PropertyName == nameof(TaskItem.State)) {
				OnPropertyChanged(nameof(StateIcon));
			}
			if (e.PropertyName == nameof(TaskItem.Comment)) {
				OnPropertyChanged(nameof(IsCommentVisible));
			}
		};
	}

	[RelayCommand]
	private void Success() {
		if (IsEditing)
			return;
		Task.State = TaskState.Success;
		_onSaveRequest?.Invoke();
		_onRequestNext?.Invoke(this);
	}

	[RelayCommand]
	private void Failed() {
		if (IsEditing)
			return;
		Task.State = TaskState.Failed;
		StartEditing();
	}

	[RelayCommand]
	private void EditSuccess() {
		if (IsEditing)
			return;
		Task.State = TaskState.Success;
		_onSaveRequest?.Invoke();
		StartEditing();
	}

	[RelayCommand]
	private void Pending() {
		if (IsEditing)
			return;
		Task.State = TaskState.Pending;
		_onSaveRequest?.Invoke();
		_onRequestNext?.Invoke(this);
	}

	[RelayCommand]
	private void EnterEdit() {
		if (IsEditing)
			return;
		StartEditing();
	}

	[RelayCommand]
	private void SaveEdit() {
		IsEditing = false;
		_onSaveRequest?.Invoke();
		OnPropertyChanged(nameof(IsCommentVisible));
		_onRequestNext?.Invoke(this);
	}

	[RelayCommand]
	private void CancelEdit() {
		IsEditing = false;
		OnPropertyChanged(nameof(IsCommentVisible));
	}

	private void StartEditing() {
		IsEditing = true;
		OnPropertyChanged(nameof(IsCommentVisible));
	}
}

