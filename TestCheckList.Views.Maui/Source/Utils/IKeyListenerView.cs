// 2025-12-16

/// <summary></summary>
/// <param name="Characters"></param>
/// <param name="KeyCode"></param>
/// <param name="ModifierFlags"></param>
public record struct KeyPressedInfo(string Characters, long KeyCode, long ModifierFlags );

/// <summary></summary>
public interface IKeyListenerView {

	Action<KeyPressedInfo>? OnKeyPressed {get; set;}

	void SetupHandler(IViewHandler? pageHandler);

	Action? Focusing { get; set; }

	Action? Unfocusing { get; set; }

	void Focus();

}