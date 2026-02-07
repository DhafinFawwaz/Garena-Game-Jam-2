using System.Collections.Generic;
using UnityEngine;

public class EntityDetector : MonoBehaviour
{
    [SerializeField] List<SheepCore> _sheepCores;
    public List<SheepCore> SheepCores => _sheepCores;
    void OnTriggerEnter2D(Collider2D col) {
        if(!col.attachedRigidbody) return;
        if(!col.attachedRigidbody.TryGetComponent<SheepCore>(out var sheepCore)) return;
        _sheepCores.Add(sheepCore);
    }
    void OnTriggerExit2D(Collider2D col) {
        if(!col.attachedRigidbody) return;
        if(!col.attachedRigidbody.TryGetComponent<SheepCore>(out var sheepCore)) return;
        _sheepCores.Remove(sheepCore);
    }

    public SheepCore GetClosestSheepCoreWithDifferentTeamID(int teamID) {
        SheepCore closestSheepCore = null;
        float closestDistance = float.MaxValue;
        foreach(var sheepCore in _sheepCores) {
            if(sheepCore.Stats.TeamID == teamID) continue;
            float distance = Vector2.Distance(transform.position, sheepCore.transform.position);
            if(distance < closestDistance) {
                closestDistance = distance;
                closestSheepCore = sheepCore;
            }
        }
        return closestSheepCore;
    }
    public SheepCore GetClosestSheepCoreWithSameTeamID(int teamID) {
        SheepCore closestSheepCore = null;
        float closestDistance = float.MaxValue;
        foreach(var sheepCore in _sheepCores) {
            if(sheepCore.Stats.TeamID != teamID) continue;
            float distance = Vector2.Distance(transform.position, sheepCore.transform.position);
            if(distance < closestDistance) {
                closestDistance = distance;
                closestSheepCore = sheepCore;
            }
        }
        return closestSheepCore;
    }

    [SerializeField] float _attackRange = 1.5f;
    public bool IsCloseEnoughToAttack(SheepCore target) {
        float distance = Vector2.Distance(transform.position, target.transform.position);
        return distance <= _attackRange;
    }
}
