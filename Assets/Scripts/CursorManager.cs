using System.Linq;
using UnityEngine;

public class CursorManager: MonoBehaviour
{
    [Header("Mouse Textures")]
    [SerializeField] private Texture2D _cursorTexture;
    [SerializeField] private Texture2D _cursorTextureOver;
    [SerializeField] private Texture2D _cursorTextureInRange;
    [SerializeField] private bool _disableCursorTextures;
    private CursorState _currentCursorState;
    //Mouse configuration
    private const CursorMode CursorMode = UnityEngine.CursorMode.Auto;
    private readonly Vector2 _hotSpot = Vector2.zero;


    private void OnEnable()
    {
        TargetableObject.OnPlayerCanTarget += OnPlayerHasTargets;
        TargetableObject.OnPlayerCannotTarget += OnPlayerLosesTarget;
        SetCursorState(CursorState.Normal);
    }

    private void OnDisable()
    {
        TargetableObject.OnPlayerCanTarget -= OnPlayerHasTargets;
        TargetableObject.OnPlayerCannotTarget -= OnPlayerLosesTarget;
    }

    //Whenever a player has valid targets
        private void OnPlayerHasTargets(TargetableObject targetableObject)
        {
            if (_disableCursorTextures) return;
            UpdateCursorState(CursorState.InRange);
        }
        
        //when player exists from range of a targetable object
        private void OnPlayerLosesTarget(TargetableObject targetableObject)
        {
            if (_disableCursorTextures) return;
            UpdateCursorState(CursorState.InRange);
        }
        
   
        //End a cursor state
        private void UpdateCursorState(CursorState exitState)
        {   
            //check if there is still other valid target
            using var enumerator = FindObjectsOfType<TargetableObject>().Where( to => to.gameObject.GetComponent<SpriteRenderer>().isVisible).GetEnumerator();

            var stillHasTarget = false;
            var targetLocked = false;
            while (enumerator.MoveNext())
            {
                var targetableObject = enumerator.Current;
                if (targetableObject == null) continue;
                var collisionInfo = targetableObject.IsValidTargetInfo();
                if (!collisionInfo.InRange) continue;
                stillHasTarget = true;
                targetLocked = collisionInfo.IsTargetLocked;
                if (targetLocked) break;
            }
            //update the cursor
            if (targetLocked)
            {
                SetCursorState(CursorState.Over);
            }
            else
            {
                SetCursorState(stillHasTarget ? CursorState.InRange : CursorState.Normal);    
            }
        }

        //Set the cursor texture and updates the state
        private void SetCursorState(CursorState state)
        {
            switch (state)
            {
                case CursorState.Over:
                    Cursor.SetCursor(_cursorTextureOver, _hotSpot, CursorMode);
                    _currentCursorState = state;
                    break;
                case CursorState.InRange:
                    Cursor.SetCursor(_cursorTextureInRange, _hotSpot, CursorMode);
                    _currentCursorState = state;
                    break;
                case CursorState.Normal:
                default:
                    Cursor.SetCursor(_cursorTexture, _hotSpot, CursorMode);
                    _currentCursorState = CursorState.Normal;
                    break;
            }
        }
}
