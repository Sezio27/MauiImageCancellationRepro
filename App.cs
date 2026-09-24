namespace MauiImageCancellationRepro;

public class App : Application
{
	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new ReproPage());
	}
}