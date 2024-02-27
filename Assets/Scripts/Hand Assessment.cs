using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class HandAssessment : MonoBehaviour
{
    private List<Card> sharedCards;
    private List<Card> cards;
    private List<Card> cardsSortedByRank;
    private List<Card> cardsSortedBySuit;
    public Player player;
    public TMP_Text text;

    public enum PokerHands
    {
        RoyalFlush = 10,
        StraightFlush = 9,
        FourOfAKind = 8,
        FullHouse = 7,
        Flush = 6,
        Straight = 5,
        ThreeOfAKind = 4,
        TwoPairs = 3,
        Pair = 2,
        HighCard = 1,
    }

    private void Awake()
    {
        sharedCards = new List<Card>();
    }

    public void CalculateHand()
    {
        FindAllCards();
        Debug.Log(GetBestHand().ToString());
        text.text = GetBestHand().ToString();
    }

    private void FindAllCards()
    {
        sharedCards.Clear();
        foreach (GameObject cardObj in player.dealer.sharedCards)
        {
            sharedCards.Add(cardObj.GetComponentInChildren<Card>());
        }
        cards = new List<Card>
        {
            player.GetFirstCard().GetComponent<Card>(),
            player.GetSecondCard().GetComponent<Card>(),
        };
        cards = cards.Concat(sharedCards).ToList();

        // Order cards by rank and suit
        // NOTE: List.Sort() is only stable for small lists
        cardsSortedByRank = new List<Card>(cards);
        cardsSortedByRank.Sort((x, y) => x.rank - y.rank);
        cardsSortedBySuit = new List<Card>(cardsSortedByRank);
        cardsSortedBySuit.Sort((x, y) => x.suit - y.suit);

        foreach (Card card in cardsSortedBySuit)
        {
            Debug.Log($"{card.rank} {card.suit}");
        }
    }

    public PokerHands GetBestHand()
    {
        if (CheckRoyalFlush())
            return PokerHands.RoyalFlush;
        else if (CheckStraightFlush())
            return PokerHands.StraightFlush;
        else if (CheckFourOfAKind())
            return PokerHands.FourOfAKind;
        else if (CheckFullHouse())
            return PokerHands.FullHouse;
        else if (CheckFlush())
            return PokerHands.Flush;
        else if (CheckStraight())
            return PokerHands.Straight;
        else if (CheckThreeOfAKind())
            return PokerHands.ThreeOfAKind;
        else if (CheckTwoPairs())
            return PokerHands.TwoPairs;
        else if (CheckPair())
            return PokerHands.Pair;
        else
            return PokerHands.HighCard;
    }

    private bool CheckRoyalFlush()
    {
        return CheckStraightFlush() && cards.Any(c => c.rank == Card.Rank.Ace);
    }

    private bool CheckStraightFlush()
    {
        if (!CheckFlush())
            return false;

        List<Card> possibleHandOne = cards.GetRange(0, 5);
        List<Card> possibleHandTwo = cards.GetRange(1, 5);
        List<Card> possibleHandThree = cards.GetRange(2, 5);

        if (CheckFlush(possibleHandOne) && CheckStraight(possibleHandOne))
            return true;
        if (CheckFlush(possibleHandTwo) && CheckStraight(possibleHandTwo))
            return true;
        if (CheckFlush(possibleHandThree) && CheckStraight(possibleHandThree))
            return true;
        return false;
    }

    private bool CheckFourOfAKind()
    {
        return cards.GroupBy(c => c.rank).Any(g => g.Count() == 4);
    }

    private bool CheckFullHouse()
    {
        return CheckThreeOfAKind() && CheckPair();
    }

    private bool CheckFlush()
    {
        return cards.GroupBy(c => c.suit).Any(g => g.Count() == 5);
    }

    // Checks flush for 5 cards
    private bool CheckFlush(List<Card> fiveCards)
    {
        return fiveCards.GroupBy(c => c.suit).Any(g => g.Count() == 5);
    }

    private bool CheckStraight()
    {
        var sortedRanks = cards.Select(c => (int)c.rank).OrderBy(r => r).Distinct().ToList();
        if (sortedRanks.Count < 5)
            return false;

        for (int i = 0; i < sortedRanks.Count - 1; i++)
        {
            if (sortedRanks[i + 1] - sortedRanks[i] != 1)
                return false;
        }
        return true;
    }

    private bool CheckStraight(List<Card> fiveCards)
    {
        var sortedRanks = fiveCards.Select(c => (int)c.rank).OrderBy(r => r).Distinct().ToList();
        if (sortedRanks.Count < 5)
            return false;

        for (int i = 0; i < sortedRanks.Count - 1; i++)
        {
            if (sortedRanks[i + 1] - sortedRanks[i] != 1)
                return false;
        }
        return true;
    }

    private bool CheckThreeOfAKind()
    {
        return cards.GroupBy(c => c.rank).Any(g => g.Count() == 3);
    }

    private bool CheckTwoPairs()
    {
        var groups = cards.GroupBy(c => c.rank);
        return groups.Count(g => g.Count() >= 2) >= 2;
    }

    private bool CheckPair()
    {
        return cards.GroupBy(c => c.rank).Any(g => g.Count() == 2);
    }

    private bool CheckHighCard()
    {
        return true;
    }


    // TODO: IMPLEMENT ALL METHODS TO FIND THE BEST HAND
    private List<Card> FindHighCardHand()
    {
        throw new NotImplementedException();
    }

    private List<Card> FindPairHand()
    {
        throw new NotImplementedException();
    }

    private List<Card> FindTwoPairHand()
    {
        throw new NotImplementedException();
    }

    private List<Card> FindThreeOfAKindHand()
    {
        throw new NotImplementedException();
    }
    private List<Card> FindStraightHand()
    {
        throw new NotImplementedException();
    }
    private List<Card> FindFlushHand()
    {
        throw new NotImplementedException();
    }
    private List<Card> FindFullhouseHand()
    {
        throw new NotImplementedException();
    }
    private List<Card> FindFourOfAKindHand()
    {
        throw new NotImplementedException();
    }
    private List<Card> FindStraightFlushHand()
    {
        throw new NotImplementedException();
    }

    private List<Card> FindRoyalFlushHand()
    {
        throw new NotImplementedException();
    }

}
