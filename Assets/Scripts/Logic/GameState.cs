using System;
using System.Runtime.InteropServices;
using UnityEngine;

public enum State
{
    InMenu,
    IntroCutscene,
    InGame,
    FightingBoss,
    BossBeaten,
    EndCutscene,
    InitializingGame
}

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
public struct RawGameState
{
    private const string SignatureString = "Sonic-Suggests_GameState_v1.2";
    private const string EndSignatureString = "Sonic-Suggests_GameState_END";
    
    private const int SizeOfSignature = 30;
    private const int SizeOfEndSignature = 29;
    
    private const int SizeOfInt = 4;
    private const int SizeOfBoolPadded = SizeOfInt;
    private const int SizeOfSignaturePadded = 32;

    private const int SignatureOffset = 0;
    
    private const int IsLoadingOffset = SignatureOffset + SizeOfSignaturePadded;
    private const int CurrentStateOffset = IsLoadingOffset + SizeOfBoolPadded;
    private const int LivesOffset = CurrentStateOffset + SizeOfInt;
    private const int RingsOffset = LivesOffset + SizeOfInt;
    private const int CheckpointOffset = RingsOffset + SizeOfInt;
    private const int BossHealthOffset = CheckpointOffset + SizeOfInt;
    
    private const int EndSignatureOffset = BossHealthOffset + SizeOfInt;

    [FieldOffset(SignatureOffset)]
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeOfSignature)]
    public string Signature;
    
    [FieldOffset(IsLoadingOffset)]
    public bool IsLoading;
    
    [FieldOffset(CurrentStateOffset)]
    [MarshalAs(UnmanagedType.I4)]
    public State CurrentState;
    
    [FieldOffset(LivesOffset)]
    public int Lives;

    [FieldOffset(RingsOffset)]
    public int Rings;
    
    [FieldOffset(CheckpointOffset)]
    public int Checkpoint;
    
    [FieldOffset(BossHealthOffset)]
    public int BossHealth;
    
    [FieldOffset(EndSignatureOffset)]
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SizeOfEndSignature)]
    public string EndSignature;

    public RawGameState(bool isLoading, State currentState, int lives, int rings, int checkpoint, int bossHealth) : this()
    {
        Signature = SignatureString;
        IsLoading = isLoading;
        CurrentState = currentState;
        Lives = lives;
        Rings = rings;
        Checkpoint = checkpoint;
        BossHealth = bossHealth;
        EndSignature = EndSignatureString;
    }
}

public class GameState
{
    public static GameState Instance = new GameState();
    private static RawGameState _rawGameState = new RawGameState(
        default(bool),
        default(State),
        default(int),
        default(int),
        default(int),
        default(int));

    public bool IsLoading
    {
        get
        {
            return _rawGameState.IsLoading;
        }
        set
        {
            _rawGameState.IsLoading = value;
            UpdateRawGameStateInMemory();
        }
    }

    public State CurrentState
    {
        get
        {
            return _rawGameState.CurrentState;
        }
        set
        {
            _rawGameState.CurrentState = value;
            UpdateRawGameStateInMemory();
        }
    }

    public int Lives
    {
        get
        {
            return _rawGameState.Lives;
        }
        set
        {
            _rawGameState.Lives = value;
            UpdateRawGameStateInMemory();
        }
    }

    public int Rings
    {
        get
        {
            return _rawGameState.Rings;
        }
        set
        {
            _rawGameState.Rings = value;
            UpdateRawGameStateInMemory();
        }
    }

    public int Checkpoint
    {
        get
        {
            return _rawGameState.Checkpoint;
        }
        set
        {
            _rawGameState.Checkpoint = value;
            UpdateRawGameStateInMemory();
        }
    }

    public int BossHealth
    {
        get
        {
            return _rawGameState.BossHealth;
        }
        set
        {
            _rawGameState.BossHealth = value;
            UpdateRawGameStateInMemory();
        }
    }

    private void UpdateRawGameStateInMemory()
    {
        if(_unsupportedPlatform)
            return;
        
        Marshal.StructureToPtr(_rawGameState, _rawIntPtr, false);
    }

    private IntPtr _rawIntPtr;
    
    private bool _unsupportedPlatform = false;

    private GameState()
    {
        _rawIntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(_rawGameState));

        if (_rawIntPtr == IntPtr.Zero)
        {
            _unsupportedPlatform = true;
            Debug.Log("GameState: Failed to allocate the GameState in memory. This could be because your platform is not supported. The public GameState will not be updated");
            return;
        }

        CurrentState = State.InitializingGame;
    }

    ~GameState()
    {
        if(_rawIntPtr != IntPtr.Zero)
            Marshal.FreeHGlobal(_rawIntPtr);
    }
}