public enum CardType 
{
  Normal,
  Discounted
}
public class Card
{
  public int Id {get; set;}
  public CardType CardType { get; set; }
  public decimal AmountLoad { get; set; }
  public DateTime? ValidUntil { get; set; }
}