using System.Diagnostics;

namespace MauiImageCancellationRepro;

public sealed class ReproPage : ContentPage
{
    private readonly ListView imageList = new(ListViewCachingStrategy.RetainElement)
    {
        HasUnevenRows = true, // true to trigger bug. false = no bug. 
        SelectionMode = ListViewSelectionMode.None,
        ItemTemplate = new DataTemplate(CreateImageCell)
    };

    public ReproPage()
    {
        Title = "MAUI image cancellation";
        var loadButton = new Button
        {
            Text = "Load images",
            HorizontalOptions = LayoutOptions.Center,
        };
        loadButton.Clicked += LoadImages;

        var layout = new Grid
        {
            Padding = new Thickness(12),
            RowSpacing = 8,
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Star }
            }
        };
        layout.Add(loadButton, 0, 0);
        layout.Add(imageList, 0, 1);
        Content = layout;
    }

    private void LoadImages(object? sender, EventArgs e)
    {
        var items = new List<ImageItem>();
        var batchId = Guid.NewGuid().ToString("N");

        for (var i = 1; i <= 100; i++)
        {
            var title = $"Image {i}";
            var source = new UriImageSource
            {
                Uri = new Uri($"https://picsum.photos/seed/{batchId}-{i}/200/200"),
                CachingEnabled = false
            };
            items.Add(new ImageItem(title, source));
        }

        imageList.ItemsSource = items;
        Debug.WriteLine("Loaded 100 images. Scroll quickly to the bottom.");
    }

    private static ViewCell CreateImageCell()
    {
        var title = new Label { VerticalOptions = LayoutOptions.Center };
        title.SetBinding(Label.TextProperty, nameof(ImageItem.Title));

        var thumbnail = new Image
        {
            WidthRequest = 60,
            HeightRequest = 60,
            Aspect = Aspect.AspectFill
        };
        thumbnail.SetBinding(Image.SourceProperty, nameof(ImageItem.Source));

        var row = new Grid
        {
            Padding = new Thickness(12),
            ColumnSpacing = 12,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = 60 }
            }
        };
        row.Add(title, 0, 0);
        row.Add(thumbnail, 1, 0);

        return new ViewCell { View = row };
    }

    private sealed record ImageItem(string Title, ImageSource Source);
}
