using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera mainCam;

    private float shakeAmount = 0f;

    private void Awake()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }

    public void Shake(float amount, float length)
    {
        shakeAmount = amount;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    private void BeginShake()
    {
        if (shakeAmount > 0)
        {
            var shakeAmountX = GetRandomShakeAmount();
            var shakeAmountY = GetRandomShakeAmount();

            ShakeCamera(shakeAmountX, shakeAmountY);
        }
    }

    private void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = Vector3.zero;
    }

    private void ShakeCamera(float shakeAmountX, float shakeAmountY)
    {
        var camPosision = mainCam.transform.position;
        camPosision.x += shakeAmountX;
        camPosision.y += shakeAmountY;

        mainCam.transform.position = camPosision;
    }


    private float GetRandomShakeAmount()
    {
        return Random.value * shakeAmount * 2 - shakeAmount;
    }
}
