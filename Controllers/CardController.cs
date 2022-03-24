using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CardController : ControllerBase
{
  public CardController()
  {
  }

  // GET all action
  [HttpGet]
  public ActionResult<List<Card>> GetAll() => CardService.GetAll();


  // GET by Id action
  [HttpGet("{id}")]
  public ActionResult<Card> Get(int id)
  {
    var card = CardService.Get(id);
    return card == null ? NotFound() : card;
  }

  // POST action
  [HttpPost]
  public IActionResult Create(Card card)
  {
    // Console.WriteLine(card.AmountLoad);
    CardService.Add(card);
    return CreatedAtAction(nameof(Create), new { id = card.Id }, card);
  }

  const string SENIOR_CITIZEN_ID_PATTERN_TEXT = @"^([a-zA-Z0-9]{2})\-{1}([a-zA-Z0-9]{4})\-{1}([a-zA-Z0-9]{4})";
  const string PWD_ID_PATTERN_TEXT = @"^([a-zA-Z0-9]{4})\-{1}([a-zA-Z0-9]{4})\-{1}([a-zA-Z0-9]{4})";


  [HttpPost("NewCard")]
  public IActionResult NewCard(NewCard newCard)
  {
    string idErrors = "";
    if(newCard.SeniorCitizenId != null)
    {
      if(!CardService.HasValidIdFormat(newCard.SeniorCitizenId, SENIOR_CITIZEN_ID_PATTERN_TEXT))
        idErrors = "\nInvalid Senior Citizen Control Number, it should be a 10-character length string with “##-####-####“ format.";
    }

    if(newCard.PwdId != null)
    {
      if(!CardService.HasValidIdFormat(newCard.PwdId, PWD_ID_PATTERN_TEXT))
        idErrors += "\nInvalid PWD ID Number is a 12-character length string with “####-####-####” format.";
    }

    if(idErrors.Length > 0) return BadRequest(idErrors);

    Card createdCard = CardService.CreateNewCard(newCard);
    return CreatedAtAction(nameof(NewCard), new { id = createdCard.Id }, createdCard);
  }
  
  [HttpPost("DiscountedCard")]
  public IActionResult DiscountedCard(NewCard newCard)
  {
    string idErrors = "";
    if(newCard.SeniorCitizenId == null && newCard.PwdId == null) return BadRequest("Please supply seniorCitizenId or pwdId."); 

    if(newCard.SeniorCitizenId != null)
    {
      if(!CardService.HasValidIdFormat(newCard.SeniorCitizenId, SENIOR_CITIZEN_ID_PATTERN_TEXT))
        idErrors = "\nInvalid Senior Citizen Control Number, it should be a 10-character length string with “##-####-####“ format.";
    }

    if(newCard.PwdId != null)
    {
      if(!CardService.HasValidIdFormat(newCard.PwdId, PWD_ID_PATTERN_TEXT))
        idErrors += "\nInvalid PWD ID Number is a 12-character length string with “####-####-####” format.";
    }
    if(idErrors.Length > 0) return BadRequest(idErrors);

    DiscountedCard createdDiscountedCard = CardService.CreateDiscountedCard(newCard);
    return CreatedAtAction(nameof(DiscountedCard), new { id = createdDiscountedCard.Id }, createdDiscountedCard);
  } 

  // PUT action
  [HttpPut("{id}")]
  public IActionResult Update(int id, Card card)
  {
    if(id != card.Id) return BadRequest();
    var existingCard = CardService.Get(id);
    if(existingCard is null) return NotFound();
    CardService.Update(card);
    return NoContent();
  }

  [HttpGet("DiscountedCard")]
  public ActionResult<List<DiscountedCard>> GetAllDiscountedCard() => CardService.GetAllDiscountedCard();

  [HttpGet("DiscountedCard/{id}")]
  public ActionResult<DiscountedCard> GetDiscountedCard(int id)
  {
    var card = CardService.GetDiscountedCard(id);
    return card == null ? NotFound() : card;
  }


  [HttpPut("FarePayment/{id}")]
  public IActionResult FarePayment(int id)
  {
    Card? existingCard = CardService.Get(id);
    if(existingCard is null) return NotFound();
    string cardErrors = CardService.HasValidLoad(existingCard);
    if(cardErrors.Length > 0) return BadRequest(cardErrors); 
    CardService.FarePayment((Card) existingCard);
    return NoContent();
  }

// How much is the discount;
    const decimal BASE_DISCOUNT = 0.2m;
    const decimal ADDITIONAL_DISCOUNT = 0.03m;


  [HttpPut("DiscountedCard/{id}")]
  public IActionResult DiscountedFarePayment(int id)
  {
    DiscountedCard? existingCard = CardService.GetDiscountedCard(id);
    if(existingCard is null) return NotFound();

    DateTime now = DateTime.Now;
    DateTime tripDayDate = existingCard.TripDayDate.HasValue ? (DateTime) existingCard.TripDayDate : DateTime.Now; 
    decimal totalDiscount = 0m;

    if(now.ToString("dd.MMM.yyyy") == tripDayDate.ToString("dd.MMM.yyyy"))
    {
      if((existingCard.TripsInADay >= 1 && existingCard.TripsInADay < 5))
      {
        totalDiscount = BASE_DISCOUNT + ADDITIONAL_DISCOUNT;
      }
      else
      {
        totalDiscount = BASE_DISCOUNT;
      }
      existingCard.TripsInADay += 1;
    }
    else
    {
      totalDiscount = BASE_DISCOUNT;
      existingCard.TripsInADay = 1;
    }
    existingCard.TripDayDate = now;


    string cardErrors = CardService.HasValidDiscountedLoad (existingCard, totalDiscount);
    if(cardErrors.Length > 0) return BadRequest(cardErrors); 
    CardService.DiscountedFarePayment((DiscountedCard) existingCard, totalDiscount);
    return NoContent();
  }
}