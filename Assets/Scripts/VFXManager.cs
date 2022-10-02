
    using System;
    using System.Linq;
    using UnityEngine;
    

    public class VFXManager: MonoBehaviour
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

        [SerializeField] private AudioSource _displaceSound;
        
        #region BuiltinMethodsdw

        private void OnEnable()
        {
            Player.OnDisplace += OnPlayerDisplaceEvent;
            Player.OnDeath += OnPlayerDeathEvent;
            Projectile.OnProjectileHit += OnProjectileHitEvent;
            CheckpointMark.OnCheckpointEnter += OnCheckpointEnterEvent;
            TargetableObject.OnMouseOverTargetable += OnMouseOverTargetableEvent;
            TargetableObject.OnMouseExitTargetable += OnMouseExitTargetableEvent;
            Enemy.OnDeath += OnEnemyDeathEvent;
            Enemy.OnCastSpell += OnEnemyCastSpellEvent;
            Props.OnDestroy += OnPropsDestroyEvent;
            AggroBox.OnAggroRange += OnAggroRangeEvent;
            TargetableObject.OnPlayerCanTarget += OnPlayerHasTargets;
            TargetableObject.OnPlayerCannotTarget += OnPlayerLosesTarget;
            Spike.OnEnemyHitSpike += OnEnemyHitSpikeEvent;
            Spike.OnPlayerHitSpike += OnPlayerHitSpikeEvent;
            //OnNewCursorState(CursorState.Normal);
            SetCursorState(CursorState.Normal);
        }

        private void OnDisable()
        {
            Player.OnDisplace -= OnPlayerDisplaceEvent;
            Player.OnDeath -= OnPlayerDeathEvent;
            Projectile.OnProjectileHit -= OnProjectileHitEvent;
            CheckpointMark.OnCheckpointEnter -= OnCheckpointEnterEvent;
            TargetableObject.OnMouseOverTargetable -= OnMouseOverTargetableEvent;
            TargetableObject.OnMouseExitTargetable -= OnMouseExitTargetableEvent;
            Enemy.OnDeath -= OnEnemyDeathEvent;
            Enemy.OnCastSpell -= OnEnemyCastSpellEvent;
            Props.OnDestroy -= OnPropsDestroyEvent;
            AggroBox.OnAggroRange -= OnAggroRangeEvent;
            TargetableObject.OnPlayerCanTarget -= OnPlayerHasTargets;
            TargetableObject.OnPlayerCannotTarget -= OnPlayerLosesTarget;
            Spike.OnEnemyHitSpike -= OnEnemyHitSpikeEvent;
            Spike.OnPlayerHitSpike -= OnPlayerHitSpikeEvent;
        }

        #endregion


        #region CursorManipulation


        //whenever a mouse is over a targetable object
        private void OnMouseOverTargetableEvent(TargetableObject targetable)
        {
            if (_disableCursorTextures) return;
            //OnNewCursorState(CursorState.Over);
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
        
        //when mouse exists a targetable object
        private void OnMouseExitTargetableEvent(TargetableObject targetable)
        {
            if (_disableCursorTextures) return;
            //OnEndCursorState(CursorState.Over);
        }
        
        //Start a new cursor state
        /*private void OnNewCursorState(CursorState newState)
        {
            switch (newState)
            {
                case CursorState.Over:
                    SetCursorState(CursorState.Over);
                    break;
                case CursorState.InRange when !CursorState.Over.Equals(_currentCursorState):
                    SetCursorState(CursorState.InRange);
                    break;
                case CursorState.Normal when !CursorState.Over.Equals(_currentCursorState):
                default:
                    SetCursorState(CursorState.Normal);
                    break;
            }
        }*/
        
        //End a cursor state
        private void UpdateCursorState(CursorState exitState)
        {
            
            //Only update the cursor if it is not OVER a target or exiting from OVER
            /*if (CursorState.Over.Equals(exitState))
            {
                _currentCursorState = CursorState.Normal;
            }
            
            if (CursorState.Over.Equals(_currentCursorState)) return;*/
            
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

        #endregion
        
        private void OnCheckpointEnterEvent(Vector2 checkpoint)
        {
           //TODO play sound
        }


        private void OnProjectileHitEvent(Projectile projectile, Player player)
        {
            //TODO shake camera
        }

        

        private void OnPlayerDeathEvent(Player player)
        {
            //TODO play sound
            //Show message
        }

        private void OnPlayerDisplaceEvent(Player player)
        {
            if(_displaceSound!=null) _displaceSound.Play();
        }
        
        
        private void OnEnemyDeathEvent(Enemy enemy)
        {
            //TODO play sound and particle
        }
        
        
        private void OnEnemyCastSpellEvent(Enemy enemy)
        {
            //TODO Play sound
        }

        
        private void OnPropsDestroyEvent(Props props)
        {
            //TODO play sound
        }

        
        private void OnAggroRangeEvent(Player player, Enemy enemy)
        {
            //TODO screen border red
        }
        
        
        private void OnPlayerHitSpikeEvent(Player player)
        {
            //TODO sfx
        }

        private void OnEnemyHitSpikeEvent(Enemy enemy)
        {
            //TODO sfx
        }
        
    }
