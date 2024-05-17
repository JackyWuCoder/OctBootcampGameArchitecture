using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorIndicator : MonoBehaviour
{
    public Color lockedColor = Color.red;
    public Color unlockedColor = Color.green;

    public UnityEvent onLocked;
    public UnityEvent onUnlocked;

    private bool isLocked = true;

    void Start()
    {
        UpdateIndicator();
    }

    public void Lock()
    {
        isLocked = true;
        UpdateIndicator();
    }

    public void Unlock()
    {
        isLocked = false;
        UpdateIndicator();
    }

    void UpdateIndicator()
    {
        if (isLocked)
        {
            GetComponent<Renderer>().material.color = lockedColor;
            onLocked.Invoke();
        }
        else
        {
            GetComponent<Renderer>().material.color = unlockedColor;
            onUnlocked.Invoke();
        }
    }
}
