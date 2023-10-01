namespace blazor_dashboard;

/// <summary>
/// Represents a page and its data at a given point in time.
/// </summary>
public class PageState
{
	/// <summary>
	/// The type of the component that represents this page.
	/// </summary>
	public readonly Type PageType;

	public readonly string PageName;

	public readonly Dictionary<string, object> PageParameters;

	public PageState(Type pageType, string pageName, Dictionary<string, object>? pageParameters = null)
	{
		PageType = pageType;
		PageName = pageName;
		PageParameters = pageParameters ?? new();
	}

	public override bool Equals(object? obj)
	{
		if (obj == null)
			return false;

		if (obj == this)
			return true;

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

		// The original idea here was to make it so two pages that contained the same
		// data don't duplicate (and if they contain different data, they do).
		// In reality this doesn't really provide the clever behaviour we're after.
		// Even if we implement Equals() functions on the relevant types, this is
		// using the Equals() function on the object type, which just defaults to
		// the default behaviour. There is a way around this but it's long-winded
		// and just not worth fussing over here.
		return page1Parameters.Keys.All(
			k => page2Parameters.ContainsKey(k)
			&& page1Parameters[k].Equals(page2Parameters[k])
		);
	}

	public override int GetHashCode()
	{
		int hash = 17;

		hash = hash * 31 + PageType.GetHashCode();
		hash = hash * 31 + PageName.GetHashCode();

		foreach (var key in PageParameters.Keys)
		{
			hash = hash * 31 + key.GetHashCode();

			if (PageParameters[key] != null)
				hash = hash * 31 + PageParameters[key].GetHashCode();
		}

		return hash;
	}
}