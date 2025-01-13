using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagDoll : MonoBehaviour
{
    [SerializeField] private Transform ragDollRootBone;
    public void SetUp(Transform orginalRootBone)
    {
        MatchAllChildTransform(orginalRootBone, ragDollRootBone);
        ApplyExlosionRagdoll(ragDollRootBone, 300f, transform.position, 10f);
    }
    private void MatchAllChildTransform(Transform root,Transform clone)
    {
        foreach(Transform child in root)
        {
            Transform cloneChild=clone.Find(child.name);
            if(cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MatchAllChildTransform (child, cloneChild);
            }
        }
    }
    private void ApplyExlosionRagdoll(Transform root,float exploisionForce,Vector3 explosionPosition,float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidiBody))
            {
                childRigidiBody.AddExplosionForce(exploisionForce, explosionPosition, explosionRange); 
            }
            ApplyExlosionRagdoll(child,exploisionForce, explosionPosition,explosionRange);
        }
    }
}
