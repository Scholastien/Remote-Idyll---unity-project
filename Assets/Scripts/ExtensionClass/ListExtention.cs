using System.Collections.Generic;

public static class ListExtention
{
    public static List<T> GetOccurrenceList<T>(List<T> items)
    {
        // Use HashSet to remember items seen.
        var result = new List<T>();
        var set = new HashSet<T>();
        for (int i = 0; i < items.Count; i++)
        {
            // Add if needed.
            if (!set.Contains(items[i]))
            {
                set.Add(items[i]);
            }
            else if (!result.Contains(items[i]) && set.Contains(items[i]))
            {
                result.Add(items[i]);
            }
        }
        return result;
    }

    public static List<T> RemoveDuplicatesSet<T>(List<T> items)
    {
        // Use HashSet to remember items seen.
        var result = new List<T>();
        var set = new HashSet<T>();
        for (int i = 0; i < items.Count; i++)
        {
            // Add if needed.
            if (!set.Contains(items[i]))
            {
                result.Add(items[i]);
                set.Add(items[i]);
            }
        }
        return result;
    }
}
