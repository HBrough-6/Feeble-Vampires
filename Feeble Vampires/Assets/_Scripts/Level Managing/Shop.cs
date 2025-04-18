using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private PlayerAbilities playerAbilities;

    private ItemHolderManager itemHolderManager;
    private GameObject itemSelectHolder;
    private LevelManager levelManager;
    private GameManager gameManager;
    private PlayerItems playerItems;

    private GameObject shopHolder;

    public ItemButton[] itemButtons = new ItemButton[3];

    private ItemSO[] availableItems = new ItemSO[3];

    public List<ItemSO> possibleItems;
    public List<ItemSO> currentItems;

    public List<int> itemsInButton = new List<int>();

    public List<ItemButton> itemButton = new List<ItemButton>();

    [SerializeField] private GameObject dialogueHolder;
    private TMP_Text dialogueObject;

    [SerializeField] private string firstDialogue;
    [SerializeField] private string secondDialogue;
    private string currentText;

    private void Awake()
    {
        playerAbilities = FindObjectOfType<PlayerAbilities>();
        itemHolderManager = FindObjectOfType<ItemHolderManager>();
        levelManager = FindObjectOfType<LevelManager>();
        gameManager = FindObjectOfType<GameManager>();

        shopHolder = transform.GetChild(0).gameObject;
        itemSelectHolder = shopHolder.transform.GetChild(2).gameObject;
        /*dialogueHolder = transform.GetChild(1).gameObject;*/
        dialogueObject = dialogueHolder.transform.GetChild(1).GetComponent<TMP_Text>();
        playerItems = FindObjectOfType<PlayerItems>();
    }

    public void ActivateVendor()
    {
        // set the current dialogue to the correct text
        currentText = firstDialogue;
        dialogueObject.text = currentText;
        // turn on UI
        shopHolder.SetActive(true);
        dialogueHolder.SetActive(true);

        // set the player to interacting
        gameManager.skillSelecting = true;
    }


    public void CloseShop()
    {
        // disable to window
        shopHolder.SetActive(false);
        shopHolder.SetActive(false);
        // set the player to not interacting
        gameManager.skillSelecting = false;
    }

    public void ChooseRandomItems()
    {
        List<int> indexes = new List<int>();
        // get the player's current items

        Debug.Log("possible items" + possibleItems.Count);
        for (int i = 0; i < possibleItems.Count; i++)
        {
            indexes.Add(i);
        }

        // get indexes in the list of the items
        for (int i = 0; i < currentItems.Count; i++)
        {
            int itemIndex = possibleItems.IndexOf(currentItems[i]);
            for (int j = 0; j < indexes.Count; j++)
            {
                // if the current index is the same as the item being checked, remove it
                if (indexes[j] == itemIndex)
                {
                    indexes.RemoveAt(j);
                    break;
                }
            }
        }

        // get the items from the list
        // choose 3 items
        // assign them to the current Items
        int index1 = Random.Range(0, indexes.Count);
        itemsInButton.Add(indexes[index1]);
        indexes.RemoveAt(index1);
        itemButtons[0].AssignItem(possibleItems[itemsInButton[0]]);

        int index2 = Random.Range(0, indexes.Count);
        itemsInButton.Add(indexes[index2]);
        itemButtons[1].AssignItem(possibleItems[itemsInButton[1]]);

        indexes.RemoveAt(index2);

        int index3 = Random.Range(0, indexes.Count);
        itemsInButton.Add(indexes[index3]);
        Debug.Log(itemsInButton[2]);
        Debug.Log(possibleItems[itemsInButton[2]].DisplayName);
        Debug.Log(itemButtons.Length);
        itemButtons[2].AssignItem(possibleItems[itemsInButton[2]]);
    }

    public void PurchaseItem(int itemIndex)
    {
        ItemSO pItem = availableItems[itemIndex];
    }

    // pass through either 1 or 2 to use button 1 or 2
    public void UseButton(int button)
    {
        if (button < 1 || button > 3)
        {
            return;
        }

        if (currentItems.Count >= 2)
        {
            // if the player has too many items, give the option to replace one of them

        }
        else
        {
            // add the item to the currentItems
            currentItems.Add(possibleItems[itemsInButton[button - 1]]);
            // add the item to the display
            itemHolderManager.AddItem(currentItems[currentItems.Count - 1]);
            // activate the item
            bool fakeRef = true;
            playerItems.itemSlotCheck(ref fakeRef, currentItems[currentItems.Count - 1].DisplayName);
            // disable the option to purchase that item
            Debug.Log("button " + button);
            itemButtons[button - 1].GetComponent<Button>().interactable = false;
        }
    }

    public void DialogueButton()
    {
        if (currentText == firstDialogue)
        {
            currentText = secondDialogue;
            dialogueObject.text = currentText;
        }
        else if (currentText == secondDialogue)
        {
            dialogueHolder.SetActive(false);
            itemSelectHolder.SetActive(true);
            // choose random items
            ChooseRandomItems();
        }
    }


    private void OnGUI()
    {
        if (GUILayout.Button("set"))
        {
            ActivateVendor();
        }
        if (GUILayout.Button("activate"))
        {
            DialogueButton();
        }
    }
}
