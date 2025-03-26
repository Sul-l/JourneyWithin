using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Header("Setup")]
    public int itemID, requiredItemID;
    public Transform GoToPoint;
    public string objectName;
    public Vector2 nameTagSize = new Vector2(3, 0.65f);

    [Header("Item picked up")]
    public GameObject[] objectsToRemove;

    [Header("Fail/Cant pick up")]
    [TextArea(3,3)]
    public string hintMessage;
    public Vector2 hintBoxSize = new Vector2(3,0.65f);
}
