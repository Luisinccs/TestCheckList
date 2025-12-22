using TestCheckList.ViewModels;
using TestCheckList.Views;

namespace TestCheckList;

public class App : Application
{
	private readonly MainAppViewModel _viewModel;

	public App(MainAppPage mainPage, MainAppViewModel viewModel)
	{
		_viewModel = viewModel;
		MainPage = mainPage;
	}
}

