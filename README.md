# MAUI ListView image cancellation repro

Reproduces an iOS image-loading error with `ListView.HasUnevenRows = true`.

Confirmed on an iOS simulator with MAUI **10.0.110**
# MAUI ListView image-loading repro

Reproduces `PlatformView cannot be null here` when loading remote images in an iOS `ListView` with `HasUnevenRows = true`.

Confirmed on an iOS simulator with MAUI **10.0.50** and **10.0.110**. The project currently uses **10.0.110**.

## Run

Open the project in an IDE with .NET 10 MAUI/iOS support and run it on an iOS simulator with the debugger attached. Visual Studio on Windows requires a paired Mac.

On an Apple Silicon Mac, you can also run:

```sh
dotnet build -t:Run -f net10.0-ios -p:RuntimeIdentifier=iossimulator-arm64
```

## Reproduce

1. Leave `HasUnevenRows = true` in `ReproPage.cs`.
2. Open the IDE's Debug output.
3. Press **Load images** once and wait for the thumbnails to load.
4. Look for:

```text
Unexpected exception in MapSource.
System.InvalidOperationException: PlatformView cannot be null here
```

The stack includes `ImageImageSourcePartSetter.SetImageSource` and `ImageSourcePartExtensions.UpdateSourceAsync`. A cancelled image download and an “Unable to load image stream” warning appear beforehand.

The app remains running; the exception is logged.

## Comparison

Set `HasUnevenRows = false`, rebuild, and repeat. This avoids the error in the tested comparison, but changes the list's row-sizing behavior.

## What the sample does

The button assigns 100 items to a stock `ListView` using `ListViewCachingStrategy.RetainElement`. Each row contains a label and a 60-by-60 image.

Images come from public Picsum URLs. Each load uses fresh URLs with image caching disabled, so internet access is required.

There are no custom image handlers, explicit cancellations, or handler-disconnection calls.

- `ReproPage.cs` — the list, item template, and image URLs.
- `MauiProgram.cs` — MAUI setup and Debug logging.
- `App.cs` — opens the page.
