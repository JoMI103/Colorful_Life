using UnityEngine;

namespace old
{
    public abstract class HandsBaseState
    {
        protected HandsStateMachine _ctx;
        protected HandsStateFactory _factory;

        public HandsBaseState(HandsStateMachine currentctx, HandsStateFactory factory)
        {
            _ctx = currentctx;
            _factory = factory;
        }

        public abstract void EnterState();

        public abstract void UpdateState();

        public abstract void ExitState();

        public abstract bool CheckSwitchStates();



        protected void SwitchState(HandsBaseState newState)
        {
            //current State Exits
            ExitState();

            newState.EnterState();

            _ctx.CurrentsState = newState;
        }

        public abstract void GrabAction();
        public abstract void AttackAction(Transform TargetAttack);


        protected void UpdateHandsTarget((Transform, Transform) ts)
        {
            _ctx.LeftRightHands.Item1.updateTargetTransform(ts.Item1);
            _ctx.LeftRightHands.Item2.updateTargetTransform(ts.Item2);
        }

        protected void UpdateHandsPosRot()
        {
            _ctx.LeftRightHands.Item1.updateHand();
            _ctx.LeftRightHands.Item2.updateHand();
        }
    }
}