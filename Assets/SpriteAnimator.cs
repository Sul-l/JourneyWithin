using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public SpriteRenderer mySpriteRenderer;
    public AnimationData baseAnimation;
    Coroutine previousAnimation;
    GameManager gameManager;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {

            PlayAnimation(baseAnimation);
    }

    public void PlayAnimation(AnimationData data)
    {
        if (previousAnimation != null)
            StopCoroutine(previousAnimation);
       
            previousAnimation = StartCoroutine(PlayAnimationCoroutine(data));
    }

    public IEnumerator PlayerAnimation(AnimationData data)
    {
        int spriteAmount = data.sprites.Length, i=0;
        float waitTime = data.framesOfGap * AnimationData.targetFrameTime;
        while (i < spriteAmount)
        {
            //play sound
            //change sprite and increase i
            mySpriteRenderer.sprite = data.sprites[i++];
            yield return new WaitForSeconds(waitTime);
            

            if (data.loop && i>=spriteAmount)
                i = 0;
        }
        yield return null;
    }

    public IEnumerator PlayAnimationCoroutine(AnimationData data)
    {
        int spritesAmount = data.sprites.Length, i = 0;
        float waitTime = data.framesOfGap * AnimationData.targetFrameTime;
        //change sprites
        while (i < spritesAmount)
        {
            //play sound
            
            //change sprite and increase i
            mySpriteRenderer.sprite = data.sprites[i++];
            yield return new WaitForSeconds(waitTime);

            //looping
            if (data.loop && i >= spritesAmount)
                i = 0;
        }
        if (data.returnToBase && data != baseAnimation)
            PlayAnimation(baseAnimation);
        yield return null;
    }
}