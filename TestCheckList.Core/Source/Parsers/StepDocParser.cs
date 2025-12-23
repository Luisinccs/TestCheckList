using System.Collections.ObjectModel;
using TestCheckList.Models;

namespace TestCheckList.Parsers;

public class StepDocParser
{
	public Document Parse(string content, string filePath)
	{
		var doc = new Document
		{
			FilePath = filePath,
			Tasks = new ObservableCollection<TaskItem>(),
		};

		if (string.IsNullOrWhiteSpace(content))
		{
			Console.WriteLine("Parser: Content is empty.");
			return doc;
		}

		using var reader = new StringReader(content);
		string? line;

		// Simple state machine
		// 0 = Metadata/Header
		// 1 = Steps
		int state = 0;

		while ((line = reader.ReadLine()) != null)
		{
			if (string.IsNullOrWhiteSpace(line))
				continue;

			line = line.Trim();

			// Detect Section Changes
			if (line.StartsWith("@Metadata", StringComparison.OrdinalIgnoreCase))
			{
				state = 0;
				continue;
			}
			if (line.StartsWith("@Steps", StringComparison.OrdinalIgnoreCase))
			{
				state = 1;
				continue;
			}

			if (state == 0) // Metadata Parsing
			{
				var parts = line.Split('=', 2);
				if (parts.Length == 2)
				{
					if (doc.Metadata == null)
						doc.Metadata = new Dictionary<string, string>();
					doc.Metadata[parts[0].Trim()] = parts[1].Trim();
				}
			}
			else if (state == 1) // Tasks Parsing
			{
				// Format: [State] Title | Comment
				// Ex: [Pending] Verify Login | Check that login works

				var item = new TaskItem();

				// 1. Parse State [Pending]
				if (line.StartsWith("[") && line.Contains("]"))
				{
					int closeBracket = line.IndexOf(']');
					string stateStr = line.Substring(1, closeBracket - 1);
					if (Enum.TryParse<TaskState>(stateStr, true, out var ts))
					{
						item.State = ts;
					}
					else
					{
						// Fallback or log?
						item.State = TaskState.Pending;
					}

					if (line.Length > closeBracket + 1)
						line = line.Substring(closeBracket + 1).Trim();
					else
						line = "";
				}
				else
				{
					// No state tag? Default to Pending
					item.State = TaskState.Pending;
				}

				// 2. Parse Title | Comment
				// line is now "Title | Comment"
				var parts = line.Split('|', 2);
				item.Title = parts[0].Trim();

				if (parts.Length > 1)
				{
					item.Comment = parts[1].Trim();
				}

				doc.Tasks.Add(item);
			}
		}

		Console.WriteLine($"Parser: Finished. Found {doc.Tasks.Count} tasks.");
		return doc;
	}
}

