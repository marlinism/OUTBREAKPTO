using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class Tags : MonoBehaviour
{
    [SerializeField]
    private List<Tag> tagList;

    public List<Tag> TagList
    {
        get { return tagList; }
    }

    public bool HasTag(Tag searchTag)
    {
        return tagList.Contains(searchTag);
    }

    public bool HasTag(string tagString)
    {
        return tagList.Exists(currTag => currTag.Name == tagString);
    }
}
