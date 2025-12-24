using System.Collections.ObjectModel;

namespace TestCheckList.Models;

public class Document {
	
	public Dictionary<string, string> Metadata { get; set; } = new();
	public ObservableCollection<TaskItem> Tasks { get; set; } = new();
	public string? FilePath { get; set; }

}