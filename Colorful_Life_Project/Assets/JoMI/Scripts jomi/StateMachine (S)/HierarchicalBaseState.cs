using System;

public abstract class HierarchicalBaseState<EState> where EState : Enum {

    protected bool _isRootState;
    protected HierarchicalBaseState<EState> _currentSuperState;
    protected HierarchicalBaseState<EState> _currentSubState;

    protected HierarchicalBaseState(EState key) {
        StateKey = key;
    }

    public EState StateKey { get; private set; }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract bool CheckSwitchStates();

    public void UpdateStates() { 
        UpdateState();
        _currentSubState?.UpdateStates();
    }

    protected bool SwitchState(HierarchicalBaseState<EState> newState, ref HierarchicalBaseState<EState> contextCurrentState) {
        ExitState(); //Exit current state (this)
        newState.EnterState(); //Enter new state (newState)

        if (_isRootState)  contextCurrentState = newState; else _currentSuperState.SetSubState(newState);
        if (_currentSubState != null) newState.SetSubState(_currentSubState);
        return true;
    }

    protected void SetSuperState(HierarchicalBaseState<EState> newSuperState) {
        _currentSuperState = newSuperState; 
    }

    protected void SetSubState(HierarchicalBaseState<EState> newSubState) { 
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
