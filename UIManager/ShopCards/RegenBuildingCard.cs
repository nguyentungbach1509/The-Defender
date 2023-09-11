using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenBuildingCard : ShopCard
{
    MainBuilding _mainBuilding;
    MeowKnightControllerPC _meowKnightControllerPC;

    private void Awake()
    {
        _mainBuilding = FindFirstObjectByType<MainBuilding>();
        _meowKnightControllerPC = FindFirstObjectByType<MeowKnightControllerPC>();
    }
    public override void CardEffect()
    {
        int cardPoints = int.Parse(_cardPoints.text.Substring(0, _cardPoints.text.Length-1));
        if(_meowKnightControllerPC.PlayerPoints >= cardPoints )
        {
            _meowKnightControllerPC.PlayerPoints -= cardPoints;
            _mainBuilding._currentHealth = _mainBuilding._maxHealth;
        }
    }


}
