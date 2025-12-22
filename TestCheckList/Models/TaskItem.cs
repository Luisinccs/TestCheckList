using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestCheckList.Models;

public class TaskItem : INotifyPropertyChanged {

	private string _title;
	private TaskState _state;
	private string _comment;

	// Original properties for parsing reconstruction if needed,
	// but here we just store the data.

	public string Title {
		get => _title;
		set { _title = value; OnPropertyChanged(); }
	}

	public TaskState State {
		get => _state;
		set { _state = value; OnPropertyChanged(); }
	}

	public string Comment {
		get => _comment;
		set { _comment = value; OnPropertyChanged(); }
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string name = null) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}