
using UnityEngine;

[CreateAssetMenu(fileName = "newSeekStateData", menuName = "Data/State Data/Seek State")]
public class D_SeekState : ScriptableObject
{
    public float speed = 400f;
    public float nextWaypointDistance = 3f;
}
