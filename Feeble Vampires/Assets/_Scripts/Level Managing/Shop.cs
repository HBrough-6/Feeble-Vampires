using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private PlayerAbilities playerAbilities;

    private ItemHolderManager itemHolderManager;
    private GameObject itemSelectHolder;
    private LevelManager levelManager;
    private GameManager gameManager;

    private GameObject shopHolder;

    public ItemButton[] itemButtons = new ItemButton[2];

    private ItemSO[] availableItems = new ItemSO[3];

    public List<ItemSO> possibleItems;
    public List<ItemSO> currentItems;

    private List<int> itemsInButton = new List<int>();

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
    }

    public void ActivateVendor()
    {
        // choose random items
        ChooseRandomItems();

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
        itemSelectHolder.SetActive(false);
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
        // choose 2 items
        // assign them to the current Items
        int index1 = Random.Range(0, indexes.Count);

        itemsInButton.Add(indexes[index1]);
        indexes.RemoveAt(index1);
        int index2 = Random.Range(0, indexes.Count);

        itemsInButton.Add(indexes[index2]);

    }

    public void PurchaseItem(int itemIndex)
    {
        ItemSO pItem = availableItems[itemIndex];
    }

    // pass through either 1 or 2 to use button 1 or 2
    public void UseButton(int button)
    {
        if (button != 1 && button != 2)
        {
            return;
        }

        if (currentItems.Count > 2)
        {
            // if the player has too many items, give the option to replace one of them

        }
        else
        {
            // add the item to the currentItems
            currentItems.Add(possibleItems[itemsInButton[button]]);
            // add the item to the display

            // activate the item
            // disable the option to purchase that item
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
        }
    }

    /*/// <summary>
    /// Randomly chooses and assign dialogue from the Resources folder Dialogue Options 
    /// </summary>
    public void ChooseRandomDialogue()
    {
        // fill a list with numbers 0 - dialogueOptions.count
        List<int> indexes = new List<int>();
        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            indexes.Add(i);
        }

        // if dialogue indexes is not -1, remove that index from the list
        if (previousDialogueOption != -1)
        {
            indexes.RemoveAt(previousDialogueOption);
        }
        // choose a random number from that list
        int rand = Random.Range(0, dialogueOptions.Length);
        DialogueSO dialogue = dialogueOptions[rand];
        // if dialogue index is -1, assign it to current index of the dialogue option
        previousDialogueOption = rand;

        // change the text to have the same text as the dialogueOption
        dialogueObject.text = dialogueOptions[rand].DialogueOption;
    }*/


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
