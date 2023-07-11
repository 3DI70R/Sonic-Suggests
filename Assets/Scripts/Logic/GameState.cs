using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

public class GameState
{
    public static GameState Instance = new GameState();

    public unsafe bool IsLoading
    {
        get
        {
            if (_unsupportedPlatform)
                return default(bool);
            return *_isLoadingPtr;
        }
        set
        {
            if (_unsupportedPlatform)
                return;
            *_isLoadingPtr = value;
        }
    }

    public unsafe State CurrentState
    {
        get
        {
            if (_unsupportedPlatform)
                return default(State);
            return (State)(*_currentStatePtr);
        }
        set
        {
            if (_unsupportedPlatform)
                return;
            *_currentStatePtr = (int)value;
        }
    }

    public unsafe int Lives
    {
        get
        {
            if (_unsupportedPlatform)
                return default(int);
            return *_livesPtr;
        }
        set
        {
            if (_unsupportedPlatform)
                return;
            *_livesPtr = value;
        }
    }

    public unsafe int Rings
    {
        get
        {
            if (_unsupportedPlatform)
                return default(int);
            return *_ringsPtr;
        }
        set
        {
            if (_unsupportedPlatform)
                return;
            *_ringsPtr = value;
        }
    }

    public unsafe int Checkpoint
    {
        get
        {
            if (_unsupportedPlatform)
                return default(int);
            return *_checkpointPtr;
        }
        set
        {
            if (_unsupportedPlatform)
                return;
            *_checkpointPtr = value;
        }
    }

    public unsafe int BossHealth
    {
        get
        {
            if (_unsupportedPlatform)
                return default(int);
            return *_bossHealthPtr;
        }
        set
        {
            if (_unsupportedPlatform)
                return;
            *_bossHealthPtr = value;
        }
    }

    private IntPtr _rawIntPtr;
    
    private unsafe void* _rawPtr;

    private const string GameStateSignature = "Sonic-Suggests_GameState_v1.2";
    private const string GameStateEndSignature = "Sonic-Suggests_GameState_END";

    private const int GameStateSignatureLength = 29;
    private const int GameStateEndSignatureLength = 28;

    private const int SizeOfInt = 4;
    private const int SizeOfBoolPadded = SizeOfInt;
    private const int SizeOfLong = 8;
    
    private const int GameStateSize = 88;

    private const int IsLoadingOffset = 32;
    private const int CurrentStateOffset = IsLoadingOffset + SizeOfBoolPadded;
    
    private const int LivesOffset = CurrentStateOffset + SizeOfInt;
    private const int RingsOffset = LivesOffset + SizeOfInt;
    private const int CheckpointOffset = RingsOffset + SizeOfInt;
    private const int BossHealthOffset = CheckpointOffset + SizeOfInt;
    
    private const int EndSignatureOffset = BossHealthOffset + SizeOfInt;

    private unsafe bool* _isLoadingPtr;
    private unsafe int* _currentStatePtr;
    
    private unsafe int* _livesPtr;
    private unsafe int* _ringsPtr;
    private unsafe int* _checkpointPtr;
    private unsafe int* _bossHealthPtr;

    private bool _unsupportedPlatform = false;

    private unsafe GameState()
    {
        byte[] signatureBytes = Encoding.ASCII.GetBytes(GameStateSignature);
        byte[] signatureEndBytes = Encoding.ASCII.GetBytes(GameStateEndSignature);

        _rawIntPtr = Marshal.AllocHGlobal(GameStateSize);
        _rawPtr = _rawIntPtr.ToPointer();

        IntPtr endSignaturePtr = IntPtr.Zero;

        if (IntPtr.Size == SizeOfInt)
        {
            endSignaturePtr = new IntPtr(_rawIntPtr.ToInt32() + EndSignatureOffset);

            _isLoadingPtr = (bool*)((int)_rawPtr + IsLoadingOffset);
            _currentStatePtr = (int*)((int)_rawPtr + CurrentStateOffset);

            _livesPtr = (int*)((int)_rawPtr + LivesOffset);
            _ringsPtr = (int*)((int)_rawPtr + RingsOffset);
            _checkpointPtr = (int*)((int)_rawPtr + CheckpointOffset);
            _bossHealthPtr = (int*)((int)_rawPtr + BossHealthOffset);
        }
        else if (IntPtr.Size == SizeOfLong)
        {
            endSignaturePtr = new IntPtr(_rawIntPtr.ToInt64() + EndSignatureOffset);

            _isLoadingPtr = (bool*)((long)_rawPtr + IsLoadingOffset);
            _currentStatePtr = (int*)((long)_rawPtr + CurrentStateOffset);

            _livesPtr = (int*)((long)_rawPtr + LivesOffset);
            _ringsPtr = (int*)((long)_rawPtr + RingsOffset);
            _checkpointPtr = (int*)((long)_rawPtr + CheckpointOffset);
            _bossHealthPtr = (int*)((long)_rawPtr + BossHealthOffset);
        }
        else
        {
            _unsupportedPlatform = true;
            Debug.Log("GameState: Platform not supported, the public GameState will not be updated");
        }

        CurrentState = State.InitializingGame;

        byte[] zeroBytes = new byte[GameStateSize].Select(x => (byte)0).ToArray();
        
        Marshal.Copy(zeroBytes, 0, _rawIntPtr, GameStateSize);
        
        Marshal.Copy(signatureBytes, 0, _rawIntPtr, signatureBytes.Length);
        Marshal.Copy(signatureEndBytes, 0, endSignaturePtr, signatureEndBytes.Length);
    }

    ~GameState()
    {
        Marshal.FreeHGlobal(_rawIntPtr);
    }
}