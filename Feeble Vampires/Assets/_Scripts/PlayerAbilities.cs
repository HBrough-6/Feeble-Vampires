using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public bool canEcholocate;
    public bool isSwifter;
    public bool canRushAttack;
    public bool smarter;
    public bool hideable;
    public bool isGreedy;
    public bool strongestInstinct;
    public bool scentTracker;
    public bool clone;

    public bool currentlyTracking;

    public UIManager uiManager;
    public MovementManager movementManager;

    public int experiencePoints;

    [Header("Particle Systems")]
    public GameObject scentTrackerParticles;
    public GameObject hemoglobinRushParticles;
    public GameObject swiftStepParticles;


    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        movementManager = FindObjectOfType<MovementManager>();

        if (isSwifter)
        {
            // this should be moved out of start and into a function that is called when the skill is gained
            movementManager.spaceCap += 1;
        }

        if (smarter)
        {
            // this should be moved out of start and into a function that is called when the skill is gained
            movementManager.timeLimit = movementManager.baseTime + 2;
        }

        if (strongestInstinct)
        {
            // this should be moved out of start and into a function that is called when the skill is gained
            movementManager.timeLimit /= 2;
            movementManager.spaceCap *= 2;
        }
        //movementManager.spaceCap += swiftLevel;

        experiencePoints = 0;

        uiManager.makeMap(canEcholocate);
    }

    // heath
    public void GainXP(int amount)
    {
        experiencePoints += amount;
        uiManager.UpdateXP(experiencePoints);
    }


    // heath reworked this
    public void activateSkill(string skillName)
    {
        Debug.Log(skillName + " passed through");
        if (skillName == "Echolocation")
        {
            //Debug.Log("echo");
            canEcholocate = true;
            AudioManager.Instance.PlaySFX("Echolocation");
        }
        else if (skillName == "Swift Step")
        {
            //Debug.Log("Swift");
            movementManager.spaceCap += 1;
            isSwifter = true;
            //AudioManager.Instance.PlaySFX("Swift Step");
        }
        else if (skillName == "Hemoglobin Rush")
        {
            //Debug.Log("Hemo");
            canRushAttack = true;
            AudioManager.Instance.PlaySFX("Hemoglobin Rush");
        }
        else if (skillName == "Neural Formation")
        {
            //Debug.Log("Neu");
            movementManager.timeLimit = movementManager.baseTime + 2;
            smarter = true;
            AudioManager.Instance.PlaySFX("Neural Formation");
        }
        else if (skillName == "Hang")
        {
            //Debug.Log("Hang");
            hideable = true;
            AudioManager.Instance.PlaySFX("Hang");
        }
        else if (skillName == "Cheapskate")
        {
            //Debug.Log("Cheap");
            isGreedy = true;
            AudioManager.Instance.PlaySFX("Cheapskate");
        }
        else if (skillName == "Apex Instinct")
        {
            //Debug.Log("apex");
            movementManager.timeLimit /= 2;
            movementManager.spaceCap *= 2;
            strongestInstinct = true;
            AudioManager.Instance.PlaySFX("Apex Instinct");
        }
        else if (skillName == "Tracker")
        {
            //Debug.Log("Tracker");
            scentTracker = true;
            AudioManager.Instance.PlaySFX("Tracker");
        }
        else if (skillName == "Clone")
        {
            //Debug.Log("Clone");
            clone = true;
            AudioManager.Instance.PlaySFX("Clone");
        }
    }

    public void activateSkill(TMPro.TextMeshProUGUI skillToActivate)
    {
        if (skillToActivate.text == "Echolocation")
        {
            canEcholocate = true;
        }
        else if (skillToActivate.text == "Swift Step")
        {
            isSwifter = true;
        }
        else if (skillToActivate.text == "Hemoglobin Rush")
        {
            canRushAttack = true;
        }
        else if (skillToActivate.text == "Neural Formation")
        {
            smarter = true;
        }
        else if (skillToActivate.text == "Hang")
        {
            hideable = true;
        }
        else if (skillToActivate.text == "Cheapskate")
        {
            isGreedy = true;
        }
        else if (skillToActivate.text == "Apex Instinct")
        {
            strongestInstinct = true;
        }
        else if (skillToActivate.text == "Scent Tracker")
        {
            scentTracker = true;
        }
        else if (skillToActivate.text == "Clone")
        {
            clone = true;
        }
    }

    public void GetSwift()
    {
        if (experiencePoints < 2)
        {
            Debug.Log("You do not have enough experience points");
        }
        else
        {
            //spendPoints();
            //swiftLevel += 1;
            //movementManager.spaceCap += swiftLevel;
        }

    }

    public void GetSmart()
    {
        if (experiencePoints < 2)
        {
            Debug.Log("You do not have enough experience points");
        }
        else
        {
            //spendPoints();
            movementManager.timeLimit = movementManager.baseTime + 2;
        }
    }

    public bool spendPoints(int points, bool isSkill)
    {
        Debug.Log("points: " + points + " isSkill: " + isSkill);
        // check if player has enough xp
        if (points <= experiencePoints || (isSkill && isGreedy && points - 1 <= experiencePoints))
        {
            if (isSkill && isGreedy)
            {
                experiencePoints--;
                Debug.Log("greedy skill");
            }
            else if (isSkill && !isGreedy)
            {
                experiencePoints -= 2;
                Debug.Log("skill");
            }
            else
            {
                Debug.Log("items");
                experiencePoints -= points;
            }
            Debug.Log("returned true");
            uiManager.UpdateXP(experiencePoints);
            return true;
        }
        else
        {
            Debug.Log("returned false");
            return false;
        }
    }

    public void sniffEnemies()
    {
        if (scentTracker)
        {
            currentlyTracking = true;
            StartCoroutine(scentTrackerPulse());
        }
    }

    public IEnumerator scentTrackerPulse()
    {
        scentTrackerParticles.SetActive(false);
        scentTrackerParticles.SetActive(true);
        yield return new WaitForSeconds(3.9f);
        scentTrackerParticles.SetActive(false);
    }

    public void toggleHemoglobinEnergy()
    {
        hemoglobinRushParticles.SetActive(!hemoglobinRushParticles.activeInHierarchy);
    }

    public IEnumerator swiftStepPulse()
    {
        swiftStepParticles.SetActive(false);
        swiftStepParticles.SetActive(true);
        yield return new WaitForSeconds(9.54f);
        swiftStepParticles.SetActive(false);
    }
}
