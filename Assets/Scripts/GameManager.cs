using UnityEngine;

/// <summary>
/// Game manager responsible to load/checkpoint player, screen, messages
/// </summary>
public class GameManager : MonoBehaviour
{

    [SerializeField] private Checkpoint _checkpoint;
    [SerializeField] private CheckpointMark _initialCheckPoint;
    
    
    #region BuiltinMethods

    private void OnEnable()
    {
        _checkpoint.SetCheckpointPosition(_initialCheckPoint.Position);
        CheckpointMark.OnCheckpointEnter += UpdateCheckpointEvent;
        Player.OnDeath += OnPlayerDeathEvent;
    }

    private void OnDisable()
    {
        CheckpointMark.OnCheckpointEnter -= UpdateCheckpointEvent;
        Player.OnDeath -= OnPlayerDeathEvent;
    }

    #endregion
    

    private void OnPlayerDeathEvent(Player player)
    {
        player.RespawnAt(_checkpoint.Position);
    }

    private void UpdateCheckpointEvent(Vector2 checkpoint)
    {
        _checkpoint.SetCheckpointPosition(checkpoint);
    }
    

}
