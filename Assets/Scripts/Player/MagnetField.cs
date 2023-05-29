using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class MagnetField : MonoBehaviour
{
    [SerializeField] private CircleCollider2D magnetTrigger;
    [Header("Debugging")]
    [SerializeField] private float upgradeTimeLeft;
    [SerializeField] private List<GameObject> magnetizedObjects = new();
    [SerializeField] private List<Vector2> magnetizedObjInitialPosition = new();
    [SerializeField] private List<float> magnetizedObjAnimationTimers = new();

    public bool MagnetCoroutineRunning { get; private set; } = false;

    private void Awake()
    {
        Actions.OnPickableObjectPickedUp += RemovePickedObjFromMagnetizedObjects;
    }

    private void OnDestroy()
    {
        Actions.OnPickableObjectPickedUp -= RemovePickedObjFromMagnetizedObjects;
    }

    public void ActivateMagnetFor(float seconds)
    {
        gameObject.SetActive(true);
        magnetTrigger.enabled = true;
        MagnetCoroutineRunning = true;

        upgradeTimeLeft = seconds;
        magnetizedObjects.Clear();
        magnetizedObjInitialPosition.Clear();
        magnetizedObjAnimationTimers.Clear();

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
                int objIndex = magnetizedObjects.IndexOf(gameObj);
                float interpValue = magnetizedObjAnimationTimers[objIndex];

                //Interpolate each object towards Player using appropriate timer
                objTransform.position = new Vector2
                (
                    Mathf.Lerp(magnetizedObjInitialPosition[objIndex].x, this.transform.position.x, interpValue),
                    Mathf.Lerp(magnetizedObjInitialPosition[objIndex].y, this.transform.position.y, interpValue)
                );
            }

            //Increase each Coin's timer on how long it's been magnetized
            for (int i = 0; i < magnetizedObjAnimationTimers.Count; i++)
                magnetizedObjAnimationTimers[i] += Time.deltaTime;

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
        if (!magnetTrigger.enabled) return;
        PickableObject objectScript = collision.GetComponent<PickableObject>();
        if (objectScript != null && objectScript.Type == PickableObjectType.Coin)
        {
            if (objectScript.IsAttractedByMagnet)
            {
                return;
            }
            
            objectScript.IsAttractedByMagnet = true;
            magnetizedObjects.Add(collision.gameObject);
            magnetizedObjAnimationTimers.Add(0.0f);
            magnetizedObjInitialPosition.Add(collision.transform.position);
        }
    }

    private void RemovePickedObjFromMagnetizedObjects(PickableObject pickableObjectScript, GameObject pickableObject)
    {
        if (magnetizedObjects.Contains(pickableObject))
        {
            pickableObjectScript.IsAttractedByMagnet = false;
            magnetizedObjInitialPosition.RemoveAt(magnetizedObjects.IndexOf(pickableObject));
            magnetizedObjAnimationTimers.RemoveAt(magnetizedObjects.IndexOf(pickableObject));
            magnetizedObjects.Remove(pickableObject);
        }
    }
}
