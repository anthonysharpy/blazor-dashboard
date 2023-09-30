using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Collections;

namespace blazor_dashboard;

/// <summary>
/// Stores a user's state in memory.
/// </summary>
public class SiteState
{
	private const int MAX_HISTORY_LENGTH = 10;

	/// <summary>
	/// The page we are looking at in the browsing history (the nth element
	/// of History).
	/// </summary>
	public int HistoryIndex { get; set; } = 0;

	/// <summary>
	/// Our browsing history. The list has a maxmimum size; if the history length exceeds this,
	/// old history is removed.
	/// </summary>
	public List<PageState> History { get; set; } = new();

	public PageState ActivePage { get; set; }

	private List<Action> OnStateChangedActions { get; set; } = new();

	public SiteState()
	{
		History = new()
		{
			new PageState{
				PageType = typeof(HomePage),
				PageParameters = new()
			}
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
        {
            History.RemoveAt(0);
        }

        StateHasChanged();
    }

    /// <summary>
    /// Go to this page. If this page is already in our history, we will switch
    /// to the saved version automatically. Optionally accepts parameters if the
    /// page needs them.
    /// </summary>
    public void SetPage(Type pageType, Dictionary<string, object>? pageParameters = null)
	{
		var pageState = new PageState
		{
			PageType = pageType,
			PageParameters = pageParameters ?? new()
		};

		SetPage(pageState);
	}

	/// <summary>
	/// Set this page as the active page.
	/// </summary>
	private void SetActivePage(PageState pageState)
	{
		if (!History.Any(x => x == pageState))
			throw new Exception("no such PageState in History when attempting to set active page");

		HistoryIndex = History.IndexOf(pageState);
		ActivePage = History[HistoryIndex];

		StateHasChanged();
	}

	public void SubscribeToStateChange(Action onStateChangeAction)
	{
		OnStateChangedActions.Add(onStateChangeAction);
	}

	/// <summary>
	/// Force subscribers to re-render as the state has changed. Couldn't find any
	/// any better way of doing this.
	/// </summary>
	private void StateHasChanged()
	{
		foreach(var action in OnStateChangedActions)
		{
			action.Invoke();
		}
	}
}

/// <summary>
/// Represents a page and its data at a given point in time.
/// </summary>
public class PageState
{
	/// <summary>
	/// The type of the component that represents this page.
	/// </summary>
	public Type? PageType { get; set; }

	public Dictionary<string, object>? PageParameters { get; set; }

	public override bool Equals(object? obj)
	{
		if (obj == null)
			return false;

		if (obj is PageState otherPageState)
		{
            if (otherPageState.PageType.Name != PageType.Name)
				return false;

			return PagesHaveSameParameters(PageParameters, otherPageState.PageParameters);
		}

        return false;
	}

	private bool PagesHaveSameParameters(Dictionary<string, object> page1Parameters,
		Dictionary<string, object> page2Parameters)
	{
		if (page1Parameters.Keys.Count != page2Parameters.Keys.Count)
			return false;

		// In reality this doesn't really provide the clever behaviour we're after.
		// Even if we implement Equals() functions on the relevant types, this is
		// using the Equals() function on the object type, which just defaults to
		// the default behaviour. There is a way around this but it's long-winded
		// and this is not really a priority.
        return page1Parameters.Keys.All(
            k => page2Parameters.ContainsKey(k)
            && page1Parameters[k].Equals(page2Parameters[k])
        );
    }
}