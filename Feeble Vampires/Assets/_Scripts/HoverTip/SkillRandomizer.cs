using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillRandomizer : MonoBehaviour
{
    public TextMeshProUGUI skill1Text;
    public TextMeshProUGUI skill2Text;
    public List<string> skills;

    // Start is called before the first frame update
    void Start()
    {
        skill1Text.text = skills[Random.Range(0, skills.Count - 1)];
        skill2Text.text = skills[Random.Range(0, skills.Count - 1)];

        if (skill2Text == skill1Text) skill2Text.text = skills[Random.Range(0, skills.Count - 1)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
