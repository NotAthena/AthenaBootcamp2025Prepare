using System.Collections;
using System.Collections.Generic;

public class ListHelper 
{
    public static bool IsContainDuplicatedElement<T>(List<T> list)
    {
        HashSet<T> tmp = new HashSet<T>();
        foreach (T num in list)
        {
            if (tmp.Contains(num))
                return true; // Duplicate found
            tmp.Add(num);
        }
        return false; // No duplicates
    }

    public static bool IsHaveCommonElements<T>(List<T> list1, List<T> list2)
    {
        HashSet<T> set = new HashSet<T>(list1); // Store elements of list1 in a set
        foreach (T num in list2)
        {
            if (set.Contains(num)) // Check if any element of list2 exists in set
                return true;
        }
        return false;
    }
}
