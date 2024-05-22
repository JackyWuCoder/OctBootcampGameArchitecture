using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    [SerializeField] protected PlayerInput playerInput;

    // Update is called once per frame
    void Update()
    {
        Interact();
    }

    public abstract void Interact();
}
