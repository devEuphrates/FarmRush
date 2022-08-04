using UnityEngine;

namespace Euphrates
{
    public class Garbage : StackActionTrigger
	{
        protected override void DoAction()
        {
            if (!_stack.CanRemoveItem()) 
                return;

            Item itm = _stack.RemoveItem();
            itm.FloatToPos(0.2f, Vector3.zero, Quaternion.identity, 0f, () => SpawnManager.ReleaseItem(itm), transform);

            base.DoAction();
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            base.OnDrawGizmos();
        }
    }
}
