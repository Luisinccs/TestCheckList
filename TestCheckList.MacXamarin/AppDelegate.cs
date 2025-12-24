
using static AppKit.NSWindowStyle;

using AppKit;
using CoreGraphics;
using Foundation;
using TestCheckList.Views.Mac;
using TestCheckList.ViewModels;

namespace TestCheckList.MacXamarin {
	[Register("AppDelegate")]

	public class AppDelegate : NSApplicationDelegate {
		public AppDelegate() {
		}

		public override void DidFinishLaunching(NSNotification notification) {
			CGRect rect = new(0, 100, 1100, 700);
			NSWindow mainWindow = new(rect, Closable | Resizable | Titled | Miniaturizable,
				NSBackingStore.Buffered, false);
			MainViewController controller = new( new MainAppPageViewModel());

			controller.View.Frame = rect;


			mainWindow.ContentViewController = controller;
			mainWindow.MakeKeyAndOrderFront(this);
		}

		public override void WillTerminate(NSNotification notification) {
			// Insert code here to tear down your application
		}
	}
}

