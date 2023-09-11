using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ShopCard : MonoBehaviour
{
    /*
    public Text _cardName;
    protected string _cardType;
    public Text _cardDescription;
    */

    public Text _cardPoints;

    public abstract void CardEffect();
}
