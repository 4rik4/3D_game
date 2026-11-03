using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Game.Scripts.Gameplay.Views
{
    public class GlassWallView : MonoBehaviour
    {
        [field: SerializeField] public float Strength { get; private set; }
    }
}