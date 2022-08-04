using UnityEngine;

namespace Euphrates
{
    public class CashUnloader : StackActionTrigger
    {
        readonly Vector3 OFFSET = new Vector3(0f, 1f, 0f);
        [Header("Cash Attributes"), Space]
        [SerializeField] IntSO _cashAmount;
        [SerializeField] ItemSO _cashItem;

        [SerializeField, Space] FloatSO _cashFloat; 

        protected override void DoAction()
        {
            if (!_ownStack.CanRemoveItem(_cashItem))
                return;

            Item item = _ownStack.RemoveItem(_cashItem);
            _cashAmount.Value++;
            item.FloatToPos(_cashFloat, Vector3.zero + OFFSET, Quaternion.identity, 0f, () => SpawnManager.ReleaseItem(item), _stackOwner.transform);
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            base.OnDrawGizmos();
        }
    }
}
