# MAUI image cancellation reproduction

Small iOS-only .NET 10 / MAUI 10.0.110 sample: one button and a list of remote images. No custom image handlers, workaround, local server, or Sentry.

## Run

Open `MauiImageCancellationRepro.csproj` in an IDE with MAUI/iOS support and select an iPhone simulator or provisioned iPhone. Windows requires a paired Mac for running iOS apps.

Alternatively, on an Apple Silicon Mac with a compatible Xcode and .NET 10 MAUI/iOS workload:

```sh
dotnet build MauiImageCancellationRepro.csproj -t:Run -f net10.0-ios -p:RuntimeIdentifier=iossimulator-arm64
```

For an Intel Mac simulator, use `iossimulator-x64`.

## Reproduce

1. Run with the debugger attached and open **View > Output > Debug** in Visual Studio.
2. Tap **load images**.
3. Immediately scroll to the bottom while the thumbnails are loading.
4. Tap the button again for another attempt, or restart for a fresh run.

Internet access is required. Each attempt uses 100 distinct Picsum URLs with caching disabled. The list keeps `RetainElement`, `HasUnevenRows`, and the same image-cell layout as the earlier reproduction. Some rows have extra text to vary their height. No code explicitly cancels a request or disconnects a handler.

## Expected evidence

Look for these together in Debug output:

```text
Unexpected exception in MapSource.
System.InvalidOperationException: PlatformView cannot be null here
```

The stack should include `ImageImageSourcePartSetter.SetImageSource` and `ImageSourcePartExtensions.UpdateSourceAsync`. Cancellation or network warnings alone do not establish reproduction. Scrolling is timing-dependent, so the error might not appear on every attempt.

## Main files

- `ReproPage.cs`: creates the list, its cells, and the image URLs.
- `MauiProgram.cs`: starts MAUI and enables standard Debug logging.
- `App.cs`: opens the page.

The earlier scrolling sample reproduced the exception on an iOS simulator. This simplified version removes the diagnostic UI and uses native default styling; it needs a fresh simulator run to confirm the same behavior.
