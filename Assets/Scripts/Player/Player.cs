using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private HeightSimulator heightSimulator;
    [SerializeField] private Rigidbody2D ownRigidbody;
    [SerializeField] private Collider2D ownCollider;
    [SerializeField] private SpriteRenderer ownSpriteRenderer;
    [SerializeField] private AudioManager audioManager;

    [Header("Skins")]
    [SerializeField] private Sprite defaultSkin;
    [SerializeField] private Sprite pirateSkin;
    [SerializeField] private Sprite diverSkin;

    [Header("Upgrade References")]
    [SerializeField] private MagnetField magnetField;
    [SerializeField] private ScoreMultiplier scoreMultiplierScript;

    [Header("Player Movement")]
    [SerializeField] private float accelerationRate = 0.4f;
    [SerializeField] private float decelerationRate = 0.4f;
    private const float minHorizontalVelocity = 0.35f;
    private const float maxHorizontalVelocity = 10f;
    private const float jumpForce = 10f;
    private float maxVerticalVelocity = jumpForce;
    private Vector2 resetPosition;
    private GameData gameData;

    public bool IsFrozenOnX { private set; get; }
    public bool IsFrozenOnY { private set; get; }

    private Vector3 currentTapPosition;

    void Awake() => resetPosition = transform.position;

    void OnEnable()
    {
        Actions.OnPickableObjectPickedUp += ApplyBoost;
        ResetToStartingPosition();
    }

    void OnDisable()
    {
        Actions.OnPickableObjectPickedUp -= ApplyBoost;
        ResetToStartingPosition();
    }

    void FixedUpdate()
    {
        if (transform.position.y < GlobalAttributes.DespawnBarrier && GameManager.CurrentGameState == GameState.Playing)
            Actions.OnGameLost?.Invoke();
        
        //Cancel all movement input when game isnt playing
        if (GameManager.CurrentGameState == GameState.Playing)
        {
            // X Movement System
            //Teleport player to the other side of screen when he falls out of the screen bounds
            if (transform.position.x < GlobalAttributes.LeftScreenEdge)
                transform.position = new Vector2(GlobalAttributes.RightScreenEdge, transform.position.y);
            if (transform.position.x > GlobalAttributes.RightScreenEdge)
                transform.position = new Vector2(GlobalAttributes.LeftScreenEdge, transform.position.y);

            if (Input.GetMouseButton(0))
            {
                //Flip sprite only when moving
                if (ownRigidbody.velocity.x > 0)
                    ownSpriteRenderer.flipX = true;
                else if (ownRigidbody.velocity.x < 0)
                    ownSpriteRenderer.flipX = false;

                //Update currentTapPosition and accelerate if user is holding finger
                currentTapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentTapPosition.z = 0;

                if (Mathf.Abs(ownRigidbody.velocity.x) < maxHorizontalVelocity)
                {
                    ownRigidbody.velocity = new Vector2(
                    currentTapPosition.x < GlobalAttributes.MiddleOfScreen.x ?
                        ownRigidbody.velocity.x - accelerationRate :
                        ownRigidbody.velocity.x + accelerationRate,
                    ownRigidbody.velocity.y);
                }
            }
            else
            {
                //Else, decelerate - round up velocity lower than minHorizontalVelocity to 0
                ownRigidbody.velocity =
                    Mathf.Abs(ownRigidbody.velocity.x) < minHorizontalVelocity ?
                    new Vector2(0, ownRigidbody.velocity.y) :
                    new Vector2(ownRigidbody.velocity.x - (Mathf.Sign(ownRigidbody.velocity.x) * decelerationRate), ownRigidbody.velocity.y);
            }

            // Y Movement system
            //Speed limit
            ownRigidbody.velocity = new Vector2(
                ownRigidbody.velocity.x,
                Mathf.Clamp(ownRigidbody.velocity.y, -10, maxVerticalVelocity)
                );
        }

        if (IsFrozenOnY) return;
        if (ownRigidbody.position.y > GlobalAttributes.HeightBarrier && ownRigidbody.velocity.y > 0)
        {
            heightSimulator.ResetPosition();
            heightSimulator.Unfreeze();
            heightSimulator.SetVerticalVelocity(ownRigidbody.velocity.y);
            FreezeOnlyOnY();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*        
         * When a Rigidbody2D falls into a Collider with high enough velocity, it sometimes goes slightly inside the Collider.
         * Pinpoint perfect calculations of Center + -Extent don't work in this situation, because on the frame when Player's Rigidbody
         * enters a Platform's Collider, its lower edge goes slightly inside, causing the comparisson to rightfully return false. Keeping only the center
         * allows for the audio to sometimes play twice, but otherwise the Jumping sometimes just doesn't work, which is arguably worse.
         *
         * An acceptable compromise is this comparrison + keeping the playerCollider thin enough.It might need to be enlargened if player is ever
         * allowed to reach greater velocities.
         */
        Bounds colliderBounds = collision.collider.bounds;
        Bounds ownBounds = ownCollider.bounds;

        bool aboveAndFalling = ownRigidbody.velocity.y <= 0 && ownBounds.center.y - ownBounds.extents.y >= colliderBounds.center.y;
        /*Debug.Log($"Player entered collision. ownVelocity: {ownRigidbody.velocity.y} <= 0 " +
                $"&& ownBoundCenter: {ownBounds.center.y} >= colliderBoundCenter: {colliderBounds.center.y}, " +
                $"Result: {aboveAndFalling}");*/
        if (aboveAndFalling)
        {
            audioManager.Play("pogostick");
            Jump();
        }
    }

     /* Movement related methods    */

    public void Jump() => ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, jumpForce);

    public void Freeze()
    {
        IsFrozenOnX = IsFrozenOnY = true;
        ownRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void FreezeOnlyOnY()
    {
        IsFrozenOnY = true;
        ownRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    public void Unfreeze()
    {
        IsFrozenOnX = IsFrozenOnY = false;
        ownRigidbody.WakeUp();
        ownRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void ResetToStartingPosition() => ownRigidbody.position = resetPosition;

    public void SetVelocity(Vector2 velocity) => ownRigidbody.velocity = velocity;

    public Vector2 GetVelocity() { return ownRigidbody.velocity; }

    public float GetLowerBoundCoordinate() => ownCollider.bounds.center.y - ownCollider.bounds.extents.y;

        /*  Upgrades Logic   */

    private void ApplyBoost(PickableObject pickableObjScript, GameObject pickableObjRef)
    {
        switch (pickableObjScript.Type)
        {
            case PickableObjectType.Coin:
                break;
            case PickableObjectType.SpringBoost:
                //Lvl 0. - 1.7, Lvl 1. - 1.8, Lvl 2. - 1.9, Lvl 3. - 2.0, Lvl 4. - 2.1, Lvl 5. - 2.2
                float jumpBoost = 1.7f + (0.1f * Mathf.Clamp(gameData.upgradeLvl_springBoost, 0, 5));
                //Debug.Log($"SpringBoost picked up. Calculated jump boost: {jumpBoost}");
                StartCoroutine(SpringBoostCoroutine(jumpBoost));
                break;
            case PickableObjectType.Magnet:
                int duration = 5 + Mathf.Clamp(gameData.upgradeLvl_magnet, 0, 5);
                //Debug.Log($"Magnet picked up. Calculated duration: {duration}");
                if (magnetField.CoroutineRunning)
                    magnetField.SetDurationTo(duration);
                else
                    magnetField.ActivateFor(duration);
                break;
            case PickableObjectType.ScoreMultiplier:
                duration = 5 + Mathf.Clamp(gameData.upgradeLvl_magnet, 0, 5);
                //Debug.Log($"ScoreMultiplier picked up. Calculated duration: {duration}");
                if (scoreMultiplierScript.CoroutineRunning)
                    scoreMultiplierScript.SetDurationTo(duration);
                else
                    scoreMultiplierScript.ActivateFor(duration);
                break;
            default:
                Debug.LogWarning($"Player acquired an unimplemented boost/upgrade: {pickableObjScript.Type}");
                break;
        }
    }

    private IEnumerator SpringBoostCoroutine(float jumpBoost)
    {
        maxVerticalVelocity = jumpForce * jumpBoost;
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, jumpForce * jumpBoost);
        if (IsFrozenOnY) heightSimulator.SetVerticalVelocity(jumpForce * jumpBoost);
        yield return new WaitForSeconds(2.0f);
        maxVerticalVelocity = jumpForce;
    }

    public void LoadData(GameData data)
    {
        this.gameData = data;
        //Switch to equipped Skin
        //Check how many had Equipped status, if there's somehow more than 1, equip the first one
        Dictionary<string, SkinStatus> loadedSkinStatuses = new()
        {
            { "skin_default", gameData.skin_default },
            { "skin_pirate", gameData.skin_pirate },
            { "skin_diver", gameData.skin_diver }
        };
        string skinToEquip = "";
        foreach (KeyValuePair<string, SkinStatus> kvp in loadedSkinStatuses)
        {
            if (kvp.Value == SkinStatus.Equipped)
            {
                skinToEquip = kvp.Key;
                break;
            }
        }
        switch (skinToEquip)
        {
            case "skin_default":
                ownSpriteRenderer.sprite = defaultSkin;
                //ownSpriteRenderer.color = Color.red;
                break;
            case "skin_pirate":
                ownSpriteRenderer.sprite = pirateSkin;
                //ownSpriteRenderer.color = Color.blue;
                break;
            case "skin_diver":
                ownSpriteRenderer.sprite = diverSkin;
                //ownSpriteRenderer.color = Color.green;
                break;
            default:
                //Equip fallback skin here
                ownSpriteRenderer.color = Color.red;
                Debug.LogError($"Player was asked to change their SpriteRenderer's sprite to unknown sprite!: {skinToEquip}");
                break;
        }
    }

    public void SaveData(ref GameData data){ /* Player doesn't save any data. It only loads.*/ }
}
