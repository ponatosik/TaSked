using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace TaSked.App;

public class TaskGroupModel : ObservableCollectionExtended<TaskViewModel>, IDisposable, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;
	// TODO: Use dynamic filter and use same group data source in all groups

	private string _title { get; set; }
	public string Title 
	{ 
		get => this._title;
		set
		{
			_title = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
		}
	}

	public TaskGroupModel(IGroup<TaskViewModel, Guid, string> group, IObservable<Func<TaskViewModel, bool>> filter, IObservable<IComparer<TaskViewModel>> comparer)
	{
		Title = $"{group.Key} ({Count})";

		var dataLoader = group.Cache.Connect()
				.Filter(filter)
				.Sort(comparer)
				.ObserveOn(RxApp.MainThreadScheduler)
				.Bind(this)
				.Subscribe();

		this.CollectionChanged += (_,_) => Title = $"{group.Key} ({Count})";
		_cleanUp = new CompositeDisposable(dataLoader);
	}

	public void Dispose()
	{
		_cleanUp.Dispose();
	}

	private readonly IDisposable _cleanUp;
}

