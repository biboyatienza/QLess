
using System.Text.RegularExpressions;
public static class CardService
{
  static List<Card> Cards {get;}
  static List<DiscountedCard> DiscountedCards {get;}
  static int nextId = 3;
  static int nextDiscountedCardId = 102;
  
  const decimal INITIAL_LOAD_NORMAL = 100m;
  const decimal INITIAL_LOAD_DISCOUNTED = 500m;
  static CardService()
  {
    Cards = new List<Card>
    {
      new Card {Id = 1, CardType = CardType.Normal, AmountLoad = 100m },
      new Card {Id = 2, CardType = CardType.Discounted, AmountLoad = 500m }
    };

    // Section C: discount, base of 20%, addt'l 3% for the next 4 rides/fare.
    DiscountedCards = new List<DiscountedCard>
    {
      new DiscountedCard {Id = 101, CardType = CardType.Discounted, AmountLoad = 500m, TripsInADay = 0, TripDayDate = DateTime.Now }
    };
  }

  public static List<Card> GetAll() => Cards;
  public static List<DiscountedCard> GetAllDiscountedCard() => DiscountedCards;

  public static Card? Get(int id) => Cards.FirstOrDefault(p => p.Id == id);
  public static DiscountedCard? GetDiscountedCard(int id) => DiscountedCards.FirstOrDefault(p => p.Id == id);
  
  public static void Add(Card card)
  {
    card.Id = nextId++;
    Cards.Add(card);
  }

  public static Card CreateNewCard(NewCard newCard)
  {
    CardType cardType = newCard.SeniorCitizenId == null && newCard.PwdId == null ? CardType.Normal : CardType.Discounted;

    Card card = new Card 
    {  
      CardType = cardType, 
      AmountLoad = cardType == CardType.Normal ? INITIAL_LOAD_NORMAL : INITIAL_LOAD_DISCOUNTED 
    };
    card.Id = nextId++;
    Cards.Add(card);
    return card;
  }

  public static DiscountedCard CreateDiscountedCard(NewCard newCard)
  {
    DiscountedCard discountedCard = new DiscountedCard 
    {  
      CardType = CardType.Discounted, 
      AmountLoad = INITIAL_LOAD_DISCOUNTED,
      TripDayDate = DateTime.Now,
      TripsInADay = 0 
    };
    discountedCard.Id = nextDiscountedCardId++;
    DiscountedCards.Add(discountedCard);
    return discountedCard;
  }

  public static void Update(Card card)
  {
    var index = Cards.FindIndex(p => p.Id == card.Id);
    if(index == -1) return;
    Cards[index] = card;
  }

  public static bool HasValidIdFormat(string idNumber, string patternText)
  {
    string idFormatError = string.Empty;
    Regex regEx = new Regex(patternText);
    return regEx.IsMatch(idNumber);
  }



  const decimal NORMAL_FARE = 15m;
  const decimal DISCOUNTED_FARE = 10m;
   const int NORMAL_VALIDITY_IN_YEARS_AFTER_USED = 5;
   const int DISCOUNTED_VALIDITY_IN_YEARS_AFTER_USED = 3;

  public static string HasValidLoad(Card card)
  {
     DateTime? validUntil = card.ValidUntil.HasValue ? card.ValidUntil : DateTime.Now.AddYears(DISCOUNTED_VALIDITY_IN_YEARS_AFTER_USED);  
    string cardErrors = string.Empty;
    string newKeyword = card.CardType == CardType.Normal ? string.Empty : "Discounted";

    if(DateTime.Now > validUntil)
    {
       cardErrors += string.Format("\nQ-LESS {0}Transport Card is now expired, with validity date of {1}", newKeyword ,validUntil.ToString()); 
    }

    decimal minimumFare = card.CardType == CardType.Normal ? NORMAL_FARE : DISCOUNTED_FARE;
    if(card.AmountLoad < minimumFare)
    {
       cardErrors += string.Format("\nQ-LESS {0}Transport Card is now below minimum fare amout of {1}, current load balance is {2}.", newKeyword ,minimumFare, card.AmountLoad); 
    }

    return cardErrors;
  } 

  public static string HasValidDiscountedLoad(DiscountedCard card, decimal discount)
  {
     DateTime? validUntil = card.ValidUntil.HasValue ? card.ValidUntil : DateTime.Now.AddYears(DISCOUNTED_VALIDITY_IN_YEARS_AFTER_USED);  
    string cardErrors = string.Empty;
    string newKeyword = "Discounted";

    if(DateTime.Now > validUntil)
    {
       cardErrors += string.Format("\nQ-LESS {0}Transport Card is now expired, with validity date of {1}", newKeyword ,validUntil.ToString()); 
    }

    decimal minimumFare = DISCOUNTED_FARE - (DISCOUNTED_FARE * discount);
    if(card.AmountLoad < minimumFare)
    {
       cardErrors += string.Format("\nQ-LESS {0}Transport Card is now below minimum fare amout of {1}, current load balance is {2}.", newKeyword ,minimumFare, card.AmountLoad); 
    }

    return cardErrors;
  } 



  public static void FarePayment(Card card)
  {
    var index = Cards.FindIndex(p => p.Id == card.Id);

    decimal minimumFare = card.CardType == CardType.Normal ? NORMAL_FARE : DISCOUNTED_FARE;
    card.AmountLoad -= minimumFare;
    card.ValidUntil = DateTime.Now.AddYears(card.CardType == CardType.Normal ? NORMAL_VALIDITY_IN_YEARS_AFTER_USED : DISCOUNTED_VALIDITY_IN_YEARS_AFTER_USED);

    Cards[index] = card;
  }

  public static void DiscountedFarePayment(DiscountedCard discountedCard, decimal discount)
  {
    var index = DiscountedCards.FindIndex(p => p.Id == discountedCard.Id);
    decimal minimumFare = DISCOUNTED_FARE - (DISCOUNTED_FARE * discount);
    discountedCard.AmountLoad -= minimumFare;
    discountedCard.ValidUntil = DateTime.Now.AddYears(DISCOUNTED_VALIDITY_IN_YEARS_AFTER_USED);
    DiscountedCards[index] = discountedCard;
  }

}