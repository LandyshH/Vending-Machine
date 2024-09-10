namespace Domain.Products.ValueObjects;

public class Image
{
    public string Url { get; private set; }

    public Image(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Image URL cannot be null or empty.", nameof(url));
        }

        Url = url;
    }
}
