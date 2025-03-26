using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  
using TMPro;
using UnityEditor.SearchService;

public class GameManager : MonoBehaviour
{
    public static List<int> collectedItems = new  List<int>();
    static float moveSpeed = 3.5f, moveAccuracy = 0.15f;
    public RectTransform nameTag, hintBox;
    public Image blockingImage;
    public GameObject[] localScenes;
    int activeLocalScene = 0;
    public Transform[] playerStartPositions;



    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)
    {
        Vector2 positionDifference = point - (Vector2)myObject.position;

        SpriteRenderer spriteRenderer = myObject.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null && positionDifference.x != 0)
            spriteRenderer.flipX = positionDifference.x < 0;  //Flip character code

        while (positionDifference.magnitude > moveAccuracy)
        {
            myObject.Translate(moveSpeed * positionDifference.normalized * Time.deltaTime);
            positionDifference = point - (Vector2)myObject.position;
            yield return null;
        }
        myObject.position = point;
        if (myObject == FindObjectOfType<ClickManager>().player)
            FindObjectOfType<ClickManager>().playerWalking = false;
        yield return null;
    }
    public void UpdateNameTag(ItemData Item)
    {
        nameTag.GetComponentInChildren<TextMeshProUGUI>().text = Item.objectName;
        nameTag.sizeDelta = Item.nameTagSize;
        nameTag.localPosition = new Vector2(Item.nameTagSize.x/2, -1.5f);
    }

    public void UpdateHintBox(ItemData Item, bool playerFlipped)
    {
        hintBox.GetComponentInChildren<TextMeshProUGUI>().text = Item.hintMessage;
        //change name
        hintBox.sizeDelta = Item.hintBoxSize;
        //change the size of the box
        if (playerFlipped)
             hintBox.localPosition = new Vector2(Item.nameTagSize.x / 2, -1.5f);
         
    }


    public void CheckSpecialConditions(ItemData item)

    {
        switch (item.itemID)
        {
            //if item id == something, go to scene 1
            //if item id == something, go to scene 2
            //if item id == something, end game


            case -11:
                StartCoroutine(ChangeScene(1, 0));
                break;

            case -12:
                StartCoroutine(ChangeScene(0, 0));
                break;

            case -1:
                StartCoroutine(ChangeScene(2, 1));
                break;
        }
    }



        public IEnumerator ChangeScene(int sceneNumber, float delay)
    {
        Color c = blockingImage.color;
        blockingImage.enabled = true;



        while (blockingImage.color.a < 1)
        {
            c.a += Time.deltaTime;
            blockingImage.color = c;
            yield return null;
        }
        c.a = 1;
        blockingImage.color = c;

        // Wait for the specified delay
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        localScenes[activeLocalScene].SetActive(false);
        localScenes[sceneNumber].SetActive(true);
        activeLocalScene = sceneNumber;
        FindObjectOfType<ClickManager>().player.position = playerStartPositions[sceneNumber].position;



        while (blockingImage.color.a > 0)
        {
            c.a -= Time.deltaTime;
            blockingImage.color = c;
            yield return null;
        }
        blockingImage.color = c;
        blockingImage.enabled = false;

        yield return null;
    }

    public void RestartGame()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Start()
    {
        StartCoroutine(ChangeScene(0, 0));
    }




}



