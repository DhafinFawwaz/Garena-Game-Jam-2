using System;
using UnityEngine;

[System.Serializable]
public class HitRequest: HitData
{
    public Vector3 Direction = Vector3.zero;
    public Vector3 HitPosition = Vector3.zero;

    public HitRequest(){}
    // Everything
    public HitRequest(float damage, float knockback, Vector3 direction, Vector3 hitPosition, Element element = Element.Normal, float stunDuration = 0.1f)
    {
        Damage = damage;
        Knockback = knockback;
        Direction = direction;
        HitPosition = hitPosition;
        Element = element;
        StunDuration = stunDuration;
    }

    public void SetFromHitData(HitData hitData) {
        Damage = hitData.Damage;
        Knockback = hitData.Knockback;
        Element = hitData.Element;
        StunDuration = hitData.StunDuration;
    }

    public HitRequest Clone() {
        return new HitRequest(Damage, Knockback, Direction, HitPosition, Element, StunDuration);
    }
}
