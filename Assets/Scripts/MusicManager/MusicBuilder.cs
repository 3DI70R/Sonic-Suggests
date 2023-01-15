using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MusicBuilder
{
	/// <summary>
	/// Sets priority for this background music, music with higher priority will replace music with lower priority
	/// </summary>
	MusicBuilder SetPriority(int priority);

	/// <summary>
	/// If set to true, music will loop indefinetly, untill Stop() is called
	/// </summary>
	MusicBuilder SetLooping(bool looping);

	/// <summary>
	/// If set to true, this music stop every currently playing music
	/// </summary>
	MusicBuilder ReplaceEverything(bool replace);

	/// <summary>
	/// Set wait time, before this audio should start playing
	/// It will be active only if audio immediately gains focus
	/// </summary>
	MusicBuilder SetStartDelay(float startDelay);

	/// <summary>
	/// Sets "cut" transition, previous music stops playing, and new one immediatelly starts playing
	/// Overrides start delay to 0
	/// </summary>
	MusicBuilder TransitionCut();

	/// <summary>
	/// Sets "CrossFade" transition, previous music starts fading, and new one slowly appears
	/// Overrides start delay to 0
	/// </summary>
	MusicBuilder TransitionCrossFade(float duration);

	/// <summary>
	/// Sets "CrossFade" transition, previous music starts fading, and new one slowly appears
	/// Overrides start delay to fadeOutDuration + pause
	/// </summary>
	MusicBuilder TransitionFadeOutFadeIn(float fadeOutDuration, float pause, float fadeInDuration);

	/// <summary>
	/// Sets custom in transition
	/// This transition will run on this music, when it gains focus
	/// </summary>
	MusicBuilder CustomInTransition(MusicManager.MusicTransition inTransition);
        
	/// <summary>
	/// Sets custom out transition
	/// This transition will run on other music, when this music gains focus
	/// </summary>
	MusicBuilder CustomOutTransition(MusicManager.MusicTransition outTransition);

	/// <summary>
	/// Builds and starts this music instance
	/// </summary>
	Music Start();
}
