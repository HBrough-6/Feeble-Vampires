using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemButton : MonoBehaviour
{
    private TMP_Text itemDescription;
    private Image itemSprite;
    private void Awake()
    {
        itemDescription = transform.GetChild(0).GetComponent<TMP_Text>();
        itemSprite = transform.GetChild(2).GetComponent<Image>();
    }

    public void AssignItem(ItemSO item)
    {
        itemDescription.text = item.ItemDescription;
        itemSprite.sprite = item.Icon;
    }
}
