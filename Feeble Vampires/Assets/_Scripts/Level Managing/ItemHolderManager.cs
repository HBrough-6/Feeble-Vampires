using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolderManager : MonoBehaviour
{
    private List<ItemSO> items = new List<ItemSO>();

    [SerializeField] private Image[] skillIcons = new Image[3];


    public void AddItem(ItemSO item)
    {
        // cannot have more than 2 skills displaying
        if (items.Count >= 2)
            return;

        // add the skill to the list
        items.Add(item);

        // assign the newest skill to an Icon
        skillIcons[items.Count - 1].sprite = items[items.Count - 1].Icon;
    }

    public void RemoveItem(ItemSO item)
    {
        int index = items.IndexOf(item);
        if (index == -1)
        {
            return;
        }

        items.RemoveAt(index);

    }

    public void ReplaceItem(ItemSO target, ItemSO newItem)
    {

    }
}
