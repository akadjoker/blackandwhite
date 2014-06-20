using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace flashpunk
{
    using flashpunk.utils;

    public class AnimatedEntity : Entity
    {

        [System.Serializable]
        public class SpriteAnimation
        {
            public string animName;
            public UnityEngine.Sprite[] sprites;
            public WrapMode wrapMode;
            public int fps;
        }



        public WrapMode defaultWrapMode;
        public bool bPlayOnStart = true;
        public int iPlayOnStartIndex = 0;
        public SpriteAnimation[] animList;
        public int iFrame = 0;
        bool bPlaying, bPaused;
        bool bChangingFrame;
        int iLastAnimation;
        bool pong;

        override public void Start()
        {
            base.Start();
            if (bPlayOnStart) Play(iPlayOnStartIndex);	
        }
         /*
          * public void AddAnimation()
        {
            if (animList == null) animList = new SpriteAnimation[0];
            animList.CopyTo(animList = new SpriteAnimation[animList.Length + 1], 0);

            SpriteAnimation tempSprite = new SpriteAnimation();
            tempSprite.animName = "New Animation";
            tempSprite.wrapMode = WrapMode.Default;
            tempSprite.fps = 30;

            animList[animList.Length - 1] = tempSprite;

        }
    */
        public void RemoveAnimation(ref int index)
        {
            RemoveAt<SpriteAnimation>(ref animList, ref index);
        }





        //PLAYS
        public void Play()
        {
            PerformPlay(0);
        }
        public void Play(string anim)
        {
            for (int i = 0; i < animList.Length; i++)
                if (animList[i].animName == anim)
                {
                    PerformPlay(i);
                    break;
                }
        }
        public void Play(int animIndex)
        {
            PerformPlay(animIndex);
        }


        void PerformPlay(int animIndex)
        {
            if (animIndex != iLastAnimation || bPaused || !bPlaying)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeSprite(animIndex));
            }
        }

        public void EditorPlay(int animIndex)
        {
            SpriteAnimation spriteAnim = animList[animIndex];

            if (animIndex != iLastAnimation)
            {
                iLastAnimation = animIndex;
                bChangingFrame = false;
                iFrame = 0;
            }

            if (!bChangingFrame)
            {
                 bChangingFrame = true;
                bPlaying = true;
                if (iFrame >= spriteAnim.sprites.Length) iFrame = 0;
                mainSprite.sprite = spriteAnim.sprites[iFrame];
                bChangingFrame = false;
                iFrame++;
            }
        }

        protected IEnumerator ChangeSprite(int animIndex)
        {
            SpriteAnimation spriteAnim = animList[animIndex];
            WrapMode wrap = spriteAnim.wrapMode == WrapMode.Default ? defaultWrapMode : spriteAnim.wrapMode;
            bool playNext = true;

            if (animIndex != iLastAnimation)
            {
                iLastAnimation = animIndex;
                bChangingFrame = false;
                iFrame = 0;
            }

            if (!bChangingFrame)
            {
                 bChangingFrame = true;
                bPlaying = true;
                if (!pong ? iFrame >= spriteAnim.sprites.Length : iFrame <= -1)
                {
                    switch (wrap)
                    {
                        case WrapMode.Clamp:
                        case WrapMode.ClampForever:
                        //case WrapMode.Once:
                        case WrapMode.Default:
                            iFrame = spriteAnim.sprites.Length - 1;
                            playNext = false;
                            break;
                        case WrapMode.Loop:
                            iFrame = 0;
                            break;
                        case WrapMode.PingPong:
                            if (!pong)
                            {
                                iFrame = spriteAnim.sprites.Length > 1 ? spriteAnim.sprites.Length - 2 : spriteAnim.sprites.Length - 1;
                                pong = true;
                            }
                            else
                            {
                                iFrame = spriteAnim.sprites.Length > 1 ? 1 : 0;
                                pong = false;
                            }
                            break;
                    }
                }
                 mainSprite.sprite = spriteAnim.sprites[iFrame];

                yield return new WaitForSeconds((float)1.0f / spriteAnim.fps);
                bChangingFrame = false;
                iFrame += pong ? -1 : 1;
                if (playNext) StartCoroutine(ChangeSprite(animIndex));
            }
        }

        public void Stop()
        {
            StopAllCoroutines();
            iFrame = 0;
            bPlaying = false;
            bPaused = false;
            bChangingFrame = false;
            pong = false;
        }

        public void Pause()
        {
            StopAllCoroutines();
            bPlaying = false;
            bPaused = false;
            bChangingFrame = false;
            pong = false;
        }

  
        public string Playing()
        {
            return animList[iLastAnimation].animName;
        }


        public bool isPlaying
        {
            get
            {
                return bPlaying;
            }
        }

        public int GetAnimationIndex(string requestedAnimationName)
        {
            for (int i = 0; i < animList.Length; i++)
            {
                if (animList[i].animName == requestedAnimationName) return i;
            }
            Debug.LogError("No animation \"" + requestedAnimationName + "\" found.");
            return -1;
        }

        public string GetAnimationName(int requestedAnimationIndex)
        {
            if (requestedAnimationIndex < animList.Length - 1 && requestedAnimationIndex > 0) return animList[requestedAnimationIndex].animName;
            if (requestedAnimationIndex < 0 || requestedAnimationIndex > animList.Length - 1)
            {
                Debug.LogError("Requested index is out of range");
                return "";
            }
            return "";
        }

        public static void RemoveAt<type>(ref type[] arrayToChange, ref int index)
        {
            type[] tempArray = new type[arrayToChange.Length - 1];
            int j = 0;
            for (int i = 0; i < arrayToChange.Length; i++)
            {
                if (i != index)
                {
                    tempArray[j] = arrayToChange[i];
                    j++;
                }
            }
            if (index != 0 && index == arrayToChange.Length - 1) index -= 1;
            arrayToChange = tempArray;
        }


    }

}