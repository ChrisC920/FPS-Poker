using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public GameObject[] cardSpawns;
    public GameObject cardPrefab;
    private bool[,] cardCheck;
    private int numSharedCards;
    public List<GameObject> sharedCards;
    private GameObject[] players;
    private Card.Suit[] suits;
    private Card.Rank[] ranks;
    private int numCards;

    void Awake()
    {
        suits = Card.suits;
        ranks = Card.ranks;
        cardCheck = new bool[suits.Length, ranks.Length];
        players = GameObject.FindGameObjectsWithTag("Player");
        sharedCards = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            sharedCards.Add(GameObject.Find($"Card Spawns/Card Spawn {i + 1}"));
        }
        numCards = GameObject.FindGameObjectsWithTag("Card").Length;
    }

    void Update()
    {
        
    }


    private GameObject CreateNewCard(Card.Suit suit, Card.Rank rank, Transform transform)
    {
        cardPrefab.GetComponent<Card>().suit = suit;
        cardPrefab.GetComponent<Card>().rank = rank;
        numCards++;
        return Instantiate(cardPrefab, transform);
    }

    private Card.Suit GetRandomSuit()
    {
        if (suits == null || suits.Length == 0)
        {
            throw new ArgumentException("Array must contain at least one element");
        }

        System.Random random = new System.Random();
        int randomSuit = random.Next(0, suits.Length);
        Card.Suit output = suits[randomSuit];
        
        return output;
    }

    private Card.Rank GetRandomRank()
    {
        if (ranks == null || ranks.Length == 0)
        {
            throw new ArgumentException("Array must contain at least one element");
        }

        System.Random random = new System.Random();
        int randomRank = random.Next(0, ranks.Length);
        Card.Rank output = ranks[randomRank];
        return output;
    }

    public GameObject DealCard(Transform spawn)
    {
        Card.Suit currSuit = GetRandomSuit();
        Card.Rank currRank = GetRandomRank();
        int cardCheckSuitIndex = (int)currSuit - 1;
        int cardCheckRankIndex = currRank == Card.Rank.Ace ? 0 : (int)currRank - 1; // Ace is index 0, King is index 13
        bool CurrCardHasBeenDealt = cardCheck[cardCheckSuitIndex, cardCheckRankIndex];
        while (CurrCardHasBeenDealt)
        {
            currSuit = GetRandomSuit();
            currRank = GetRandomRank();
            cardCheckSuitIndex = (int)currSuit - 1;
            cardCheckRankIndex = currRank == Card.Rank.Ace ? 0 : (int)currRank - 1;
            CurrCardHasBeenDealt = cardCheck[cardCheckSuitIndex, cardCheckRankIndex];

            // FAIL SAFE: gets out of loop and resets to a new deck of cards
            if (numCards > cardCheck.Length)
            {
                cardCheck = new bool[suits.Length, ranks.Length];
                numCards = 0;
                break;
            }
        }
        GameObject currCard = CreateNewCard(currSuit, currRank, spawn);
        cardCheck[cardCheckSuitIndex, cardCheckRankIndex] = true;
        return currCard;
    }

    public void DealTable()
    {
        if (numSharedCards < 3)
        {
            DealAllPlayers();
            DealOneCardToTable();
            DealOneCardToTable();
            DealOneCardToTable();
        }
        else if (numSharedCards == 3)
        {
            DealOneCardToTable();

        }
        else if (numSharedCards == 4)
        {
            DealOneCardToTable();
            foreach (GameObject player in players)
            {
                player.GetComponentInChildren<HandAssessment>().CalculateHand();
            }
        }
    }

    private void DealAllPlayers()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<Player>().DealPlayerCards();
        }
    }

    private void DealOneCardToTable()
    {
        GameObject cardToDeal = sharedCards[numSharedCards];
        if (cardToDeal.GetComponentInChildren<Card>())
        {
            RemoveCard(cardToDeal.GetComponentInChildren<Card>().gameObject);
        }
        GameObject card = DealCard(cardToDeal.transform);
        numSharedCards++;
    }

    public void RemoveCard(GameObject card)
    {
        Card.Suit suit = card.GetComponent<Card>().suit;
        Card.Rank rank = card.GetComponent<Card>().rank;
        int cardCheckSuitIndex = (int)suit - 1;
        int cardCheckRankIndex = rank == Card.Rank.Ace ? 0 : (int)rank - 1;
        cardCheck[cardCheckSuitIndex, cardCheckRankIndex] = false;
        Destroy(card);
    }

    public void Reset()
    {
        numSharedCards = 0;
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cards)
        {
            RemoveCard(card);
        }
    }
}
