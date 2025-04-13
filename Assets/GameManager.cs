using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  
using TMPro;
using UnityEditor.SearchService;

public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    public RectTransform nameTag, hintBox;
    public AnimationData[] playerAnimations;


    [Header("Local Scenes")]
    public Image blockingImage;
    public GameObject[] localScenes;
    int activeLocalScene = 0;
    public Transform[] playerStartPositions;

    [Header("Equipment")]
    public GameObject equipmentCanvas;
    public Image[] equipmentSlots, equipmentImages;
    public Sprite emptyItemSlotSprite;
    public Color selectedItemColor;
    public int selectedCanvasSlotID = 0, selectedItemID = -1;


    public static List<ItemData> collectedItems = new  List<ItemData>();
    static float moveSpeed = 3.5f, moveAccuracy = 0.15f;
    
    private bool isTransitioning = false;  








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
             hintBox.localPosition = new Vector2(Item.nameTagSize.x / 3, -1.5f);
         
    }

    public void HideHintBox()
    {
        hintBox.GetComponentInChildren<TextMeshProUGUI>().text = "";
        hintBox.sizeDelta = new Vector2(0, 0);
        hintBox.localPosition = new Vector2(0, 0);
    }

    //EQUIPMENT CODE
    public void SelectItem(int  equipmentCanvasID)
    {
        Color c = Color.white;
        c.a = 0;
        //change alpha of previous slot to 0
        equipmentSlots[selectedCanvasSlotID].color = c;

        if (equipmentCanvasID>= collectedItems.Count)
        {
            selectedItemID = -1;
            selectedCanvasSlotID = 0;
            return;
        }

        //change alpha of new slot to x
        equipmentSlots[equipmentCanvasID].color = selectedItemColor;

        selectedCanvasSlotID = equipmentCanvasID;
        selectedItemID = collectedItems[selectedCanvasSlotID].itemID;

    }

    public void ShowItemName(int equipmentCanvasID)
    {
        if (equipmentCanvasID < collectedItems.Count)
            UpdateNameTag(collectedItems[equipmentCanvasID]);

    }

    public void UpdateEquipmentCanvas()
    {
        //find out how many items we have and when to stop
        int itemsAmount = collectedItems.Count, itemSlotsAmount = equipmentSlots.Length;
        //replace no item sprites and old sprites with collectedItems[x].itemSlotSprite
        for (int i = 0; i < itemSlotsAmount; i++)
        {
            //choose between emptyItemSlotSprite and an item sprite
            if (i < itemsAmount && collectedItems[i].itemSlotSprite != null)
                equipmentImages[i].sprite = collectedItems[i].itemSlotSprite;
            else
                equipmentImages[i].sprite = emptyItemSlotSprite;
        }
        //add special conditions for selecting items
        if (itemsAmount == 0)
            SelectItem(-1);
        else if (itemsAmount == 1)
            SelectItem(0);
    }


    //SCENE TRANSITION CODE
    public void CheckSpecialConditions(ItemData item)

    {
        switch (item.itemID)
        {
            //if item id == something, go to scene 1
            //if item id == something, go to scene 2
            //if item id == something, end game

            case -10:
                StartCoroutine(ChangeScene(1, 0));
                break;

            case -11:
                StartCoroutine(ChangeScene(1, 0));
                break;

            case -12:
                StartCoroutine(ChangeScene(0, 0));
                break;

            case -13:
                StartCoroutine(ChangeScene(1, 0));
                break;

            case -14:
                StartCoroutine(ChangeScene(2, 1));
                break;

            case -15:
                StartCoroutine(ChangeScene(3, 0));
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
        if (sceneNumber < playerStartPositions.Length && playerStartPositions[sceneNumber] != null)
        {
            FindObjectOfType<ClickManager>().player.position = playerStartPositions[sceneNumber].position;
        }

        foreach (SpriteAnimator spriteAnimator in FindObjectsOfType<SpriteAnimator>())
        {
            spriteAnimator.PlayAnimation(null);
        }


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

    public void StartGame()
    {
    }



    public IEnumerator GlobalSceneTransition(int sceneIndex)
    {
        Color c = blockingImage.color;
        blockingImage.enabled = true;

        // Fade in
        while (blockingImage.color.a < 1)
        {
            c.a += Time.deltaTime;
            blockingImage.color = c;
            yield return null;
        }
        c.a = 1;
        blockingImage.color = c;

        // Load the new scene
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);

        // Give the new scene time to initialize
        yield return new WaitForSeconds(0.5f);

        foreach (SpriteAnimator spriteAnimator in FindObjectsOfType<SpriteAnimator>())
        {
            spriteAnimator.PlayAnimation(null);
        }

        // Fade out
        while (blockingImage.color.a > 0)
        {
            c.a -= Time.deltaTime;
            blockingImage.color = c;
            yield return null;
        }
        c.a = 0;
        blockingImage.color = c;
        blockingImage.enabled = false;

    }

    public void ChangeMainScene(int sceneIndex)
    {
        StartCoroutine(GlobalSceneTransition(sceneIndex));
    }





    //CUTSCENE CHANGE HANDLE CODE BELOW
    //UNSUBSCRIBING TO GM IN CASE OF SCENE CHANGE

    void Start()
    {

        BackgroundController.OnCutsceneEnd += HandleCutsceneEnd;
    }

    void OnDestroy()
    {
        BackgroundController.OnCutsceneEnd -= HandleCutsceneEnd;
    }

    void OnDisable()
    {
        BackgroundController.OnCutsceneEnd -= HandleCutsceneEnd;
    }



    private void HandleCutsceneEnd()
    {
        if (isTransitioning) return;  // Prevent multiple transitions
        isTransitioning = true;       // Lock transition

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(GlobalSceneTransition(nextSceneIndex));
        }
        else
        {
            Debug.LogWarning("No more scenes to load! Reached the end of the build order.");
        }
    }
    //Width: 19.16 * 2
    //height: 10.8 * 2

}
