using System;
using System.Linq;
using UnityEngine;


    public class CursorHandler: MonoBehaviour
    {
        
        [SerializeField] private LayerMask _targetableMask;
        
        [Header("Mouse Textures")]
        [SerializeField] private Sprite _cursorTexture;
        [SerializeField] private Sprite _cursorTextureOver;
        [SerializeField] private Sprite _cursorTextureInRange;
        [SerializeField] private bool _disableCursorTextures;
        [SerializeField] private float _range = 2f;
        
        [SerializeField] private bool _enableGizmos;
        [SerializeField] private Color _gizmosColor;
        
        private SpriteRenderer _spriteRenderer;
        private CursorState _currentCursorState;
        private bool _cursorOver;
        

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            //TargetableObject.OnMouseOverTargetable += OnMouseOverTargetableEvent;
            //TargetableObject.OnMouseExitTargetable += OnMouseExitTargetableEvent;
            TargetableObject.OnPlayerCanTarget += OnPlayerHasTargets;
            TargetableObject.OnPlayerCannotTarget += OnPlayerLosesTarget;
        }

        private void OnDisable()
        {
            //TargetableObject.OnMouseOverTargetable -= OnMouseOverTargetableEvent;
            //TargetableObject.OnMouseExitTargetable -= OnMouseExitTargetableEvent;
            TargetableObject.OnPlayerCanTarget -= OnPlayerHasTargets;
            TargetableObject.OnPlayerCannotTarget -= OnPlayerLosesTarget;
        }
        
        
        private void OnDrawGizmos()
        {
            if (!_enableGizmos) return;
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireSphere(transform.position, _range);  
        }
        
        private void Update()
        {
            /*Vector2 newPosition= Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            transform.position = newPosition;*/
            
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//Define the ray pointed by the mouse in the game window
            RaycastHit hitInfo; //Information of ray collision
            if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.gameObject.CompareTag("Targetable")) //Determine whether to hit the object
            {
                if (_disableCursorTextures) return;
                Debug.Log("ENTER ...");
                OnNewCursorState(CursorState.Over);
                _cursorOver = true;
            }
            else
            {
                _currentCursorState = CursorState.Normal;
                OnEndCursorState(CursorState.Over);
                _cursorOver = false;
            }

            /*Collider2D hit = Physics2D.OverlapCircle(transform.position, _range, _targetableMask);
            if (hit!= null && hit.gameObject.TryGetComponent<TargetableObject>(out var targetableObject) && targetableObject.IsValidTarget())
            {
                if (_disableCursorTextures) return;
                Debug.Log("ENTER ...");
                OnNewCursorState(CursorState.Over);
                _cursorOver = true;
            }
            else
            {
                if (_cursorOver)
                {
                    Debug.Log("EXITING ...");
                    _currentCursorState = CursorState.Normal;
                    OnEndCursorState(CursorState.Over);
                    _cursorOver = false;
                }
                
            }*/
            
        }
        
        //whenever a mouse is over a targetable object
        /*private void OnMouseOverTargetableEvent(TargetableObject targetable)
        {
            if (_disableCursorTextures) return;
            OnNewCursorState(CursorState.Over);
        }*/
        
        //Whenever a player has valid targets
        private void OnPlayerHasTargets(TargetableObject targetableObject)
        {
            if (_disableCursorTextures) return;
            OnNewCursorState(CursorState.InRange);
        }
        
        //when player exists from range of a targetable object
        private void OnPlayerLosesTarget(TargetableObject targetableObject)
        {
            if (_disableCursorTextures) return;
            OnEndCursorState(CursorState.InRange);
        }
        
        //when mouse exists a targetable object
        /*
        private void OnMouseExitTargetableEvent(TargetableObject targetable)
        {
            if (_disableCursorTextures) return;
            OnEndCursorState(CursorState.Over);
        }
        */
        
        //Start a new cursor state
        private void OnNewCursorState(CursorState newState)
        {
            if (CursorState.Over.Equals(_currentCursorState)) return;
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
        }
        
        //End a cursor state
        private void OnEndCursorState(CursorState exitState)
        {
            if (CursorState.Over.Equals(_currentCursorState)) return;
            
            //Debug.Log($" EXIT {exitState}");
            //Only update the cursor if it is not OVER a target or exiting from OVER
            /*if (CursorState.Over.Equals(exitState))
            {
                _currentCursorState = CursorState.Normal;
            }*/
            
            
            
            //check if there is still other valid target
            using var enumerator = FindObjectsOfType<TargetableObject>().Where( to => to.gameObject.GetComponent<SpriteRenderer>().isVisible).GetEnumerator();

            var stillHasTarget = false;
            while (enumerator.MoveNext())
            {
                var targetableObject = enumerator.Current;
                if (targetableObject == null || !targetableObject.IsValidTarget()) continue;
                stillHasTarget = true;
                break;
            }
            //update the cursor
            SetCursorState(stillHasTarget ? CursorState.InRange : CursorState.Normal);
        }

        //Set the cursor texture and updates the state
        private void SetCursorState(CursorState state)
        {
            //Debug.Log($" SET {state}");
            switch (state)
            {
                case CursorState.Over:
                    //Cursor.SetCursor(_cursorTextureOver, _hotSpot, CursorMode);
                    //_cursor = _cursorTexture;
                    _spriteRenderer.sprite = _cursorTextureOver;
                    _currentCursorState = state;
                    break;
                case CursorState.InRange:
                    //Cursor.SetCursor(_cursorTextureInRange, _hotSpot, CursorMode);
                   // _spriteRenderer.sprite = _cursorTextureInRange;
                    _currentCursorState = state;
                    break;
                case CursorState.Normal:
                default:
                    //Cursor.SetCursor(_cursorTexture, _hotSpot, CursorMode);
                   // _spriteRenderer.sprite = _cursorTexture;
                    _currentCursorState = CursorState.Normal;
                    break;
            }
        }
        
    }
