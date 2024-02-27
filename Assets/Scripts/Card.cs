using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum Suit
    {
        Diamond = 1,
        Club = 2,
        Heart = 3,
        Spade = 4,
    }

    public enum Rank
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14,
        
    }

    private SpriteRenderer back;
    private SpriteRenderer front;
    public Suit suit;
    public Rank rank;
    private int rankValue;
    public static Suit[] suits = (Suit[])Enum.GetValues(typeof(Suit));
    public static Rank[] ranks = (Rank[])Enum.GetValues(typeof(Rank));

    public void Awake()
    {
        back = GetComponentsInChildren<SpriteRenderer>()[1];
        front = GetComponentInChildren<SpriteRenderer>();
    }

    public void Update()
    {
        front.sprite = getFrontSpriteResource();
        back.sprite = Resources.Load<Sprite>("Images/card back red");
    }

    private Sprite getFrontSpriteResource()
    {
        string rankString = rank.ToString().ToLower();
        rankValue = (int)rank;
        if (rankValue >= 2 && rankValue <= 10)
        {
            rankString = rankValue.ToString();
        }

        return Resources.Load<Sprite>($"Images/card fronts/{suit}s/{rankString}_of_{suit.ToString().ToLower()}s");
    }
}

