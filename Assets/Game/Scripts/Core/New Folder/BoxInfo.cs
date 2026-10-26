using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBoxInfo", menuName = "Game/Box Info")]
public class BoxInfo : ScriptableObject
{
    [field: SerializeField] public float Weight { get; private set; } = 9.0f;
    [field: SerializeField] public float Glassness { get; private set; } = 1.0f;
}
