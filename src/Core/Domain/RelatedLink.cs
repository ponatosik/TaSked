namespace TaSked.Domain;

public record RelatedLink
{
	public string? Title { get; init; }
	public Uri Url { get; init; } = null!;

	private RelatedLink() { }

	public RelatedLink(Uri url, string? title)
	{
		Url = url;
		Title = title;
	}

	public static RelatedLink Create(Uri url, string? title = null)
	{
		return new RelatedLink(url, title);
	}
}