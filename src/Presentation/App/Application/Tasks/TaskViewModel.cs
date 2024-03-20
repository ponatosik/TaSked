using CommunityToolkit.Mvvm.ComponentModel;
using TaSked.Domain;

namespace TaSked.App;

public partial class TaskViewModel : ObservableObject
{
	[ObservableProperty]
	private HomeworkTask _task;

	[ObservableProperty]
	private string _subjectName;

	public TaskViewModel() { }
	public TaskViewModel(HomeworkTask task, string subjectName)
	{
		_task = task;
		_subjectName = subjectName;
	}
}
