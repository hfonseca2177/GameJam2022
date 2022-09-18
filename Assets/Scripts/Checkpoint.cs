using System;
using UnityEngine;

/// <summary>
/// Checkpoint last position data
/// </summary>
[CreateAssetMenu(fileName = "Checkpoint", menuName = "Game/Checkpoint", order = 0)]
[Serializable]
public class Checkpoint : ScriptableObject
{
    public Vector2 Position;

    public void SetCheckpointPosition(Vector2 checkpoint)
    {
        Position = checkpoint;
    }

}
