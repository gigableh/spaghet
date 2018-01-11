using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public GameScript gameScript;

    void Update()
    {
        if (!gameScript.isGameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var pos = Input.mousePosition;
                pos.z = Camera.main.nearClipPlane;
                Ray r = Camera.main.ScreenPointToRay(pos);
                var hit = Physics2D.Raycast(r.origin, r.direction);
                if (hit != null && hit.transform != null)
                {
                    var hc = hit.transform.GetComponentInParent<HandController>();
                    if (hc != null)
                    {
                        hc.SlapHand();
                    }
                }
            }
        }
    }
}
