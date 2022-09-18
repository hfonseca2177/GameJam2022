
    using System;
    using UnityEngine;
    

    public class VFXManager: MonoBehaviour
    {

        
        [Header("Mouse Textures")]
        [SerializeField] private Texture2D _cursorTexture;
        [SerializeField] private Texture2D _cursorTextureOver;
        [SerializeField] private Texture2D _cursorTextureInRange;
        [SerializeField] private bool _disableCursorTextures;
        //Mouse configuration
        private const CursorMode CursorMode = UnityEngine.CursorMode.Auto;
        private readonly Vector2 _hotSpot = Vector2.zero;
        
        #region BuiltinMethods

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

        
        private void OnMouseExitTargetableEvent(TargetableObject targetable)
        {
            if (_disableCursorTextures) return;
            Cursor.SetCursor(_cursorTexture, _hotSpot, CursorMode);
        }

        private void OnMouseOverTargetableEvent(TargetableObject targetable)
        {
            if (_disableCursorTextures) return;
            Cursor.SetCursor(_cursorTextureOver, _hotSpot, CursorMode);
        }
        
        private void OnPlayerHasTargets(TargetableObject targetableObject)
        {
            /*if (_disableCursorTextures) return;
            Cursor.SetCursor(_cursorTextureInRange, _hotSpot, CursorMode);*/
        }
        
        private void OnPlayerLosesTarget(TargetableObject targetableObject)
        {
            /*if (_disableCursorTextures) return;
            Cursor.SetCursor(_cursorTexture, _hotSpot, CursorMode); */
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
            //TODO play sound
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
