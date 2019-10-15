using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformeDesactivable : SpecialPlatform
{
    [SerializeField]
    Color normalColor;
    [SerializeField]
    Color disableColor;

    [SerializeField]
    float activeTime;
    [SerializeField]
    float resetTime;

    [SerializeField]
    LayerMask playerLayer;
    [SerializeField]
    BoxCollider2D boxCollider;

    bool isActivated = false;
    Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    public override void OnActivate()
    {
        if (!isActivated)
        {
            StartCoroutine(ActivationCoroutine());
        }
    }

    IEnumerator ActivationCoroutine()
    {
        isActivated = true;

        yield return new WaitForSeconds(activeTime);

        mat.color = disableColor;
        boxCollider.enabled = false;

        yield return new WaitForSeconds(resetTime);

        mat.color = normalColor;
        boxCollider.enabled = true;

        isActivated = false;
    }
}
