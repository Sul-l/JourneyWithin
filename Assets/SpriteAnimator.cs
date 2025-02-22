using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public SpriteRenderer mySpriteRenderer;
    public AnimationData baseAnimation;


    public IEnumerator PlayAnimation(AnimationData data)
    {
        int spritesAmount = 0;
        yield return null;
    }


}
