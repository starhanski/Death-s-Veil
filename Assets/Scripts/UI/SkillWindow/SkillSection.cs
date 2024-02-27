using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SkillSection : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public Text title;
    public Text description;


    public Skills currentSkill;

    private SkillWindowHandler skillWindowHandler;

    private PlayerAbilityManager abilityManager;
    private void Awake()
    {
        skillWindowHandler = GetComponentInParent<SkillWindowHandler>();
        abilityManager = GameObject.Find("Player").GetComponent<PlayerAbilityManager>();
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        HandleSkillSectionClick(this);
    }
    public void HandleSkillSectionClick(SkillSection skillSection)
    {
        Debug.Log("U selected: " + skillSection.currentSkill.title);
        skillWindowHandler.SetSkillWindow();
        abilityManager.OnSkillSelected(skillSection.currentSkill);

    }

    

}
