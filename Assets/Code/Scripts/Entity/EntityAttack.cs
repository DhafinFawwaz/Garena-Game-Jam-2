using System.Collections.Generic;
using DhafinFawwaz.AnimationUI;
using UnityEngine;

public class EntityAttack : MonoBehaviour
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


    [SerializeField] AnimationUI _attackAUI;
    public void PlayAttackAnimation() {
        _attackAUI.Play();
    }

    [SerializeField] AnimationUI _hurtAUI;
    public void PlayHurtAnimation() {
        _hurtAUI.Play();
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

    
    public SheepCore HitClosestSheepCoreWithDifferent(int teamID, HitRequest hitRequest) {
        var closestSheepCore = GetClosestSheepCoreWithDifferentTeamID(teamID);
        if(closestSheepCore == null) return null;
        closestSheepCore.OnHurt(hitRequest);
        return closestSheepCore;
    }

    public SheepCore HitClosestSheepCoreWithSameTeamID(int teamID, HitRequest hitRequest) {
        var closestSheepCore = GetClosestSheepCoreWithSameTeamID(teamID);
        if(closestSheepCore == null) return null;
        closestSheepCore.OnHurt(hitRequest);
        return closestSheepCore;
    }
}
