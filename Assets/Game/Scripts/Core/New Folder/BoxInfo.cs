using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBoxInfo", menuName = "Game/Box Info")]
public class BoxInfo : ScriptableObject
{
    [field: SerializeField] public float _weight { get; private set; } = 9.0f;
}
