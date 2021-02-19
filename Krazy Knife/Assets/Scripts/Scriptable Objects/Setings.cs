using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameSetings", menuName = "GameSetings")]
public class Setings : ScriptableObject
{
    [Range(0,100)]public int chanceSpawnApple = 25;
    [Range(1,10)]public int maxSpawnKnif = 4;
    [Range(0f, 1f)] public float ShotDely = 0.5f;
    [Range(0f, 500f)] public float SpeedRotation = 70f;
}
