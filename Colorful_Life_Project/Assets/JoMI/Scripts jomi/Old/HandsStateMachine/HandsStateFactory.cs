
namespace old
{
    public class HandsStateFactory
    {
        HandsStateMachine _context;

        public HandsStateFactory(HandsStateMachine currentContext)
        {
            _context = currentContext;
        }

        public HandsBaseState Body()
        {
            return new HandsBodyState(_context, this);
        }
        public HandsBaseState Grabbing()
        {
            return new HandsGrabbingState(_context, this);
        }
        public HandsBaseState Attacking()
        {
            return new HandsAttackingState(_context, this);
        }
    }
}