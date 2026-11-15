using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Views;
using UnityEngine;

public class VicoryController : MonoBehaviour
{
    private BoxDeliveryService _boxDeliveryService;
    [SerializeField] private GameObject _victoryPanel;
    
    
    private void Start()
    {
        _boxDeliveryService.OnWon += YouWon;
    }

    private void YouWon()
    {
        _victoryPanel.SetActive(true);
    }
}
