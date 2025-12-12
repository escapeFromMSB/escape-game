using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class InteractablePuzzle : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private string puzzleSceneName = "PuzzleScene"; 
    [SerializeField] private Transform visual;                       

    [Header("Trigger Settings")]
    [Tooltip("Leave empty to detect CharacterController automatically.")]
    [SerializeField] private string playerTag = "";                   
    [SerializeField] private Vector3 triggerSize = new Vector3(2.5f, 2.0f, 2.5f);
    [SerializeField] private Vector3 triggerCenter = Vector3.zero;

    [Header("Highlight")]
    [SerializeField] private Color highlightEmission = new Color(1f, 0.7f, 0.2f); 
    [SerializeField] private float emissionIntensity = 2.2f;

    [Header("UI Prompt (optional)")]
    [SerializeField] private bool showPrompt = true;
    [SerializeField] private string promptText = "Press E to open puzzle";

    
    private bool inRange = false;
    private float lastSeenAt = -999f;
    private const float grace = 0.15f; 
    private Renderer visRend;
    private Color baseColor;
    private bool hadEmissionKeyword;

    void Awake()
    {
        
        var bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;
        bc.center = triggerCenter;
        bc.size   = triggerSize;

        var rb = GetComponent<Rigidbody>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true; rb.useGravity = false;

        if (!visual) visual = transform; 
        visRend = visual.GetComponent<Renderer>();
        if (visRend)
        {
            
            if (visRend.sharedMaterial.HasProperty("_BaseColor"))
                baseColor = visRend.material.GetColor("_BaseColor");
            else if (visRend.sharedMaterial.HasProperty("_Color"))
                baseColor = visRend.material.GetColor("_Color");
            else
                baseColor = Color.gray;

            hadEmissionKeyword = visRend.sharedMaterial.IsKeywordEnabled("_EMISSION");
        }
    }
    
    public void Configure(string sceneName, Transform visualTransform)
    {
        puzzleSceneName = sceneName;
        visual = visualTransform;
    }

    void Update()
    {
        
        bool seenRecently = (Time.time - lastSeenAt) <= grace;
        bool wantActive = inRange || seenRecently;

        SetHighlight(wantActive);

        
        if (wantActive && Input.GetKeyDown(KeyCode.E))
        {
            if (!string.IsNullOrWhiteSpace(puzzleSceneName))
            {
                SceneManager.LoadScene(puzzleSceneName, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogError("[InteractablePuzzle] puzzleSceneName is empty. Set it in the Inspector.");
            }
        }
    }

    private void SetHighlight(bool on)
    {
        if (!visRend) return;

        var mat = visRend.material; 
        if (on)
        {
            
            if (!mat.IsKeywordEnabled("_EMISSION")) mat.EnableKeyword("_EMISSION");

            
            if (mat.HasProperty("_EmissionColor"))
                mat.SetColor("_EmissionColor", highlightEmission * emissionIntensity);

            if (mat.HasProperty("_BaseColor"))
                mat.SetColor("_BaseColor", Color.Lerp(baseColor, baseColor * 1.15f, 0.5f)); 
            else if (mat.HasProperty("_Color"))
                mat.SetColor("_Color", Color.Lerp(baseColor, baseColor * 1.15f, 0.5f));
        }
        else
        {
            
            if (mat.HasProperty("_EmissionColor"))
                mat.SetColor("_EmissionColor", Color.black);

            if (!hadEmissionKeyword && mat.IsKeywordEnabled("_EMISSION"))
                mat.DisableKeyword("_EMISSION");

            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", baseColor);
            else if (mat.HasProperty("_Color")) mat.SetColor("_Color", baseColor);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other)) { inRange = true; lastSeenAt = Time.time; }
    }

    void OnTriggerStay(Collider other)
    {
        if (IsPlayer(other)) { inRange = true; lastSeenAt = Time.time; }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other)) { inRange = false; }
    }

    private bool IsPlayer(Collider other)
    {
        if (!string.IsNullOrEmpty(playerTag))
            return other.CompareTag(playerTag);

        
        return other.GetComponent<CharacterController>() != null;
    }

    void OnGUI()
    {
        if (!showPrompt) return;
        if (!(inRange || (Time.time - lastSeenAt) <= grace)) return;

        const int w = 340, h = 28;
        var rect = new Rect(Screen.width / 2 - w / 2, Screen.height - 100, w, h);
        GUIStyle s = GUI.skin.label;
        s.alignment = TextAnchor.MiddleCenter;
        s.fontSize = 16;
        GUI.Label(rect, promptText, s);
    }
}
