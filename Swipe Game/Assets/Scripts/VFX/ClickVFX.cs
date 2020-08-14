using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickVFX : MonoBehaviour
{
    [SerializeField] private GameObject vfx;
    static private GameObject effect;
    static private bool vfxAct = false;

    private void Start()
    {
        effect = Instantiate(vfx);
        EndVFX();
    }

    private void Update()
    {
        if (vfxAct)
            if (Input.GetMouseButtonDown(0))
                StartVFX();
    }

    static public void SetVFXStatus(bool state)
    {
        vfxAct = state;
    }

    private void StartVFX()
    {
        effect.SetActive(true);
        Vector3 mPos = Input.mousePosition;
        Vector3 realPos = Camera.main.ScreenToWorldPoint(mPos);
        realPos.z = -9;
        effect.transform.position = realPos;
    }

    private void EndVFX()
    {
        effect.SetActive(false);
    }
}
