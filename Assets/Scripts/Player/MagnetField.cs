﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class MagnetField : MonoBehaviour
{
    [SerializeField] private CircleCollider2D magnetTrigger;
    [SerializeField] private float magnetizedObjectSpeed;
    [Header("Debugging")]
    [SerializeField] private float upgradeTimeLeft;
    [SerializeField] private List<GameObject> magnetizedObjects = new();
    [SerializeField] private List<Vector2> magnetizedObjInitialPosition = new();

    public bool MagnetCoroutineRunning { get; private set; } = false;

    private void Awake()
    {
        Actions.OnPickableObjectPickedUp += RemovePickedObjFromMagnetizedObjects;
        Actions.OnGameLost += StopMagnetCoroutinePrematurily;
    }

    private void OnDestroy()
    {
        Actions.OnPickableObjectPickedUp -= RemovePickedObjFromMagnetizedObjects;
        Actions.OnGameLost += StopMagnetCoroutinePrematurily;
    }

    public void ActivateMagnetFor(float seconds)
    {
        gameObject.SetActive(true);
        magnetTrigger.enabled = true;
        MagnetCoroutineRunning = true;

        upgradeTimeLeft = seconds;
        magnetizedObjects.Clear();
        magnetizedObjInitialPosition.Clear();
        StartCoroutine(MagnetCoroutine(seconds));
    }

    public void StopMagnetCoroutinePrematurily()
    {
        StopAllCoroutines();
        DecomissionMagnet();
    }

    private void DecomissionMagnet()
    {
        upgradeTimeLeft = 0f;

        MagnetCoroutineRunning = false;
        magnetTrigger.enabled = true;
        gameObject.SetActive(false);
    }

    private IEnumerator MagnetCoroutine(float seconds)
    {
        while (true)
        {
            //Interpolate each Coin "attracted by magnetic field" closer to Player
            foreach (GameObject gameObj in magnetizedObjects)
            {
                Transform objTransform = gameObj.transform;
                Vector3 direction = objTransform.position - this.transform.position;
                objTransform.position -= magnetizedObjectSpeed * Time.deltaTime * direction.normalized;
            }

            //Even if upgrade's time is up, keep going until all Coins caught up in magnet field get picked up by Player.
            if (upgradeTimeLeft <= 0f)
            {
                magnetTrigger.enabled = false;
            }
            if (upgradeTimeLeft <= 0f && magnetizedObjects.Count == 0)
            {
                DecomissionMagnet();
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
            upgradeTimeLeft -= Time.deltaTime;
        }
    }

    public void SetDurationTo(float seconds)
    {
        if (!MagnetCoroutineRunning) return;
        Debug.Log($"Magnet duration reset from {upgradeTimeLeft} to {seconds}");
        upgradeTimeLeft = seconds;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!magnetTrigger.enabled) 
            return;
        PickableObject objectScript = collision.GetComponent<PickableObject>();
        if (objectScript != null && objectScript.Type == PickableObjectType.Coin)
        {
            if (objectScript.IsAttractedByMagnet)
                return;
            objectScript.IsAttractedByMagnet = true;
            magnetizedObjects.Add(collision.gameObject);
            magnetizedObjInitialPosition.Add(collision.transform.position);
        }
    }

    private void RemovePickedObjFromMagnetizedObjects(PickableObject pickableObjectScript, GameObject pickableObject)
    {
        if (magnetizedObjects.Contains(pickableObject))
        {
            pickableObjectScript.IsAttractedByMagnet = false;
            magnetizedObjInitialPosition.RemoveAt(magnetizedObjects.IndexOf(pickableObject));
            magnetizedObjects.Remove(pickableObject);
        }
    }
}
