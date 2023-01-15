using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Music
{
	/// <summary>
	/// Read-only priority of this music instance
	/// </summary>
	int Priority { get; }
	
	/// <summary>
	/// Volume of background music
	/// </summary>
	float Volume { get; set; }
        
	/// <summary>
	/// Pitch of background music
	/// </summary>
	float Pitch { get; set; }
        
	/// <summary>
	/// Is background music looping
	/// </summary>
	bool Loop { get; set; }
        
	/// <summary>
	/// Loop point, where audio should start after playback finish
	/// </summary>
	/// TODO: Unsupported at the moment
	float LoopPosition { get; set; }

	/// <summary>
	/// Stops this music instance, and makes reverse transition to previous music, if there is any
	/// </summary>
	void Stop();
}