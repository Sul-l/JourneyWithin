using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public bool playerWalking;
    public Transform player;
    GameManager gameManager;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }


    public void GoToItem(ItemData item)
    {
        StartCoroutine(gameManager.MoveToPoint(player,item.GoToPoint.position));
        player.GetComponent<SpriteAnimator>().PlayAnimation(gameManager.playerAnimations[1]);
        playerWalking = true;
        TryGettingItem(item); 
        //this is a test to see if the item can be picked up.
    }

    


    void TryGettingItem(ItemData item)
    {
        bool canGetItem = item.requiredItemID == -1 || GameManager.collectedItems.Contains(item);
        if (canGetItem)
        {
            GameManager.collectedItems.Add(item);
        }
        StartCoroutine(UpdateSceneAfterAction(item, canGetItem));

    }




    private IEnumerator UpdateSceneAfterAction(ItemData item, bool canGetItem)
    {
        while (playerWalking) 
            yield return new WaitForSeconds(0.05f);
        if (canGetItem) //removes the item if the method check is correct. 
                        //Item has to be added to objectstoremove list.
        {
            foreach (GameObject g in item.objectsToRemove)
                Destroy(g);
            player.GetComponent<SpriteAnimator>().PlayerAnimation(null);
            Debug.Log("Item Picked up");
        }

        else
        {
            gameManager.UpdateHintBox(item, player.GetComponentInChildren<SpriteRenderer>().flipX);
            gameManager.CheckSpecialConditions(item);


            yield return null;
        }
    }
}