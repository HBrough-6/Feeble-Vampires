using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemButton : MonoBehaviour
{
    public TMP_Text itemDescription;
    public Image itemSprite;
    public Button button;
    public GameObject selectedPanel;

    private void Awake()
    {
        itemDescription = transform.GetChild(0).GetComponent<TMP_Text>();
        itemSprite = transform.GetChild(2).GetComponent<Image>();
        button = GetComponent<Button>();
        selectedPanel = transform.GetChild(3).gameObject;
    }

    public void AssignItem(ItemSO item)
    {
        if (item == null)
        {
            Debug.Log("is null item");
        }
        itemDescription.text = item.ItemDescription;
        itemSprite.sprite = item.Icon;
    }

    // enables the panel that greys out the item image
    public void SetSelected(bool selected)
    {
        selectedPanel.SetActive(selected);
    }
}
