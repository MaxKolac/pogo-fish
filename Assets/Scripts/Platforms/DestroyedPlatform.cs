using UnityEngine;

public class DestroyedPlatform : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] partsRigidbodies = new Rigidbody2D[3];
    private bool isActivated = false;

    private void OnEnable()
    {
        Actions.OnPlatformDespawn += TeleportAndPerformEffect;
    }

    private void OnDisable() => Actions.OnPlatformDespawn -= TeleportAndPerformEffect;

    private void Activate()
    {
        if (isActivated)
        {
            Deactivate();
        }
        Actions.OnDeltaHeightChanged += ScrollPartsDown;
        isActivated = true;
        foreach (Rigidbody2D part in partsRigidbodies)
        {
            part.gameObject.SetActive(true);
        }
        partsRigidbodies[0].velocity = new Vector2(
            Random.Range(-1f, -0.25f),
            Random.Range(0.25f, 1f)
            );
        partsRigidbodies[1].velocity = new Vector2(
            Random.Range(-0.25f, 0.25f),
            Random.Range(0.25f, 1f)
            );
        partsRigidbodies[2].velocity = new Vector2(
            Random.Range(0.25f, 1f),
            Random.Range(0.25f, 1f)
            );
        foreach (Rigidbody2D part in partsRigidbodies)
            part.angularVelocity = Random.Range(15f, 45f);
    }

    private void Update()
    {
        if (partsRigidbodies[0].transform.position.y < GlobalAttributes.DespawnBarrier &&
            partsRigidbodies[1].transform.position.y < GlobalAttributes.DespawnBarrier &&
            partsRigidbodies[2].transform.position.y < GlobalAttributes.DespawnBarrier &&
            isActivated)
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        Actions.OnDeltaHeightChanged -= ScrollPartsDown;
        foreach (Rigidbody2D part in partsRigidbodies)
        {
            part.gameObject.SetActive(false);
            part.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            part.velocity = Vector3.zero;
        }
        isActivated = false;
    }

    private void TeleportAndPerformEffect(Platform platformScript, GameObject platformObject)
    {
        OneJumpPlatform oneJumpScript = platformScript as OneJumpPlatform;
        if (oneJumpScript != null && oneJumpScript.Type == PlatformType.OneJump && oneJumpScript.DespawnedByPlayer)
        {
            this.transform.position = oneJumpScript.DespawnPosition;
            Activate();
        }
    }

    private void ScrollPartsDown(float deltaHeight)
    {
        foreach (Rigidbody2D part in partsRigidbodies)
        {
            part.transform.position -= new Vector3(0f, deltaHeight, 0f);
        }
    }
}
