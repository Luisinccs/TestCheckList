// 2025-12-16

#if MACCATALYST
using UIKit;
using Foundation;
using ObjCRuntime;
#endif

/// <summary></summary>
public class MacKeyListenerView : UIView, IKeyListenerView {

	public Action<KeyPressedInfo>? OnKeyPressed { get; set; }

	public Action? Focusing { get; set; }

	public Action? Unfocusing { get; set; }

	public MacKeyListenerView() {
		UserInteractionEnabled = true;
		BackgroundColor = UIColor.Clear; // Transparente pero existe
	}

	public void SetupHandler(IViewHandler? handler) {

		var platformView = handler?.PlatformView;

		if (platformView is UIView nativeView) {
			Frame = nativeView.Bounds;
			AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			nativeView.AddSubview(this);
		} 

	}

	public void Focus() {
		BecomeFirstResponder();
	}

	public override bool CanBecomeFirstResponder {
		get {
			Focusing?.Invoke();
			return true;
		}
	}

	public override bool CanResignFirstResponder {
		get {
			Unfocusing?.Invoke();
			return true;
		}
	}

	public override UIView? HitTest(CoreGraphics.CGPoint point, UIEvent? uievent) {
		// Permite que los clicks pasen a las vistas de abajo
		return null;
	}

	public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt) {
		bool handled = false;

		foreach (UIPress press in presses) {
			if (press.Key != null) {
				KeyPressedInfo keyPressedInfo = new(
					press.Key.Characters,
					(long)press.Key.KeyCode,
					(long)press.Key.ModifierFlags
				);

				// Update UI on Main Thread
				MainThread.BeginInvokeOnMainThread(() => {
					OnKeyPressed?.Invoke(keyPressedInfo);
				});

				// Mark as handled to prevent propagation (system shortcuts)
				// Note: This relies on not calling base.PressesBegan
				handled = true;
			}
		}

		if (!handled) {
			base.PressesBegan(presses, evt);
		}
	}

}
/*
 public override UIKeyCommand[] KeyCommands {
		get {
			// Claim shortcuts to prevent system Menu Bar from stealing them
			return new[]
			{
				UIKeyCommand.Create(
					new NSString("d"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				), // Close (was W)
				UIKeyCommand.Create(
					new NSString("n"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				),
				UIKeyCommand.Create(
					new NSString("e"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				), // Menu (was M)
				UIKeyCommand.Create(
					new NSString("0"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				),
				UIKeyCommand.Create(
					new NSString("1"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				),
				UIKeyCommand.Create(
					new NSString("2"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				),
				UIKeyCommand.Create(
					new NSString("3"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				),
				UIKeyCommand.Create(
					new NSString("4"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				),
				UIKeyCommand.Create(
					new NSString("5"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				),
				UIKeyCommand.Create(
					new NSString("6"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				),
				UIKeyCommand.Create(
					new NSString("7"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				),
				UIKeyCommand.Create(
					new NSString("8"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				),
				UIKeyCommand.Create(
					new NSString("9"),
					UIKeyModifierFlags.Command,
					new Selector("handleKeyCommand:")
				),
			};
		}
	}

	[Export("handleKeyCommand:")]
	public void HandleKeyCommand(UIKeyCommand command) {
		// Process the command as a normal key press event
		long keyCode = 0; // Not strictly needed for chars but consistent with struct
						  // Convert input to keycode if needed or just pass char

		KeyPressedInfo info = new(command.Input, keyCode, (long)command.ModifierFlags);

		MainThread.BeginInvokeOnMainThread(() => {
			OnKeyPressed?.Invoke(info);
		});
	}
 */