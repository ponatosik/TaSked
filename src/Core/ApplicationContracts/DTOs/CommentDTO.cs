using TaSked.Domain;

namespace TaSked.Application;

public class CommentDTO
{
	public Guid Id { get; set; }
	public string AuthorUsername { get; set; }
	public string Content { get; set; }

	public CommentDTO(Guid id, string authorUsername, string content)
	{
		Id = id;
		AuthorUsername = authorUsername;
		Content = content;
	}

	public static CommentDTO From(Comment comment)
	{
		return new CommentDTO(comment.Id, comment.Author.Nickname, comment.Content);
	}
}