namespace blazor_dashboard;

/// <summary>
/// Stores a user's state in memory.
/// </summary>
public class SiteState
{
	private const int MAX_HISTORY_LENGTH = 10;

	public PageState ActivePage { get; set; }

	/// <summary>
	/// Our browsing history. The list has a maxmimum size; if the history length exceeds this,
	/// old history is removed.
	/// </summary>
	public List<PageState> History { get; private set; } = new();

	/// <summary>
	/// The page we are looking at in the browsing history (the nth element
	/// of _history).
	/// </summary>
	private int _historyIndex = 0;

	/// <summary>
	/// Components can register a callback function that will be called
	/// when the state changes. This helps them know when to refresh.
	/// </summary>
	private List<Action> _onStateChangedActions { get; set; } = new();

	public SiteState()
	{
		History = new()
		{
			new PageState(typeof(HomePage), "Home")
		};

		ActivePage = History[0];
	}

    /// <summary>
    /// Go to this page. If this page is already in our history, we will switch
    /// to the saved version automatically.
    /// </summary>
    public void SetPage(PageState pageState)
	{
        // Prefer backtracking to an already existing page if there is already
        // a page that matches this.
        var matchingPage = History.FirstOrDefault(x => x.Equals(pageState));

        if (matchingPage != null)
        {
            SetActivePage(matchingPage);
            return;
        }

		History.Add(pageState);
        SetActivePage(pageState);

        if (History.Count > MAX_HISTORY_LENGTH)
			History.RemoveAt(0);

        StateHasChanged();
    }

    /// <summary>
    /// Go to this page. If this page is already in our history, we will switch
    /// to the saved version automatically. Optionally accepts parameters if the
    /// page needs them.
    /// </summary>
    public void SetPage(Type pageType, string pageName, Dictionary<string, object>? pageParameters = null)
	{
		SetPage(new PageState(pageType, pageName, pageParameters));
	}

	/// <summary>
	/// Set this page as the active page.
	/// </summary>
	private void SetActivePage(PageState pageState)
	{
		if (!History.Any(x => x == pageState))
			throw new Exception("no such PageState in History when attempting to set active page");

		_historyIndex = History.IndexOf(pageState);
		ActivePage = History[_historyIndex];

		StateHasChanged();
	}

	public void SubscribeToStateChange(Action onStateChangeAction)
	{
		_onStateChangedActions.Add(onStateChangeAction);
	}

	/// <summary>
	/// Force subscribers to re-render as the state has changed.
	/// </summary>
	private void StateHasChanged()
	{
		foreach (var action in _onStateChangedActions)
			action.Invoke();
	}
}