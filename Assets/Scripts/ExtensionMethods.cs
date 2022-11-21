using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtentionMethods
{
    public static bool HasTag(this GameObject obj, Tag searchTag)
    {
        return obj.TryGetComponent<Tags>(out Tags tagList) && tagList.HasTag(searchTag);
    }

    public static bool HasTag(this GameObject obj, string tagString)
    {
        return obj.TryGetComponent<Tags>(out Tags tagList) && tagList.HasTag(tagString);
    }
}
