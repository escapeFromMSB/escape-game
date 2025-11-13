using UnityEngine;

/// Attach to any pickup object. PlayerInteractor handles LOS/range & collection.
[DisallowMultipleComponent]
public class InventoryPickup : MonoBehaviour
{
    [Header("Item")]
    public string itemId = "keycard_red";
    public string displayName = "Red Keycard";
    public int amount = 1;

    [Header("Pulse FX (only when eligible to pick up)")]
    public bool enablePulse = true;
    public float pulseScale = 1.06f;
    public float pulseSpeed = 5f;

    Vector3 baseScale;
    bool canPickUpNow;

    void Awake() => baseScale = transform.localScale;

    public void SetHoverEligible(bool eligible)
    {
        canPickUpNow = eligible;
        if (!eligible) transform.localScale = baseScale; // stop/pin scale
    }

    void Update()
    {
        if (!enablePulse || !canPickUpNow) return;

        float t = (Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f);
        transform.localScale = baseScale * Mathf.Lerp(1f, pulseScale, t);
    }

    public void OnCollected()
    {
        Destroy(gameObject);
    }
}