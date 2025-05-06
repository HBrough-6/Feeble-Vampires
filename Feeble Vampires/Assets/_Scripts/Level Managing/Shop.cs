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
    private PlayerItems playerItems;
    private MovementManager movementManager;

    private GameObject shopHolder;

    public ItemButton[] itemButtons = new ItemButton[3];

    private ItemSO[] availableItems = new ItemSO[3];

    public List<ItemSO> possibleItems;

    public List<ItemSO> currentItems;

    public List<int> itemsInButton = new List<int>();

    //public List<ItemButton> itemButton = new List<ItemButton>();

    [SerializeField] private GameObject dialogueHolder;
    private TMP_Text dialogueObject;

    [SerializeField] private string firstDialogue;
    [SerializeField] private string secondDialogue;
    private string currentText;

    public bool visited = false;
    public int savedMove;

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
        movementManager = FindObjectOfType<MovementManager>();
    }

    public void ActivateVendor()
    {
        if (!visited)
        {
            // set the current dialogue to the correct text
            currentText = firstDialogue;
            dialogueObject.text = currentText;
            // turn on UI
            shopHolder.SetActive(true);
            dialogueHolder.SetActive(true);

            // set the player to interacting
            savedMove = movementManager.spaceCap;
            movementManager.spaceCap = 0;
            visited = true;
        }
    }


    public void CloseShop()
    {
        // disable to window
        shopHolder.SetActive(false);
        itemSelectHolder.SetActive(false);
        itemsInButton.Clear();
        ConfirmSelection();

        movementManager.spaceCap = savedMove;
    }

    public void ChooseRandomItems()
    {
        List<int> indexes = new List<int>();
        // get the player's current items

        //Debug.Log("possible items" + possibleItems.Count);
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
        //Debug.Log(itemsInButton[2]);
        //Debug.Log(possibleItems[itemsInButton[2]].DisplayName);
        //Debug.Log(itemButtons.Length);
        itemButtons[2].AssignItem(possibleItems[itemsInButton[2]]);
    }


    public void SelectButton(int button)
    {
        // check if the current button is one that is already selected
        if (currentItems.IndexOf(possibleItems[itemsInButton[button - 1]]) != -1)
        {
            Debug.Log("Item already here");
            // call the deselect function
            DeselectButton(button);
            return;
        }

        // make sure there is space available and the player has enough xp to buy items
        if (currentItems.Count >= 2 || playerAbilities.experiencePoints <= currentItems.Count)
        {

            Debug.Log("currentItems count: " + currentItems.Count + "xp: " + playerAbilities.experiencePoints);
            return;
        }

        // add item to current items
        currentItems.Add(possibleItems[itemsInButton[button - 1]]);
        // disable the button
        itemButtons[button - 1].SetSelected(true);
    }

    public void DeselectButton(int button)
    {
        // enable the button
        itemButtons[button - 1].SetSelected(false);
        if (currentItems.IndexOf(possibleItems[itemsInButton[button - 1]]) != -1)
        {
            // remove the item from the list
            currentItems.Remove(possibleItems[itemsInButton[button - 1]]);
        }
    }

    private void ConfirmSelection()
    {

        for (int i = 0; i < currentItems.Count; i++)
        {
            // add the item to the display
            itemHolderManager.AddItem(currentItems[i]);
            // activate the item on the player
            playerItems.itemSlotCheck(currentItems[i].DisplayName);
        }

        playerAbilities.spendPoints(currentItems.Count, false);
        Debug.Log("spent " + currentItems.Count + " points");
        itemButtons[0].SetSelected(false);
        itemButtons[1].SetSelected(false);
        itemButtons[2].SetSelected(false);

        currentItems = new List<ItemSO>();


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
}
