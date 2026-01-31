using UnityEngine;
using System;

public class SimpleStaticActionInvoker : MonoBehaviour
{
    public static Action OnTriggered;
    public void TriggerAction() {
        OnTriggered?.Invoke();
    }    
}