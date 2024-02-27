using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Dealer dealer;
    private GameObject firstCard;
    private GameObject secondCard;
    public HandAssessment handAssessment;
    public Transform[] playerCardSpawns;

    void Awake()
    {
    }

    void Update()
    {
    }

    public void DealPlayerCards()
    {
        if (firstCard != null || secondCard != null)
        {
            dealer.RemoveCard(firstCard);
            dealer.RemoveCard(secondCard);
        }
        firstCard = dealer.DealCard(playerCardSpawns[0]);
        secondCard = dealer.DealCard(playerCardSpawns[1]);
    }

    public GameObject GetFirstCard()
    {
        return firstCard;
    }

    public GameObject GetSecondCard()
    {
        return secondCard;
    }
}
