using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAbilityData", menuName = "Data/Player Data/Ability Data")]
public class PlayerAbilityStats : ScriptableObject
{

   

    [Header(" Player Skills")]
    public List<Skills> playerSkills;

 [Header(" Started Player Skills")]
    public List<Skills> startedPlayerSkills = new List<Skills>();

}
