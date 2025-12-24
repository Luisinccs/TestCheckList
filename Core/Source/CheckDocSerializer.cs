using System.Text;
using TestCheckList.Models;

namespace TestCheckList.Parsers;

public class CheckDocSerializer {
	public string Serialize(Document doc) {
		var sb = new StringBuilder();

		// Metadata
		if (doc.Metadata.Count > 0) {
			sb.AppendLine("@metadatos");
			foreach (var kvp in doc.Metadata) {
				sb.AppendLine($"{kvp.Key}: {kvp.Value}");
			}
			sb.AppendLine("@endmetadatos");
			sb.AppendLine();
		}

		// Tasks
		foreach (var task in doc.Tasks) {
			sb.AppendLine("@task");
			sb.AppendLine($"@title {task.Title}");

			string stateStr = task.State switch {
				TaskState.Success => "success",
				TaskState.Failed => "failed",
				_ => "pendent", // using 'pendent' to match user example if strictly required, or 'pending'. User example said 'pendent'.
			};
			sb.AppendLine($"@state {stateStr}");

			if (!string.IsNullOrEmpty(task.Comment)) {
				sb.AppendLine($"@comment {task.Comment}");
			}

			sb.AppendLine("@endtask");
			sb.AppendLine();
		}

		return sb.ToString();
	}

	public async Task SaveAsync(Document doc) {
		if (string.IsNullOrEmpty(doc.FilePath))
			return;
		var content = Serialize(doc);
		await File.WriteAllTextAsync(doc.FilePath, content);
	}
}

