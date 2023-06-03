using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class MagnetField : TimedUpgrade
{
    [SerializeField] private CircleCollider2D magnetTrigger;
    [SerializeField] private float initialSpeed;
    [SerializeField] private float speedMultiplierPerYield;
    [Header("Debugging")]
    [SerializeField] private List<GameObject> magnetizedObjects = new();
    [SerializeField] private List<float> magnetizedObjSpeeds = new();
    [SerializeField] private List<Vector2> magnetizedObjInitialPosition = new();

    protected override void OnEnable()
    {
        base.OnEnable();
        Actions.OnPickableObjectPickedUp += RemovePickedObjFromMagnetizedObjects;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Actions.OnPickableObjectPickedUp -= RemovePickedObjFromMagnetizedObjects;
    }

    protected override void EnableEffect()
    {
        magnetTrigger.enabled = true;
        magnetizedObjects.Clear();
        magnetizedObjInitialPosition.Clear();
    }
    protected override void DisableEffect() => magnetTrigger.enabled = false;

    protected override IEnumerator EffectCoroutine()
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
