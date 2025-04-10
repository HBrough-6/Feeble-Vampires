using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillRandomizer : MonoBehaviour
{
    //public TextMeshProUGUI skill1Text;
    //public TextMeshProUGUI skill2Text;
    public GameObject skill1Button;
    public GameObject skill2Button;
    //public List<string> skills;
    public List<SkillSO> skills;

    // Start is called before the first frame update
    void Start()
    {
        //skill1Text.text = skills[Random.Range(0, skills.Count - 1)];
        //skill2Text.text = skills[Random.Range(0, skills.Count - 1)];

        //if (skill2Text == skill1Text) skill2Text.text = skills[Random.Range(0, skills.Count - 1)];

        skill1Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = skills[Random.Range(0, skills.Count - 1)].Icon;
        skill2Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = skills[Random.Range(0, skills.Count - 1)].Icon;

        if (skill2Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite ==
            skill1Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite)
            skill2Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = skills[Random.Range(0, skills.Count - 1)].Icon;

        //skill1Button.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
