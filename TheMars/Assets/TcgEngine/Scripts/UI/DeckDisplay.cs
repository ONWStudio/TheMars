using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TcgEngine.UI
{
    /// <summary>
    /// Can display a deck in the UI
    /// Only shows a few cards and the total amount of cards
    /// </summary>

    public class DeckDisplay : MonoBehaviour
    {
        public Text deck_title;
        public Text card_count;
        public CardUI[] ui_cards;

        private string deck_id;

        private void Awake()
        {
            Clear();
        }

        private void Update()
        {

        }

        public void Clear()
        {
            if (deck_title != null)
                deck_title.text = "";
            if (card_count != null)
                card_count.text = "";
            foreach (CardUI card in ui_cards)
                card.Hide();
        }

        public void SetDeck(string tid)
        {
            UserData user = Authenticator.Get().UserData;
            UserDeckData udeck = user.GetDeck(tid);
            DeckData ddeck = DeckData.Get(tid);
            if (udeck != null)
                SetDeck(udeck);
            else if (ddeck != null)
                SetDeck(ddeck);
            else
                Clear();
        }

        public void SetDeck(UserDeckData deck)
        {
            Clear();

            if (deck != null)
            {
                deck_id = deck.tid;

                if (deck_title != null)
                    deck_title.text = deck.title;

                if (card_count != null)
                {
                    card_count.text = deck.GetQuantity().ToString() + " / " + GameplayData.Get().deck_size.ToString();
                    card_count.color = deck.GetQuantity() >= GameplayData.Get().deck_size ? Color.white : Color.red;
                }

                List<CardDataQ> cards = new();
                foreach (UserCardData ucard in deck.cards)
                {
                    CardDataQ card = new()
                    {
                        card = CardData.Get(ucard.tid),
                        variant = VariantData.Get(ucard.variant),
                        quantity = ucard.quantity
                    };
                    if (card.card != null)
                        cards.Add(card);
                }

                ShowCards(cards);
            }

            gameObject.SetActive(deck != null);
        }

        public void SetDeck(DeckData deck)
        {
            Clear();

            if (deck != null)
            {
                deck_id = deck.id;

                if (deck_title != null)
                    deck_title.text = deck.title;

                if (card_count != null)
                {
                    card_count.text = deck.GetQuantity().ToString() + " / " + GameplayData.Get().deck_size.ToString();
                    card_count.color = deck.GetQuantity() >= GameplayData.Get().deck_size ? Color.white : Color.red;
                }

                List<CardDataQ> dcards = new List<CardDataQ>();
                VariantData variant = VariantData.GetDefault();
                foreach (CardData icard in deck.cards)
                {
                    if (icard != null)
                    {
                        dcards.Add(new()
                        {
                            card = icard,
                            variant = variant,
                            quantity = 1
                        });
                    }
                }

                if (deck is DeckPuzzleData pdeck)
                {
                    foreach (DeckCardSlot slot in pdeck.board_cards)
                    {
                        if (slot.card != null)
                        {
                            dcards.Add(new()
                            {
                                card = slot.card,
                                variant = variant,
                                quantity = 1
                            });
                        }
                    }
                }

                ShowCards(dcards);
            }

            gameObject.SetActive(deck != null);
        }

        public void ShowCards(List<CardDataQ> cards)
        {
            cards.Sort((CardDataQ a, CardDataQ b) => { return b.card.Mana.CompareTo(a.card.Mana); });

            int index = 0;
            foreach (CardDataQ icard in cards)
            {
                for (int i = 0; i < icard.quantity; i++)
                {
                    if (index < ui_cards.Length)
                    {
                        CardUI card_ui = ui_cards[index];
                        card_ui.SetCard(icard.card, icard.variant);
                        index++;
                    }
                }
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public string GetDeck()
        {
            return deck_id;
        }
    }
}
