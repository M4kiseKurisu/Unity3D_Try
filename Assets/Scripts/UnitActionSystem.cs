using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;
    
    private bool isBusy;
    private BaseAction selectedAction;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        SetSelectionUnit(selectedUnit);
    }
    
    private void Update() {
        if (isBusy) return;

        if (TryHandleUnitSelection()) return;
        HandleSelectedAction();
    }

    private void HandleSelectedAction() {
        if (Input.GetMouseButtonDown(0)) {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            
            switch (selectedAction) {
                case MoveAction moveAction:
                    if (moveAction.IsValidActionGridPosition(mouseGridPosition)) {
                        SetBusy();
                        moveAction.Move(mouseGridPosition, ClearBusy);
                    }
                    break;
                case SpinAction spinAction:
                    SetBusy();
                    spinAction.Spin(ClearBusy);
                    break;
            }
        }
    }

    private void SetBusy() {
        isBusy = true;
    }

    private void ClearBusy() {
        isBusy = false;
    }

    private bool TryHandleUnitSelection() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) {
                    SetSelectionUnit(unit);
                    return true;
                }
        }

        return false;
    }
    
    private void SetSelectionUnit(Unit unit) {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction action) {
        selectedAction = action;
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }
}
