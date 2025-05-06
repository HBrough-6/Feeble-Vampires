using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject itemsPanel;
    public GameObject skillsPanel;
    public GameObject detailPanel;

    [Header("List Setup")]
    public Transform itemContent;              // ItemsListPanel → Content
    public GameObject itemButtonPrefab;        // 아이템 버튼 프리팹
    public List<CollectionItem> items;         // 인스펙터에 드래그할 Aseprite 스프라이트+설명 목록

    [Header("Skill Setup")]
    public Transform skillContent;             // SkillsListPanel → Content
    public GameObject skillButtonPrefab;       // 스킬 버튼 프리팹
    public List<CollectionItem> skills;        // 스킬도 동일한 구조

    [Header("Detail UI")]
    public Image detailImage;
    public TMP_Text detailText;

    private enum LastList { None, Items, Skills }
    private LastList lastList = LastList.None;

    void Start()
    {
        ShowMenu();
    }

    void Update()
    {
        if (detailPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            CloseDetail();
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        itemsPanel.SetActive(false);
        skillsPanel.SetActive(false);
        detailPanel.SetActive(false);
        lastList = LastList.None;
    }

    public void ShowItemsList()
    {
        PopulateList(itemContent, itemButtonPrefab, items, LastList.Items);
    }

    public void ShowSkillsList()
    {
        PopulateList(skillContent, skillButtonPrefab, skills, LastList.Skills);
    }

    void PopulateList(Transform content, GameObject prefab, List<CollectionItem> dataList, LastList listType)
    {
        menuPanel.SetActive(false);
        itemsPanel.SetActive(false);
        skillsPanel.SetActive(false);
        detailPanel.SetActive(false);

        GameObject panelToShow = (listType == LastList.Items) ? itemsPanel : skillsPanel;
        panelToShow.SetActive(true);
        lastList = listType;

        // 기존 버튼 제거
        foreach (Transform t in content)
            Destroy(t.gameObject);

        // 새 버튼 생성
        foreach (var data in dataList)
        {
            var go = Instantiate(prefab, content);
            var btn = go.GetComponent<CollectionButton>();
            btn.Initialize(data, this);
        }
    }

    public void ShowDetail(Sprite icon, string desc)
    {
        itemsPanel.SetActive(false);
        skillsPanel.SetActive(false);
        detailImage.sprite = icon;
        detailText.text = desc;
        detailPanel.SetActive(true);
    }

    public void CloseDetail()
    {
        detailPanel.SetActive(false);
        if (lastList == LastList.Items) itemsPanel.SetActive(true);
        else if (lastList == LastList.Skills) skillsPanel.SetActive(true);
    }
}

