using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionButton : MonoBehaviour
{
    public Image iconImage;
    private CollectionItem data;
    private CollectionUIManager uiManager;

    public void Initialize(CollectionItem data, CollectionUIManager mgr)
    {
        this.data = data;
        this.uiManager = mgr;
        iconImage.sprite = data.icon;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        uiManager.ShowDetail(data.icon, data.description);
    }
}

