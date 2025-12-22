namespace Kodeks.Maui.Views;

/// <summary>
/// Defines a contract for views that can handle keyboard navigation events manually.
/// </summary>
public interface IKeyboardNavigable {
	/// <summary>
	/// Handles arrow key navigation.
	/// </summary>
	/// <param name="direction">"UP", "DOWN", "LEFT", "RIGHT"</param>
	/// <param name="direction">"UP", "DOWN", "LEFT", "RIGHT"</param>
	void HandleArrowKey(string direction);

	/// <summary>
	/// Handles character input key.
	/// </summary>
	void HandleCharKey(string character);
}

