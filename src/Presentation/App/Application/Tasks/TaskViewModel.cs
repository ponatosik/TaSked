using CommunityToolkit.Mvvm.ComponentModel;
using TaSked.Domain;

namespace TaSked.App;

public partial class TaskViewModel : ObservableObject
{
	[ObservableProperty]
	private string _taskTitle;
	[ObservableProperty]
	private string _subjectName;
	[ObservableProperty]
	private string _description;
	[ObservableProperty]
	private DateTime? _deadline;
	[ObservableProperty]
	private bool _completed;


	public TaskViewModel() { }
	public TaskViewModel(string taskTitle, string subjectName, string description, DateTime? deadline, bool completed)
	{
		TaskTitle = taskTitle;
		SubjectName = subjectName;
		Description = description;
		Deadline = deadline;
		Completed = completed;
	}

	public TaskViewModel(HomeworkTask task, string subjectName)
	{
		TaskTitle = task.Homework.Title;
		SubjectName = subjectName;
		Description = task.Homework.Description;
		Deadline = task.Homework.Deadline;
		Completed = task.Completed;
	}
}
