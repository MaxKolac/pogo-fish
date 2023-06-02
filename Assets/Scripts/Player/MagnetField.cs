using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class MagnetField : MonoBehaviour
{
    [SerializeField] private UpgradeBarsManager barsManager;
    [SerializeField] private CircleCollider2D magnetTrigger;
    [SerializeField] private float initialSpeed;
    [SerializeField] private float speedMultiplierPerYield;
    [Header("Debugging")]
    [SerializeField] private float upgradeTimeLeft;
    [SerializeField] private int reservedBarID;
    [SerializeField] private List<GameObject> magnetizedObjects = new();
    [SerializeField] private List<float> magnetizedObjSpeeds = new();
    [SerializeField] private List<Vector2> magnetizedObjInitialPosition = new();

    public bool CoroutineRunning { get; private set; } = false;
    public bool CoroutinePaused { get; private set; } = false;

    private void OnEnable()
    {
        Actions.OnPickableObjectPickedUp += RemovePickedObjFromMagnetizedObjects;
        Actions.OnGameLost += StopPrematurily;
    }

    private void OnDisable()
    {
        Actions.OnPickableObjectPickedUp -= RemovePickedObjFromMagnetizedObjects;
        Actions.OnGameLost -= StopPrematurily;
    }

    public void ActivateFor(float seconds)
    {
        gameObject.SetActive(true);
        magnetTrigger.enabled = true;
        CoroutineRunning = true;
        Actions.OnGamePaused += Pause;
        Actions.OnGameUnpaused += Unpause;

        upgradeTimeLeft = seconds;
        magnetizedObjects.Clear();
        magnetizedObjInitialPosition.Clear();
        StartCoroutine(MagnetCoroutine());
        reservedBarID = barsManager.ReserveBar();
        barsManager.GetBarScript(reservedBarID).ActivateBarFor(seconds);
    }

    public void StopPrematurily()
    {
        StopAllCoroutines();
        barsManager.GetBarScript(reservedBarID).StopBarPrematurily();
        Decomission();
    }

    public void Pause()
    {
        if (CoroutineRunning) 
            CoroutinePaused = true;
    }

    public void Unpause()
    {
        if (CoroutineRunning && CoroutinePaused)
            CoroutinePaused = false;
    }

    private void Decomission()
    {
        Actions.OnGamePaused -= Pause;
        Actions.OnGameUnpaused -= Unpause;
        upgradeTimeLeft = 0f;

        CoroutineRunning = false;
        magnetTrigger.enabled = true;
        gameObject.SetActive(false);
    }

    private IEnumerator MagnetCoroutine()
    {
        while (true)
        {
            //Interpolate each Coin "attracted by magnetic field" closer to Player
            foreach (GameObject gameObj in magnetizedObjects)
            {
                int index= magnetizedObjects.IndexOf(gameObj);
                Transform objTransform = gameObj.transform;
                Vector3 direction = objTransform.position - this.transform.position;
                objTransform.position -= magnetizedObjSpeeds[index] * Time.deltaTime * direction.normalized;
                magnetizedObjSpeeds[index] *= speedMultiplierPerYield;
            }

            //Even if upgrade's time is up, keep going until all Coins caught up in magnet field get picked up by Player.
            if (upgradeTimeLeft <= 0f)
            {
                magnetTrigger.enabled = false;
            }
            if (upgradeTimeLeft <= 0f && magnetizedObjects.Count == 0)
            {
                Decomission();
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
            if (!CoroutinePaused)
                upgradeTimeLeft -= Time.deltaTime;
        }
    }

    public void SetDurationTo(float seconds)
    {
        if (!CoroutineRunning) return;
        //Debug.Log($"Magnet duration reset from {upgradeTimeLeft} to {seconds}");
        barsManager.GetBarScript(reservedBarID).SetTimeLeft(seconds);
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
            magnetizedObjSpeeds.Add(initialSpeed);
            magnetizedObjInitialPosition.Add(collision.transform.position);
        }
    }

    private void RemovePickedObjFromMagnetizedObjects(PickableObject pickableObjectScript, GameObject pickableObject)
    {
        if (magnetizedObjects.Contains(pickableObject))
        {
            int index = magnetizedObjects.IndexOf(pickableObject);
            pickableObjectScript.IsAttractedByMagnet = false;
            magnetizedObjInitialPosition.RemoveAt(index);
            magnetizedObjSpeeds.RemoveAt(index);
            magnetizedObjects.Remove(pickableObject);
        }
    }
}
