using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CardBehavior : MonoBehaviour
{
    //TMP
    public TextMeshProUGUI cardNameField;
    public TextMeshProUGUI descriptionField;
    public TextMeshProUGUI costField;
    public TMP_Text priceText;
    public TMP_Text quantity;

    //Game State (TODO: Change to battle when i break it out)
    private GameState gameState;
    private CardManager cardManager;

    //UI
    public Image image;
    public Image bgImage;

    //Scripts
    public Player player;
    public Character targetCharacter;
    public Card card;
    public ActionManager actions;
    public Singleton singleton;
    
    //Game Objects
    public GameObject Canvas;
    public GameObject highlightScreen;
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject dropZone;
    private GameObject target;
    private GameObject[] enemies;
    public GameObject inspectCardPrefab;
    public GameObject pricePrefab;
    public GameObject amountDisplay;

    //States
    private bool isDragging = false;
    private bool isOverDropZone = false;
    public bool isDisabled = false;
    public int qty;
    
    void Awake()
    {
        gameState = FindObjectOfType<GameState>();
        Canvas = GameObject.Find("Main Canvas");
        dropZone = GameObject.Find("Drop Zone");
        singleton = FindObjectOfType<Singleton>();
        cardManager = FindObjectOfType<CardManager>();
        player = singleton.player;
        actions = FindObjectOfType<ActionManager>();
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Battle"){
            if(isDragging)
            {
                transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                transform.SetParent(Canvas.transform, true);
                if(!card.needsTarget && card.cardType != "power" && card.actionList[0] == "attackall"){
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach(GameObject enemy in enemies){
                        enemy.GetComponent<Enemy>().Highlight();
                    }
                }
            }
            IsCardPlayable();
        }
    }

    //Card Close-up
    public void CloseUp(){
       
            GameObject cardToDestroy = GameObject.FindGameObjectWithTag("Close Up");
            Destroy(cardToDestroy);
        if(!isDragging){
            GameObject closeUpCard = GameObject.Instantiate(inspectCardPrefab, new Vector2(Screen.width/2f,Screen.height/2f), Quaternion.identity) as GameObject;
            InspectCard inspectCard = closeUpCard.GetComponent<InspectCard>();
            inspectCard.RenderCard(card);
            closeUpCard.tag = "Close Up";
            closeUpCard.transform.SetParent(Canvas.transform);
        }
    }



    //Populate Card Data to UI
    public void RenderCard(Card c, bool showQty = false)
    {
        card = c;
        cardNameField.text = c.cardName;
        if(singleton.isBattle)
        {
            card.modDamage = (int) Math.Floor((card.attack * player.weaknessMod) + player.attackBoost + player.strength + player.baseStrength);
            card.modBlock = card.block + player.dexterity + player.baseDexterity;        
        }
        else
        {
            card.modDamage = card.attack;
            card.modBlock = card.block;
        }

        descriptionField.text = c.FormatString();
        if(c.actionList.Count != 0)
        {
            if(c.actionList[0] == "xattack" || c.actionList[0] == "xblock"){
                costField.text = "X"; 
            }
            else{
                costField.text = c.cardCost.ToString(); 
            }
        }
        else
        {
            costField.text = c.cardCost.ToString();
        }
           
        image.sprite = c.cardImage;
        if(showQty != true && !isDisabled){
            GameObject.Find("QuantityDisplay").SetActive(false);
        }
        if(isDisabled)
            {
                Image[] images = gameObject.GetComponentsInChildren<Image>();
                foreach(Image image in images)
                {
                    image.color = Color.gray;
                }
        }
    }

    public void AddPrice(GameObject card, int craftCost)
    {
        GameObject craftCostObject = GameObject.Instantiate(pricePrefab, new Vector2(0,250), Quaternion.identity);
        craftCostObject.transform.SetParent(card.transform);
        priceText = craftCostObject.GetComponentInChildren<TMP_Text>();
        priceText.text = "Crafting Time: " + craftCost.ToString() + " min";
    }

    public void UpdatePriceText(string textToUpdate){
        priceText.text = textToUpdate;

    }

    public void UpdateQuantity(int amount)
    {
        quantity.text = amount.ToString();
    }

    //Card Interaction
    private void OnCollisionEnter2D(Collision2D collision)
    {

       isOverDropZone = true;
       if(target != null)
       {
          target.GetComponent<Enemy>().StopHighlight();
       }
        if(!isDisabled)
        {
            if(card.needsTarget && collision.gameObject.tag == "Enemy")
       {    
            target = collision.gameObject;
            Enemy enemy = target.GetComponent<Enemy>();
            enemy.Highlight();

            //Format card to show value based on vulnerable, weak, etc
            foreach(string action in card.actionList)
                {
                    //for attack type actions (TODO: create for skill items as well);
                    if(action == "attack")
                    {
                        if(enemy.vulnerable > 0)
                        {
                            enemy.vulnerableMod = 1.25f;
                        }
                        if(player.weak > 0)
                        {
                            player.weaknessMod = .5f;
                        }
                        card.modDamage = (int) Math.Floor((card.attack * player.weaknessMod * enemy.vulnerableMod) + player.attackBoost + player.strength + player.baseStrength);
                        descriptionField.text = card.FormatString();
                        card.modDamage = card.attack;
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        //backup to stop highlight on the enemy
        if(collision.gameObject.tag == "Enemy")
        {
            card.modDamage = (int) Math.Floor((card.attack * player.weaknessMod) + player.attackBoost + player.strength + player.baseStrength);
            descriptionField.text = card.FormatString();
            collision.gameObject.GetComponent<Enemy>().StopHighlight();
        }
        else
        {
            //if needs target and leaves the enemy area
            if(card.needsTarget && collision.gameObject.tag == "Enemy Area"){
                target = null;
            }
            //if doesn't need target but leaves dropzone
            else if(collision.gameObject.tag == "Dropzone")
            {
                isOverDropZone = false;
                target = null;
            }

        }

    
    }

    public void StartDrag()
    {
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    //TODO: Clean this up, it's a confusing mess. It should break out if it requires a target after checking if it's playable. Dropzone should only need to worry about nontarget cards
    public void EndDrag()
    {
        isDragging = false;
        if(isOverDropZone && IsCardPlayable())
        {
            //if card needs target and has one or doesn't need target
            if((card.needsTarget && target != null) || !card.needsTarget){
                cardManager.handCardObjects.Remove(this.gameObject);
                //if it's an attack all card
                if(card.cardType != "power" && card.actionList[0] == "attackall"){
                    foreach(GameObject enemy in enemies){
                        target = enemy;
                        Play(target);
                    }
                }
                else
                {
                    Play(target);
                }
                
                //reduce AP
                player.turnAP -= card.cardCost;

                //show reduction in ap/block/health/etc.
                player.UpdateStats();

                //destroy card
                cardManager.MoveFromHandToDiscard(card);
                cardManager.UpdateDeckSizeText();
                cardManager.handCardObjects.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                //return to hand
                transform.position = startPosition;
                transform.SetParent(startParent.transform, false);
            
            }
        } 
        else
            {
                //return to hand
                transform.position = startPosition;
                transform.SetParent(startParent.transform, false);
            }
        //remove highlight on all enemies for attack all types of cards
        if(!card.needsTarget && card.cardType != "power" && card.actionList[0] == "attackall"){
                foreach(GameObject enemy in enemies){
                    enemy.GetComponent<Enemy>().StopHighlight();
            }
        }
    }

    public void Play(GameObject target)
    {   if(target == null){
            targetCharacter = player;  
        }
        else
        {
           targetCharacter = target.GetComponent<Character>(); 
        }
        if(card.cardType == "power")
        {
            if(card.phase == "oneTime")
            {
                card.Effect();
            }
            else
            {
                GameObject.FindObjectOfType<GameState>().powerCards[card.phase].Add(card.cardName, card);
            }

        }
            foreach(string type in card.actionList)
            {
                int attackOut = 0;
                if(type == "attack" || type == "xattack" || type == "attackAll")
                {
                    attackOut = (int)Math.Floor((card.attack + player.strength + player.baseStrength + player.attackBoost) * player.weaknessMod);
                }

                switch(type)
                {
                    case "attack":
                        actions.Attack(targetCharacter, attackOut, card.multiAction);
                    break;
                    case "xattack":
                        actions.Attack(targetCharacter, attackOut, player.turnAP);
                        player.turnAP = 0;
                    break;
                    case "attackAll":
                        enemies = GameObject.FindGameObjectsWithTag("Enemy");
                        foreach(GameObject enemy in enemies)
                        {
                            targetCharacter = enemy.GetComponent<Enemy>();
                            if(card.cardName == "Advantage")
                            {
                                attackOut = targetCharacter.weak;
                            }
                            actions.Attack(targetCharacter, attackOut , card.multiAction);
                        }
                    break;
                    case "block":
                        actions.Block(player, card.block + player.baseDexterity + player.dexterity);
                    break;
                    case "xblock":
                        for(int i = 0; i < player.turnAP; i++){
                            actions.Block(player, card.block);
                        }
                        player.turnAP = 0;
                    break;
                    case "vulnerable":
                        actions.AddEffect(targetCharacter, card.vulnerable, ref targetCharacter.vulnerable, "vulnerable");
                    break;
                    case "weak":
                        actions.AddEffect(targetCharacter, card.weak, ref targetCharacter.weak, "weak");
                        targetCharacter.NextTurn();
                    break;
                    case "strength":
                        actions.AddEffect(player, card.strength, ref player.strength, "strength");
                        targetCharacter.NextTurn();
                    break;
                    case "draw":
                        player.cardManager.Draw(card.draw);
                    break;
                    case "attackBoost":
                        actions.AddEffect(player, card.attackBoost, ref player.attackBoost, "attackBoost");
                    break;
                    case "evade":
                        actions.AddEffect(player, card.evade, ref player.evade, "evade");
                    break;
                }
            }   
        
    }

    //Does player have enough ap to play?
    public bool IsCardPlayable()
    {
        if(player.turnAP - card.cardCost >= 0 && !isDisabled){
            bgImage.color = Color.green;
            return true;
        } else
        {
            bgImage.color = Color.black;
            return false;
        }
    }

        //Card Highlight
        public void HighlightSelection()
        {
            highlightScreen.SetActive(true);
        }

        public void StopHighlightSelection()
        {
            highlightScreen.SetActive(false);
        }
}
