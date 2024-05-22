using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootInteractor : Interactor
{
    [Header("Player Shoot")]
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private float shootForce;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private PlayerMovementBehaviour playerMovementBehaviour;

    private float finalShootVelocity;

    [SerializeField] private InputType inputType;

    public enum InputType
    { 
        Primary,
        Secondary
    }

    public override void Interact()
    {
        if (inputType == InputType.Primary && playerInput.primaryButtonPressed
            || inputType == InputType.Secondary && playerInput.secondaryButtonPressed)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        finalShootVelocity = playerMovementBehaviour.GetForwardSpeed() * shootForce;
        Rigidbody bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        // ForceMode.Impulse allows the bullet to travel a lot faster
        bullet.AddForce(shootPoint.forward * finalShootVelocity, ForceMode.Impulse);
        Destroy(bullet.gameObject, 5.0f);
    }
}
