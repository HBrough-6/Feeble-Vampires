using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemHolderManager : MonoBehaviour
{
    private List<ItemSO> items = new List<ItemSO>();

    [SerializeField] private Image[] skillIcons = new Image[3];

    //Christophe addition
    [SerializeField] private TMP_Text[] itemDesc = new TMP_Text[2];

    public void AddItem(ItemSO item)
    {
        // cannot have more than 2 skills displaying
        if (items.Count >= 2)
            return;

        // add the skill to the list
        items.Add(item);

        // assign the newest skill to an Icon
        skillIcons[items.Count - 1].sprite = items[items.Count - 1].Icon;

        AddItemDesc(item);
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

    public void AddItemDesc(ItemSO itemdesc)
    {
        if (items.Count >= 2)
            return;

        itemDesc[items.Count - 1].text = items[items.Count - 1].ItemDescription;

    }
}
