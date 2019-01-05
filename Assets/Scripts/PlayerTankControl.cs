using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player tank control interface
/// </summary>
public class PlayerTankControl : MonoBehaviour
{
    private TankControl tankControl;

	void Start ()
    {
        tankControl = GetComponent<TankControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 relativeMousePosition = mousePosition - (Vector2)transform.position;
        float newOrientation = tankControl.GetOrientation(ref relativeMousePosition);

        tankControl.TurnCannonTo(ref newOrientation);

        if (Input.GetMouseButtonDown(0))
        {
            if (tankControl.TryFire())
                Assets.Scripts.Stats.getInstance().ShotFired();
            tankControl.TryMoveAndTurnTo(ref mousePosition);
        }

        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftControl))
            tankControl.BlowUp();
    }
}
