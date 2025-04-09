using System;
using System.Collections.Generic;
using _Game.Core.Events;
using _Game.Interfaces;
using _Game.Systems.BetSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Systems.ChipSystem
{
    public class ChipDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private LayerMask tableLayer;
        private Camera _mainCam;
        private IEventBus _eventBus;
        private ChipStack _draggedStack;
        private BetArea _hoveredArea;
        private List<ChipStack> _placedStacks = new List<ChipStack>();
        private ChipManager _chipManager;
        
        public void Construct(IEventBus eventBus, ChipManager chipManager)
        {
            _eventBus = eventBus;
            _mainCam = Camera.main;
            _chipManager = chipManager;
            _eventBus.Subscribe<ToRouletteEvent>(e => ResetPlacedStacks());
            Debug.Log("ChipDragger constructed");
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _eventBus.Fire(new SpinWheelEvent());
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    if (hit.collider.TryGetComponent<ChipStack>(out _draggedStack))
                    {
                        // TODO: implement successful dragging logic 
                        //Debug.Log(_draggedStack.TotalValue);
                    }
                    if(!_draggedStack) return;
                    _eventBus.Fire(new ChipDragStartedEvent(_draggedStack));
                    _eventBus.Fire(new ToTableEvent());
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (!_draggedStack) return;

                var ray = _mainCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 100, tableLayer))
                {
                    _draggedStack.transform.position = hit.point + Vector3.up * 0.1f;

                    var area = BetAreaFinder.FindClosest(hit.point);
                    if (_hoveredArea != area)
                    {
                        _hoveredArea = area;
                        _eventBus.Fire(new ChipDragEvent(area, _draggedStack));
                    }
                    
                    
                }
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (_draggedStack && _hoveredArea != null)
                {
                    _eventBus.Fire(new ChipDragEndedEvent(
                        _hoveredArea, 
                        _draggedStack.GetTotalValue()
                    ));
                    Bet placedBet = new Bet(
                        type: _hoveredArea.Data.Type,
                        amount: _draggedStack.GetTotalValue(),
                        gridPos: Vector2Int.zero, // Replace with actual grid position if needed.
                        numbers: _hoveredArea.Data.Numbers
                    );
                    _eventBus.Fire(new BetPlacedEvent(placedBet, _draggedStack.transform.position, _draggedStack.gameObject));
                
                    PlaceChipStack(_hoveredArea);
                    _draggedStack = null;
                }
            }
                
        }

        private void ResetPlacedStacks()
        {
            foreach (var stack in _placedStacks)
            {
                _chipManager.ResetStack(stack);
            }
            _placedStacks.Clear();
        }

        private void PlaceChipStack(BetArea betArea)
        {
            if (!_draggedStack) return;
            _draggedStack.transform.position = betArea.Data.Positions[0] + Vector3.up * 0.1f;
            _placedStacks.Add(_draggedStack);
        }
        
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
            if (Physics.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.TryGetComponent<ChipStack>(out _draggedStack))
                {
                    Debug.Log("Dragging stack: " + _draggedStack.name);
                    _eventBus.Fire(new ChipDragStartedEvent(_draggedStack));
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_draggedStack == null) return;

            var ray = _mainCam.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out var hit, 100, tableLayer))
            {
                _draggedStack.transform.position = hit.point + Vector3.up * 0.1f;

                // Detect hovered bet area
                var area = BetAreaFinder.FindClosest(hit.point);
                if (_hoveredArea != area)
                {
                    _hoveredArea = area;
                    _eventBus.Fire(new ChipDragEvent(area,_draggedStack));
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_draggedStack != null && _hoveredArea != null)
            {
                _eventBus.Fire(new ChipDragEndedEvent(
                    _hoveredArea, 
                    _draggedStack.GetTotalValue()
                ));
            }
            _draggedStack = null;
        }
    }
}