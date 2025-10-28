using UnityEngine;
using UnityEngine.Events;
using System; // Needed for Action

public class PlayerController : MonoBehaviour
{
    //Made with some help from Chat GPT

    [SerializeField] private GameObject fleshCubePrefab;

    public static event Action<int> OnHealthChanged; // Event that others can subscribe to

    [Header("Pickup Settings")]
    public string pickableTag = "Pickable";  // Tag of objects that can be picked up
    public float pickupRange = 3f;           // How far the player can pick up objects
    public Transform holdPoint;              // Empty GameObject child of the player
    public float throwForce = 10f;           // Force applied when throwing the object

    [Header("Held Object Event")]
    public UnityEvent onHeldObjectLeftClick; // Event triggered when left-clicking while holding
    private Camera playerCamera;
    private Rigidbody heldObject;

    [Header("Player Stats")]
    public int maxHealth = 100;
    public int currentHealth;

    private FleshCube fleshCubeScript;
    private float cooldownTimer = 0f;

    void Start()
    {
        playerCamera = Camera.main;
        if (holdPoint == null)
        {
            Debug.LogError("HoldPoint is not assigned! Please assign an empty child GameObject to the player.");
        }

        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth); // Notify listeners of initial health
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right Mouse Button
        {
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                ThrowObject();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject != null)
            {
                FleshCube gun = heldObject.GetComponent<FleshCube>();
                if (gun != null)
                {
                    gun.Shoot();
                }
                else
                {
                    Debug.Log("Held object is not a gun.");
                }
            }
            else
            {
                Debug.Log("Not holding anything to shoot.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnAndDespawn();
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // ?? New: Check for left mouse click while holding
        if (heldObject != null && Input.GetMouseButtonDown(0))
        {
            //TriggerHeldObjectEvent();
        }

        // Keep the held object in place
        if (heldObject != null)
        {
            heldObject.MovePosition(holdPoint.position);
            heldObject.MoveRotation(holdPoint.rotation);
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.collider.CompareTag(pickableTag))
            {
                Rigidbody objectRigidbody = hit.collider.GetComponent<Rigidbody>();

                if (objectRigidbody != null)
                {
                    heldObject = objectRigidbody;
                    heldObject.useGravity = false;
                    heldObject.linearDamping = 10f;
                    heldObject.transform.position = holdPoint.position;
                    heldObject.transform.rotation = holdPoint.rotation;
                }
            }
        }
    }

    void ThrowObject()
    {
        heldObject.useGravity = true;
        heldObject.linearDamping = 1f;
        heldObject.transform.parent = null;
        heldObject.linearVelocity = playerCamera.transform.forward * throwForce;
        heldObject = null;
    }

    void SpawnAndDespawn()
    {
        if (heldObject == null & cooldownTimer <= 0f)
        {
            // Spawn the prefab and store a reference to it
            Instantiate(fleshCubePrefab, holdPoint.transform.position, holdPoint.transform.localRotation);
            TakeDamage(10);
            cooldownTimer += 3f;
        }
        else
        {
            // Destroy the currently held object and clear the reference
            Destroy(heldObject.gameObject);
            GainHealth(10);
            heldObject = null;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Notify listeners that health changed
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void GainHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Notify listeners that health changed
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Handle death logic here
    }
}
