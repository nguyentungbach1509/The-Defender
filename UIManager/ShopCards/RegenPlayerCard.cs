using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenPlayerCard : ShopCard
{
    MeowKnightControllerPC _meowKnightControllerPC;

    private void Awake()
    {
        _meowKnightControllerPC = FindFirstObjectByType<MeowKnightControllerPC>();
    }

    public override void CardEffect()
    {
        int cardPoints = int.Parse(_cardPoints.text.Substring(0, _cardPoints.text.Length - 1));
        if(_meowKnightControllerPC.PlayerPoints >= cardPoints)
        {
            _meowKnightControllerPC.PlayerPoints -= cardPoints;
            _meowKnightControllerPC._currentHealth = _meowKnightControllerPC._maxHealth;
        }
    }

}
