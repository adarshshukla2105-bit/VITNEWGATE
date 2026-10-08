using UnityEngine;

/// <summary>
/// Controls the opening and closing of the two gate wings.
/// Can be operated by pressing Spacebar or key 'E', or triggered via external scripts.
/// </summary>
public class GateController : MonoBehaviour
{
    [Header("Gate Wings (Transforms to rotate)")]
    public Transform leftWing;
    public Transform rightWing;

    [Header("Rotation Angles")]
    [Tooltip("Default closed rotation angles (Euler)")]
    public Vector3 leftClosedRotation = new Vector3(0, 0, 0);
    public Vector3 rightClosedRotation = new Vector3(0, 0, 0);

    [Tooltip("Target open rotation angles (Euler)")]
    public Vector3 leftOpenRotation = new Vector3(0, -90f, 0);
    public Vector3 rightOpenRotation = new Vector3(0, 90f, 0);

    [Header("Settings")]
    public float transitionSpeed = 2.5f;
    public KeyCode toggleKey = KeyCode.Space;
    public KeyCode alternateKey = KeyCode.E;
    public bool isOpen = false;

    [Header("UI Overlay")]
    public bool showInstructions = true;

    private void Update()
    {
        // Toggle gate on key press
        if (Input.GetKeyDown(toggleKey) || Input.GetKeyDown(alternateKey))
        {
            isOpen = !isOpen;
        }

        // Smoothly interpolate gate wings toward target rotations
        if (leftWing != null)
        {
            Quaternion targetLeft = Quaternion.Euler(isOpen ? leftOpenRotation : leftClosedRotation);
            leftWing.localRotation = Quaternion.Slerp(leftWing.localRotation, targetLeft, Time.deltaTime * transitionSpeed);
        }

        if (rightWing != null)
        {
            Quaternion targetRight = Quaternion.Euler(isOpen ? rightOpenRotation : rightClosedRotation);
            rightWing.localRotation = Quaternion.Slerp(rightWing.localRotation, targetRight, Time.deltaTime * transitionSpeed);
        }
    }

    public void ToggleGate()
    {
        isOpen = !isOpen;
    }

    public void OpenGate()
    {
        isOpen = true;
    }

    public void CloseGate()
    {
        isOpen = false;
    }

    private void OnGUI()
    {
        if (!showInstructions) return;

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter
        };

        string statusText = isOpen ? "<color=#00FF66>OPEN</color>" : "<color=#FF5555>CLOSED</color>";
        string prompt = $"<b>[ VIT ENTRANCE GATE ]</b>\nStatus: {statusText}\nPress <b>[SPACE]</b> or <b>[E]</b> to Open/Close";

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 15,
            richText = true,
            alignment = TextAnchor.MiddleCenter
        };
        labelStyle.normal.textColor = Color.white;

        float width = 360f;
        float height = 75f;
        float x = (Screen.width - width) / 2f;
        float y = Screen.height - height - 30f;

        GUI.Box(new Rect(x, y, width, height), GUIContent.none);
        GUI.Label(new Rect(x, y, width, height), prompt, labelStyle);
    }
}
