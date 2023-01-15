using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MusicManager, controls background music playback
/// </summary>
public class MusicManager : MonoBehaviour
{
    /// <summary>
    /// Typical priority which should be used for playing background music
    /// </summary>
    public const int priorityBackground = 0;
    
    /// <summary>
    /// Typical priority which should be used for music related to bonuses
    /// </summary>
    public const int priorityBonus = 5;
    
    /// <summary>
    /// Typical priority which should be used for music that should always play, regardless other settings
    /// </summary>
    public const int priorityOverlayEverything = int.MaxValue;

    private MusicParams masterLayer = new MusicParams(1f, 1f);
    private List<MusicImpl> activePlayers = new List<MusicImpl>();
    private MusicImpl focusedPlayer;
    
    /// <summary>
    /// Calculate music transition values
    /// </summary>
    /// <param name="from">Previous music parameters</param>
    /// <param name="to">Next music parameters</param>
    /// <param name="into">Parameters which transition should modify</param>
    /// <param name="time">Time of transition</param>
    /// <returns>false if transition is not yet completed, true otherwise</returns>
    public delegate bool MusicTransition(MusicParams from, MusicParams to, MusicParams into, float time);

    /// <summary>
    /// Parameters of single music controller layer
    /// </summary>
    public class MusicParams
    {
        public static MusicParams normal = new MusicParams(1f, 1f);
        public static MusicParams mute = new MusicParams(0f, 1f);
        
        public float volume;
        public float pitch;

        public MusicParams() : this(1f, 1f) { }
        
        public MusicParams(MusicParams p) : this(p.volume, p.pitch) { }
        
        public MusicParams(float volume, float pitch)
        {
            this.volume = volume;
            this.pitch = pitch;
        }
    }

    public static MusicManager Instance
    {
        get { return FindObjectOfType<MusicManager>(); }
    }

    /// <summary>
    /// Master volume of whole audio playback system
    /// </summary>
    public float MasterVolume = 1f;

    /// <summary>
    /// Master pitch of whole audio playback system
    /// </summary>
    public float MasterPitch = 1f;

    /// <summary>
    /// Decrease master volume by some value
    /// </summary>
    public bool Mute;

    /// <summary>
    /// Initiates playback of new audio clip, returns music builder instance 
    /// which can be used to configure audio parameters
    /// </summary>
    public MusicBuilder PlayMusic(AudioClip clip)
    {
        return new MusicBuilderImpl(this, clip);
    }

    /// <summary>
    /// Gets list of currently active and playing music instances
    /// </summary>
    public Music[] ActiveMusic
    {
        get { return activePlayers.ToArray(); }
    }

    /// <summary>
    /// Completely stops all music playback from all players
    /// </summary>
    public void StopAllMusic()
    {
        StopAllMusic(focusedPlayer);
    }

    // - internal stuff ---------------------------

    private class MusicBuilderImpl : MusicBuilder
    {
        public MusicManager manager;
        public AudioClip clip;
        public int priority;
        public bool replaceEverything;
        public bool isLooping;
        public float startDelay;
        public MusicTransition outTransition;
        public MusicTransition inTransition;
        
        public MusicBuilderImpl(MusicManager manager, AudioClip clip)
        {
            this.manager = manager;
            this.clip = clip;

            TransitionCut();
            SetPriority(priorityBackground);
            SetLooping(false);
            SetStartDelay(0f);
        }
        
        public MusicBuilder SetPriority(int priority)
        {
            this.priority = priority;
            return this;
        }

        public MusicBuilder SetLooping(bool looping)
        {
            isLooping = looping;
            return this;
        }

        public MusicBuilder ReplaceEverything(bool replace)
        {
            replaceEverything = replace;
            return this;
        }

        public MusicBuilder SetStartDelay(float startDelay)
        {
            this.startDelay = startDelay;
            return this;
        }

        public MusicBuilder TransitionCut()
        {
            return TransitionCrossFade(0.0f);
        }

        public MusicBuilder TransitionCrossFade(float duration)
        {
            MusicTransition transition = (o, n, i, t) =>
            {
                var interp = duration == 0 ? 1 : Mathf.Clamp(Mathf.InverseLerp(0, duration, t), 0f, 1f);
                i.volume = Mathf.Lerp(o.volume, n.volume, interp);
                i.pitch = Mathf.Lerp(o.pitch, n.pitch, interp);
                return interp == 1f;
            };
            
            CustomInTransition(transition);
            CustomOutTransition(transition);
            return this;
        }

        public MusicBuilder TransitionFadeOutFadeIn(float fadeOutDuration, float pause, float fadeInDuration)
        {
            var fadeOutEnd = fadeOutDuration;
            var fadeInStart = fadeOutDuration + pause;
            var fadeInEnd = fadeInStart + fadeInDuration;

            CustomOutTransition((o, n, i, t) =>
            {
                var interp = fadeOutEnd == 0 ? 1 : Mathf.Clamp(Mathf.InverseLerp(0, fadeOutEnd, t), 0, 1);
                i.volume = Mathf.Lerp(o.volume, n.volume, interp);
                i.pitch = Mathf.Lerp(o.pitch, n.pitch, interp);
                return interp == 1;
            });
            
            CustomInTransition((o, n, i, t) =>
            {
                
                var interp = fadeInEnd == 0 ? 1 : Mathf.Clamp(Mathf.InverseLerp(fadeInStart, fadeInEnd, t), 0, 1);
                i.volume = Mathf.Lerp(o.volume, n.volume, interp);
                i.pitch = Mathf.Lerp(o.pitch, n.pitch, interp);
                return interp == 1;
            });

            SetStartDelay(fadeInStart);
            
            return this;
        }

        public MusicBuilder CustomInTransition(MusicTransition inTransition)
        {
            this.inTransition = inTransition;
            return this;
        }

        public MusicBuilder CustomOutTransition(MusicTransition outTransition)
        {
            this.outTransition = outTransition;
            return this;
        }

        public Music Start()
        {
            return manager.CreateAndStartMusic(this);
        }
    }

    private class MusicImpl : MonoBehaviour, Music
    {
        public int priority;
        public MusicManager manager;
        public List<MusicParams> playbackLayers;
        public MusicParams ownLayer;
        public MusicParams transitionLayer;
        public AudioSource source;
        public MusicTransition fadeInTransition;
        public MusicTransition fadeOutTransition;

        private MusicParams transitionStartParams;
        private MusicParams transitionEndParams;
        private MusicTransition activeTransition;
        private float transitionTime;
        private bool destroyAfterTransition;

        public void Init(MusicBuilderImpl settings, MusicManager manager)
        {
            this.manager = manager;
            
            ownLayer = new MusicParams(1f, 1f);
            transitionLayer = new MusicParams(0f, 1f);
            priority = settings.priority;
            fadeInTransition = settings.inTransition;
            fadeOutTransition = settings.outTransition;

            source = gameObject.AddComponent<AudioSource>();
            source.clip = settings.clip;
            source.volume = 0.0f;
            source.pitch = 1.0f;
            source.spatialBlend = 0.0f;
            source.bypassEffects = true;
            source.bypassListenerEffects = true;
            source.ignoreListenerPause = true;
            source.loop = settings.isLooping;
            
            playbackLayers = new List<MusicParams>
            {
                ownLayer,
                transitionLayer,
                manager.masterLayer
            };
        }

        private void Update()
        {
            var resultVolume = 1f;
            var resultPitch = 1f;
            
            foreach (var layer in playbackLayers)
            {
                resultVolume *= layer.volume;
                resultPitch *= layer.pitch;
            }

            source.volume = resultVolume;
            source.pitch = resultPitch;

            if (!source.isPlaying)
            {
                Stop();
                Destroy(gameObject);
            }
            else if (activeTransition != null)
            {
                var isFinished = activeTransition(transitionStartParams, transitionEndParams, 
                    transitionLayer, transitionTime);

                transitionTime += Time.unscaledDeltaTime;
            
                if (isFinished)
                {
                    if (destroyAfterTransition)
                    {
                        Destroy(gameObject);
                    }
                
                    activeTransition = null;
                }
            }
        }

        public int Priority
        {
            get { return priority; }
        }

        public float Volume
        {
            get { return ownLayer.volume; }
            set { ownLayer.volume = Volume; }
        }

        public float Pitch
        {
            get { return ownLayer.pitch; }
            set { ownLayer.pitch = value; }
        }

        public bool Loop
        {
            get { return source.loop; }
            set { source.loop = value; }
        }
        
        public float LoopPosition { get; set; }
        
        public void RunTransition(MusicTransition transition, MusicParams targetParams, bool destroy = false)
        {
            transitionStartParams = new MusicParams(transitionLayer);
            transitionEndParams = new MusicParams(targetParams);
            activeTransition = transition;
            transitionTime = 0f;
            destroyAfterTransition = destroy;
        }
        
        public void Stop()
        {
            manager.RemoveMusic(this);
        }
    }

    private void Update()
    {
        masterLayer.volume = Mute ? MasterVolume * 0.25f : MasterVolume;
        masterLayer.pitch = MasterPitch;
    }

    private void StopAllMusic(MusicImpl fadeOutParamsPlayer)
    {
        foreach (var player in activePlayers)
        {
            player.RunTransition(fadeOutParamsPlayer != null 
                ? fadeOutParamsPlayer.fadeOutTransition 
                : player.fadeOutTransition, MusicParams.mute, true);
        }
        
        activePlayers.Clear();
    }
    
    private void RemoveMusic(MusicImpl music)
    {
        activePlayers.Remove(music);
        music.RunTransition(focusedPlayer != null 
            ? focusedPlayer.fadeOutTransition 
            : music.fadeOutTransition, MusicParams.mute, true);
        
        UpdatePlaybackParameters();
    }
    
    private Music CreateAndStartMusic(MusicBuilderImpl builder)
    {
        var o = new GameObject(builder.clip.name);
        o.transform.parent = transform;
        
        var music = o.AddComponent<MusicImpl>();
        music.Init(builder, this);

        if (builder.replaceEverything)
        {
            StopAllMusic(music);
        }
        
        activePlayers.Add(music);
        UpdatePlaybackParameters();

        if (focusedPlayer == music)
        {
            music.source.PlayDelayed(builder.startDelay);
        }
        else
        {
            music.source.Play();
        }
        
        return music;
    }

    private void UpdatePlaybackParameters()
    {
        if (activePlayers.Count != 0)
        {
            var newPlayer = GetHighestPriorityMusic();

            if (newPlayer != focusedPlayer)
            {
                var transitionPlayer = focusedPlayer != null
                    ? newPlayer.priority > focusedPlayer.priority
                        ? newPlayer
                        : focusedPlayer
                    : newPlayer;

                foreach (var player in activePlayers)
                {
                    if (player == newPlayer)
                    {
                        Debug.Log("Transition in: " + player.gameObject.name);
                        player.RunTransition(transitionPlayer.fadeInTransition, MusicParams.normal);
                    }
                    else
                    {
                        Debug.Log("Transition out: " + player.gameObject.name);
                        player.RunTransition(transitionPlayer.fadeOutTransition, MusicParams.mute);
                    }
                }

                focusedPlayer = newPlayer;
            }
        }
        else
        {
            focusedPlayer = null;
        }
    }
    
    private MusicImpl GetHighestPriorityMusic()
    {
        MusicImpl music = null;
        
        foreach (var i in activePlayers)
        {
            if (music == null || i.priority >= music.priority)
            {
                music = i;
            }
        }

        return music;
    }
}