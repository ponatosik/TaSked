
namespace TaSked.App.Common.Models
{
	public class RelatedLinkModel
	{
		public string? Title { get; set; }
		public string? Url { get; set; }

		public RelatedLinkModel(string? title = null, string? url = null)
		{
			Title = title;
			Url = url;
		}
	}
}