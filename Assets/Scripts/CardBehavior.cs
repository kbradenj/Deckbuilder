using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardBehavior : MonoBehaviour
{
    //TMP
    public TextMeshProUGUI cardNameField;
    public TextMeshProUGUI descriptionField;
    public TextMeshProUGUI costField;
    public Image image;

    //Scripts
    public Player player;
    public Character targetCharacter;
    public Card card;
    public Actions actions;

    //Game Objects
    public GameObject Canvas;
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject dropZone;
    private GameObject target;

    //States
    private bool isDragging = false;
    private bool isOverDropZone = false;
    
    void Start()
    {
        Canvas = GameObject.Find("Main Canvas");
        dropZone = GameObject.Find("Drop Zone");
        player = FindObjectOfType<Player>();
        actions = FindObjectOfType<Actions>();
    }

    void Update()
    {
        if(isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }
    }

    //Populate Card Data to UI
    public void RenderCard(Card c)
    {
        card = c;
        cardNameField.text = c.cardName;
        descriptionField.text = c.cardDescription;
        costField.text = c.cardCost.ToString(); 
        image.sprite = c.cardImage;
    }

    //Card Interaction
    private void OnCollisionEnter2D(Collision2D collision)
    {
       isOverDropZone = true;
       if(card.needsTarget && collision.gameObject.tag == "enemy")
       {
       target = collision.gameObject;
       target.GetComponent<Enemy>().Highlight();
       }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        if(target != null && target.tag == "enemy"){
         target.GetComponent<Enemy>().StopHighlight();
         target = null;
        }
    }

    public void StartDrag()
    {
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void EndDrag()
    {
        isDragging = false;

        if (!isOverDropZone)
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
            return;
        }

        if (!IsCardPlayable())
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
            return;
        }

        if ((card.needsTarget && target != null) || !card.needsTarget)
        {
            Play(target);
            Destroy(this.gameObject);
        }
        else{
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
            return;
        }
    }

    public void Play(GameObject target)
    {   if(target != null){
            targetCharacter = target.GetComponent<Character>();
        }
        else
        {
            targetCharacter = player;
        }
        foreach(string type in card.actionList)
        {
            switch(type)
            {
                case "attack":
                actions.Attack(targetCharacter, card.attack, 1);
                break;
                case "block":
                if(target.tag == "enemy")
                    {
                        actions.Block(player, card.block);
                    }
                    else
                    {
                        actions.Block(targetCharacter, card.block);
                    }
                break;
                case "vulnerable":
                actions.Vulnerable(targetCharacter, card.vulnerable);
                break;
                case "weak":
                actions.Weak(targetCharacter, card.weak);
                targetCharacter.NextTurn();
                Debug.Log(targetCharacter.weaknessMod);
                break;
            }
        }   
        
        player.turnAP -= card.cardCost;
        targetCharacter.UpdateStats();
        player.UpdateStats();
    }

    public bool IsCardPlayable()
    {
        if(player.turnAP - card.cardCost >= 0){
            return true;
        } else
        {
            return false;
        }
    }
}
