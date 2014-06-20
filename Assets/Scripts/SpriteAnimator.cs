/*
* Copyright (c) 2014 Luis Santos AKA DJOKER
 * 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour {
	
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
	private SpriteRenderer mainsprite;
	bool pong;
	
	void Start()
	{
        mainsprite = (SpriteRenderer)GetComponentInChildren<SpriteRenderer>();
		if (bPlayOnStart) Play(iPlayOnStartIndex);	
	}
	
	

	

	public void Play()
	{
		PerformPlay(0);
	}
	public void Play(string anim)
	{
		for(int i = 0; i < animList.Length; i++) 
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
	
	public void PlayByName(string anim)
	{
		for(int i = 0; i < animList.Length; i++) 
		if (animList[i].animName == anim)
		{
			PerformPlay(i);
			break;
		}
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
            mainsprite.sprite=spriteAnim.sprites[iFrame];
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
				switch(wrap)
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
		     mainsprite.sprite = spriteAnim.sprites[iFrame];

			yield return new WaitForSeconds((float)1.0f/spriteAnim.fps);
			bChangingFrame = false;
			iFrame += pong ? -1 : 1;
			if (playNext)StartCoroutine(ChangeSprite(animIndex));
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
		for(int i = 0; i < animList.Length; i++)
		{
			if (animList[i].animName == requestedAnimationName) return i;	
		}
		Debug.LogError("No animation \"" + requestedAnimationName + "\" found.");
		return -1;
	}

	public string GetAnimationName(int requestedAnimationIndex)
	{
		if (requestedAnimationIndex < animList.Length-1 && requestedAnimationIndex > 0) return animList[requestedAnimationIndex].animName;	
		if (requestedAnimationIndex < 0 || requestedAnimationIndex > animList.Length-1) 
		{
			Debug.LogError("Requested index is out of range");
			return "";
		}
		return "";
	}
	
	
	
}
