using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Procedurally constructs a complete 3D VIT Entrance Gate scene:
/// - Road with asphalt surface, center dashed lines, shoulder lines, and sidewalks.
/// - Two majestic stone pillars (base, shaft, capital).
/// - An upper architectural surface/roof covering and connecting the pillars.
/// - A classic 2-wing wrought iron entrance gate with interactive open/close controller.
/// - Entrance signboard, security booth, lighting, and camera.
/// </summary>
[ExecuteAlways]
public class VITGateSceneBuilder : MonoBehaviour
{
    [Header("Generation Settings")]
    public bool buildOnStart = false;

    [Header("Dimensions")]
    public float roadWidth = 8.0f;
    public float roadLength = 60.0f;
    public float pillarDistance = 9.0f; // Distance between pillar centers
    public float pillarHeight = 6.0f;
    public float pillarWidth = 1.4f;
    public float upperSurfaceThickness = 0.8f;
    public float roofOverhang = 1.0f;

    [Header("Colors & Styling")]
    public Color roadColor = new Color(0.18f, 0.19f, 0.22f); // Dark asphalt
    public Color markingColor = new Color(0.95f, 0.85f, 0.20f); // Warm yellow
    public Color sidewalkColor = new Color(0.75f, 0.77f, 0.80f); // Light concrete
    public Color pillarColor = new Color(0.88f, 0.85f, 0.80f); // Sandstone / Ivory
    public Color upperSurfaceColor = new Color(0.78f, 0.32f, 0.20f); // Terracotta / Crimson architectural trim
    public Color gateIronColor = new Color(0.12f, 0.13f, 0.15f); // Black wrought iron
    public Color grassColor = new Color(0.28f, 0.58f, 0.25f); // Natural green grass
    public Color signboardColor = new Color(0.05f, 0.15f, 0.35f); // VIT Navy Blue

    private void Start()
    {
        if (buildOnStart && Application.isPlaying)
        {
            BuildCompleteScene();
        }
    }

    [ContextMenu("Build Scene Now")]
    public void BuildCompleteScene()
    {
        // Clear previous children if any
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        // Create Materials
        Material matGrass = CreateMaterial("Mat_Grass", grassColor, 0.0f, 0.1f);
        Material matRoad = CreateMaterial("Mat_Road", roadColor, 0.05f, 0.2f);
        Material matMarking = CreateMaterial("Mat_RoadMarking", markingColor, 0.0f, 0.3f);
        Material matSidewalk = CreateMaterial("Mat_Sidewalk", sidewalkColor, 0.0f, 0.15f);
        Material matPillar = CreateMaterial("Mat_PillarStone", pillarColor, 0.1f, 0.25f);
        Material matUpper = CreateMaterial("Mat_UpperSurface", upperSurfaceColor, 0.15f, 0.35f);
        Material matIron = CreateMaterial("Mat_GateIron", gateIronColor, 0.85f, 0.45f);
        Material matSign = CreateMaterial("Mat_Signboard", signboardColor, 0.2f, 0.6f);

        // 1. Root Folders
        GameObject envFolder = CreateEmpty("Environment", transform);
        GameObject gateFolder = CreateEmpty("Gate_Structure", transform);

        // 2. Build Ground & Road
        BuildGroundAndRoad(envFolder.transform, matGrass, matRoad, matMarking, matSidewalk);

        // 3. Build Pillars & Upper Surface
        BuildPillarsAndRoof(gateFolder.transform, matPillar, matUpper, matSign);

        // 4. Build Movable Gate Wings
        BuildGateWings(gateFolder.transform, matIron);

        // 5. Build Security Booth & Street Lights
        BuildAccessories(envFolder.transform, matSidewalk, matPillar, matIron);

        // 6. Setup Lighting and Camera
        SetupLightingAndCamera();

        Debug.Log("<color=#00FF88><b>[VIT Gate Scene]</b></color> Scene generated successfully!");
    }

    private void BuildGroundAndRoad(Transform parent, Material matGrass, Material matRoad, Material matMarking, Material matSidewalk)
    {
        // Large surrounding grass terrain
        GameObject grass = GameObject.CreatePrimitive(PrimitiveType.Cube);
        grass.name = "Ground_Grass";
        grass.transform.SetParent(parent);
        grass.transform.position = new Vector3(0, -0.25f, 0);
        grass.transform.localScale = new Vector3(80f, 0.5f, roadLength + 20f);
        grass.GetComponent<Renderer>().sharedMaterial = matGrass;

        // Asphalt Road
        GameObject road = GameObject.CreatePrimitive(PrimitiveType.Cube);
        road.name = "Asphalt_Road";
        road.transform.SetParent(parent);
        road.transform.position = new Vector3(0, 0.04f, 0);
        road.transform.localScale = new Vector3(roadWidth, 0.1f, roadLength);
        road.GetComponent<Renderer>().sharedMaterial = matRoad;

        // Road Markings (Center dashed lines)
        GameObject markingsFolder = CreateEmpty("Road_Markings", parent);
        float dashLength = 3.0f;
        float dashGap = 3.0f;
        float zStart = -(roadLength / 2f) + 2f;
        float zEnd = (roadLength / 2f) - 2f;

        for (float z = zStart; z < zEnd; z += (dashLength + dashGap))
        {
            // Skip directly under the gate threshold
            if (Mathf.Abs(z) < 1.5f) continue;

            GameObject dash = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dash.name = $"DashLine_{z:F0}";
            dash.transform.SetParent(markingsFolder.transform);
            dash.transform.position = new Vector3(0, 0.10f, z + (dashLength / 2f));
            dash.transform.localScale = new Vector3(0.25f, 0.02f, dashLength);
            dash.GetComponent<Renderer>().sharedMaterial = matMarking;
            RemoveCollider(dash);
        }

        // White shoulder lines on both sides
        float shoulderOffset = (roadWidth / 2f) - 0.35f;
        for (int side = -1; side <= 1; side += 2)
        {
            GameObject shoulder = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shoulder.name = side == -1 ? "Shoulder_Left" : "Shoulder_Right";
            shoulder.transform.SetParent(markingsFolder.transform);
            shoulder.transform.position = new Vector3(side * shoulderOffset, 0.10f, 0);
            shoulder.transform.localScale = new Vector3(0.18f, 0.02f, roadLength);
            shoulder.GetComponent<Renderer>().sharedMaterial = matMarking;
            RemoveCollider(shoulder);
        }

        // Sidewalks on left and right
        float sidewalkWidth = 2.5f;
        float sidewalkHeight = 0.22f;
        float sidewalkOffset = (roadWidth / 2f) + (sidewalkWidth / 2f);

        for (int side = -1; side <= 1; side += 2)
        {
            GameObject sidewalk = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sidewalk.name = side == -1 ? "Sidewalk_Left" : "Sidewalk_Right";
            sidewalk.transform.SetParent(parent);
            sidewalk.transform.position = new Vector3(side * sidewalkOffset, sidewalkHeight / 2f, 0);
            sidewalk.transform.localScale = new Vector3(sidewalkWidth, sidewalkHeight, roadLength);
            sidewalk.GetComponent<Renderer>().sharedMaterial = matSidewalk;
        }
    }

    private void BuildPillarsAndRoof(Transform parent, Material matPillar, Material matUpper, Material matSign)
    {
        float halfDist = pillarDistance / 2f;

        // --- TWO PILLARS ---
        for (int side = -1; side <= 1; side += 2)
        {
            string pName = side == -1 ? "Pillar_Left" : "Pillar_Right";
            GameObject pillarRoot = CreateEmpty(pName, parent);
            pillarRoot.transform.position = new Vector3(side * halfDist, 0, 0);

            // 1. Base Pedestal
            GameObject baseBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            baseBlock.name = "Base_Pedestal";
            baseBlock.transform.SetParent(pillarRoot.transform);
            baseBlock.transform.localPosition = new Vector3(0, 0.5f, 0);
            baseBlock.transform.localScale = new Vector3(pillarWidth + 0.6f, 1.0f, pillarWidth + 0.6f);
            baseBlock.GetComponent<Renderer>().sharedMaterial = matPillar;

            // 2. Base Step Trim
            GameObject baseTrim = GameObject.CreatePrimitive(PrimitiveType.Cube);
            baseTrim.name = "Base_Trim";
            baseTrim.transform.SetParent(pillarRoot.transform);
            baseTrim.transform.localPosition = new Vector3(0, 1.15f, 0);
            baseTrim.transform.localScale = new Vector3(pillarWidth + 0.3f, 0.3f, pillarWidth + 0.3f);
            baseTrim.GetComponent<Renderer>().sharedMaterial = matPillar;

            // 3. Main Pillar Shaft
            float shaftHeight = pillarHeight - 1.5f;
            GameObject shaft = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shaft.name = "Column_Shaft";
            shaft.transform.SetParent(pillarRoot.transform);
            shaft.transform.localPosition = new Vector3(0, 1.3f + (shaftHeight / 2f), 0);
            shaft.transform.localScale = new Vector3(pillarWidth, shaftHeight, pillarWidth);
            shaft.GetComponent<Renderer>().sharedMaterial = matPillar;

            // 4. Capital / Top Trim
            GameObject capital = GameObject.CreatePrimitive(PrimitiveType.Cube);
            capital.name = "Capital_Top";
            capital.transform.SetParent(pillarRoot.transform);
            capital.transform.localPosition = new Vector3(0, pillarHeight + 0.15f, 0);
            capital.transform.localScale = new Vector3(pillarWidth + 0.5f, 0.4f, pillarWidth + 0.5f);
            capital.GetComponent<Renderer>().sharedMaterial = matPillar;
        }

        // --- UPPER SURFACE (ARCHITRAVE, BEAM & ROOF COVERING THE PILLARS) ---
        GameObject upperRoot = CreateEmpty("Upper_Covering_Surface", parent);
        float totalUpperSpan = pillarDistance + pillarWidth + (roofOverhang * 2f);
        float beamY = pillarHeight + 0.4f + (upperSurfaceThickness / 2f);

        // 1. Lower Lintel / Architrave Beam
        GameObject architrave = GameObject.CreatePrimitive(PrimitiveType.Cube);
        architrave.name = "Main_Architrave_Beam";
        architrave.transform.SetParent(upperRoot.transform);
        architrave.transform.position = new Vector3(0, beamY, 0);
        architrave.transform.localScale = new Vector3(pillarDistance + pillarWidth + 0.4f, upperSurfaceThickness, pillarWidth + 0.4f);
        architrave.GetComponent<Renderer>().sharedMaterial = matUpper;

        // 2. Upper Roof Slab covering both pillars and extending out
        float roofY = beamY + (upperSurfaceThickness / 2f) + 0.2f;
        GameObject roofSlab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roofSlab.name = "Roof_Covering_Slab";
        roofSlab.transform.SetParent(upperRoot.transform);
        roofSlab.transform.position = new Vector3(0, roofY, 0);
        roofSlab.transform.localScale = new Vector3(totalUpperSpan, 0.35f, pillarWidth + 1.2f);
        roofSlab.GetComponent<Renderer>().sharedMaterial = matPillar;

        // 3. Top Decorative Crown / Parapet
        float crownY = roofY + 0.3f;
        GameObject crown = GameObject.CreatePrimitive(PrimitiveType.Cube);
        crown.name = "Roof_Decorative_Crown";
        crown.transform.SetParent(upperRoot.transform);
        crown.transform.position = new Vector3(0, crownY, 0);
        crown.transform.localScale = new Vector3(totalUpperSpan * 0.85f, 0.25f, pillarWidth + 0.6f);
        crown.GetComponent<Renderer>().sharedMaterial = matUpper;

        // 4. Front & Back Campus Nameplate / Signboard
        float signWidth = pillarDistance - 1.2f;
        for (int face = -1; face <= 1; face += 2)
        {
            GameObject sign = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sign.name = face == 1 ? "Signboard_Front" : "Signboard_Back";
            sign.transform.SetParent(upperRoot.transform);
            sign.transform.position = new Vector3(0, beamY, face * ((pillarWidth / 2f) + 0.22f));
            sign.transform.localScale = new Vector3(signWidth, upperSurfaceThickness * 0.75f, 0.08f);
            sign.GetComponent<Renderer>().sharedMaterial = matSign;

            // 3D Text Header
            GameObject textObj = new GameObject(face == 1 ? "Text_Front" : "Text_Back");
            textObj.transform.SetParent(sign.transform);
            textObj.transform.localPosition = new Vector3(0, 0, face * -0.6f);
            textObj.transform.localRotation = Quaternion.Euler(0, face == 1 ? 0 : 180f, 0);

            TextMesh textMesh = textObj.AddComponent<TextMesh>();
            textMesh.text = "★ VELLORE INSTITUTE OF TECHNOLOGY ★\nMAIN ENTRANCE GATE";
            textMesh.fontSize = 42;
            textMesh.characterSize = 0.045f;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.color = Color.white;
            textMesh.fontStyle = FontStyle.Bold;
        }
    }

    private void BuildGateWings(Transform parent, Material matIron)
    {
        GameObject gateMechanism = CreateEmpty("Gate_Mechanism", parent);
        GateController controller = gateMechanism.AddComponent<GateController>();

        float innerOpeningWidth = pillarDistance - pillarWidth;
        float singleWingWidth = (innerOpeningWidth / 2f) - 0.05f;
        float gateHeight = 3.6f;
        float gateGroundClearance = 0.3f;

        // Left Wing Pivot (Hinged near left pillar inner face)
        float leftPivotX = -(pillarDistance / 2f) + (pillarWidth / 2f);
        GameObject leftPivot = CreateEmpty("Gate_Wing_Left_Pivot", gateMechanism.transform);
        leftPivot.transform.position = new Vector3(leftPivotX, 0, 0);

        // Right Wing Pivot (Hinged near right pillar inner face)
        float rightPivotX = (pillarDistance / 2f) - (pillarWidth / 2f);
        GameObject rightPivot = CreateEmpty("Gate_Wing_Right_Pivot", gateMechanism.transform);
        rightPivot.transform.position = new Vector3(rightPivotX, 0, 0);

        // Construct Wing Meshes
        BuildSingleGateWing(leftPivot.transform, singleWingWidth, gateHeight, gateGroundClearance, 1, matIron);
        BuildSingleGateWing(rightPivot.transform, singleWingWidth, gateHeight, gateGroundClearance, -1, matIron);

        // Assign to Controller
        controller.leftWing = leftPivot.transform;
        controller.rightWing = rightPivot.transform;
        controller.leftClosedRotation = Vector3.zero;
        controller.rightClosedRotation = Vector3.zero;
        controller.leftOpenRotation = new Vector3(0, -95f, 0);
        controller.rightOpenRotation = new Vector3(0, 95f, 0);
    }

    private void BuildSingleGateWing(Transform pivotTransform, float width, float height, float groundClearance, int direction, Material matIron)
    {
        // Direction: 1 for left wing (extends toward +X), -1 for right wing (extends toward -X)
        float frameThickness = 0.14f;
        float frameDepth = 0.12f;
        float wingCenterX = direction * (width / 2f);
        float wingCenterY = groundClearance + (height / 2f);

        GameObject wingRoot = CreateEmpty("Wing_Frame", pivotTransform);

        // 1. Bottom Frame Bar
        GameObject bottomBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bottomBar.name = "Frame_Bottom";
        bottomBar.transform.SetParent(wingRoot.transform);
        bottomBar.transform.localPosition = new Vector3(wingCenterX, groundClearance + (frameThickness / 2f), 0);
        bottomBar.transform.localScale = new Vector3(width, frameThickness, frameDepth);
        bottomBar.GetComponent<Renderer>().sharedMaterial = matIron;

        // 2. Top Frame Bar
        GameObject topBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        topBar.name = "Frame_Top";
        topBar.transform.SetParent(wingRoot.transform);
        topBar.transform.localPosition = new Vector3(wingCenterX, groundClearance + height - (frameThickness / 2f), 0);
        topBar.transform.localScale = new Vector3(width, frameThickness, frameDepth);
        topBar.GetComponent<Renderer>().sharedMaterial = matIron;

        // 3. Middle Horizontal Rail
        GameObject midBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        midBar.name = "Frame_Middle";
        midBar.transform.SetParent(wingRoot.transform);
        midBar.transform.localPosition = new Vector3(wingCenterX, wingCenterY, 0);
        midBar.transform.localScale = new Vector3(width, frameThickness * 0.8f, frameDepth * 0.8f);
        midBar.GetComponent<Renderer>().sharedMaterial = matIron;

        // 4. Vertical Side Posts (Hinge end and Meeting end)
        float[] postXOffsets = { direction * (frameThickness / 2f), direction * (width - (frameThickness / 2f)) };
        for (int i = 0; i < postXOffsets.Length; i++)
        {
            GameObject post = GameObject.CreatePrimitive(PrimitiveType.Cube);
            post.name = $"Post_{i}";
            post.transform.SetParent(wingRoot.transform);
            post.transform.localPosition = new Vector3(postXOffsets[i], wingCenterY, 0);
            post.transform.localScale = new Vector3(frameThickness, height, frameDepth);
            post.GetComponent<Renderer>().sharedMaterial = matIron;
        }

        // 5. Vertical Iron Grille Bars
        int barCount = 10;
        float barSpacing = width / (barCount + 1);
        float barRadius = 0.035f;

        for (int b = 1; b <= barCount; b++)
        {
            float barX = direction * (b * barSpacing);
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            bar.name = $"Iron_Bar_{b}";
            bar.transform.SetParent(wingRoot.transform);
            bar.transform.localPosition = new Vector3(barX, wingCenterY, 0);
            bar.transform.localScale = new Vector3(barRadius * 2f, (height - (frameThickness * 2f)) / 2f, barRadius * 2f);
            bar.GetComponent<Renderer>().sharedMaterial = matIron;
            RemoveCollider(bar);

            // Spearhead top
            GameObject spear = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spear.name = $"Spearhead_{b}";
            spear.transform.SetParent(wingRoot.transform);
            spear.transform.localPosition = new Vector3(barX, groundClearance + height + 0.08f, 0);
            spear.transform.localScale = new Vector3(0.09f, 0.18f, 0.09f);
            spear.GetComponent<Renderer>().sharedMaterial = matIron;
            RemoveCollider(spear);
        }
    }

    private void BuildAccessories(Transform parent, Material matSidewalk, Material matPillar, Material matIron)
    {
        // Security Cabin near left pillar
        float cabinX = -(pillarDistance / 2f) - (pillarWidth / 2f) - 2.8f;
        GameObject cabin = CreateEmpty("Security_Checkpost_Cabin", parent);
        cabin.transform.position = new Vector3(cabinX, 0, -2.5f);

        // Cabin Main Body
        GameObject cabinBody = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cabinBody.name = "Cabin_Walls";
        cabinBody.transform.SetParent(cabin.transform);
        cabinBody.transform.localPosition = new Vector3(0, 1.4f, 0);
        cabinBody.transform.localScale = new Vector3(2.6f, 2.6f, 3.2f);
        cabinBody.GetComponent<Renderer>().sharedMaterial = matPillar;

        // Cabin Roof
        GameObject cabinRoof = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cabinRoof.name = "Cabin_Roof";
        cabinRoof.transform.SetParent(cabin.transform);
        cabinRoof.transform.localPosition = new Vector3(0, 2.8f, 0);
        cabinRoof.transform.localScale = new Vector3(3.0f, 0.25f, 3.6f);
        cabinRoof.GetComponent<Renderer>().sharedMaterial = matSidewalk;

        // Cabin Window (Glass/Tinted)
        Material matGlass = CreateMaterial("Mat_WindowGlass", new Color(0.2f, 0.4f, 0.5f, 0.7f), 0.3f, 0.9f);
        GameObject window = GameObject.CreatePrimitive(PrimitiveType.Cube);
        window.name = "Window_Glass";
        window.transform.SetParent(cabin.transform);
        window.transform.localPosition = new Vector3(1.31f, 1.6f, 0.2f);
        window.transform.localScale = new Vector3(0.05f, 1.0f, 1.8f);
        window.GetComponent<Renderer>().sharedMaterial = matGlass;

        // Entrance Gate Light Lanterns on Pillars
        for (int side = -1; side <= 1; side += 2)
        {
            float lanternX = side * (pillarDistance / 2f);
            GameObject lanternObj = CreateEmpty(side == -1 ? "Lantern_Left" : "Lantern_Right", parent);
            lanternObj.transform.position = new Vector3(lanternX, pillarHeight - 0.5f, -1.0f);

            GameObject lanternMesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            lanternMesh.name = "Lamp_Body";
            lanternMesh.transform.SetParent(lanternObj.transform);
            lanternMesh.transform.localPosition = Vector3.zero;
            lanternMesh.transform.localScale = new Vector3(0.3f, 0.25f, 0.3f);
            lanternMesh.GetComponent<Renderer>().sharedMaterial = matIron;

            Light ptLight = lanternObj.AddComponent<Light>();
            ptLight.type = LightType.Point;
            ptLight.color = new Color(1.0f, 0.92f, 0.75f); // Warm incandescent
            ptLight.range = 14f;
            ptLight.intensity = 2.0f;
        }
    }

    private void SetupLightingAndCamera()
    {
        // 1. Directional Sun Light
        Light existingSun = FindObjectOfType<Light>();
        GameObject sunObj;
        if (existingSun != null && existingSun.type == LightType.Directional)
        {
            sunObj = existingSun.gameObject;
        }
        else
        {
            sunObj = new GameObject("Sun_Directional_Light");
            sunObj.AddComponent<Light>().type = LightType.Directional;
        }
        sunObj.transform.rotation = Quaternion.Euler(45f, -40f, 0f);
        Light sunLight = sunObj.GetComponent<Light>();
        sunLight.color = new Color(1.0f, 0.97f, 0.92f);
        sunLight.intensity = 1.25f;
        sunLight.shadows = LightShadows.Soft;

        // 2. Main Camera
        Camera mainCam = Camera.main;
        GameObject camObj;
        if (mainCam != null)
        {
            camObj = mainCam.gameObject;
        }
        else
        {
            camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            mainCam = camObj.AddComponent<Camera>();
            camObj.AddComponent<AudioListener>();
        }

        // Position camera to get a clear perspective of the road, pillars, gate and upper surface
        camObj.transform.position = new Vector3(0f, 3.8f, -17f);
        camObj.transform.rotation = Quaternion.Euler(8f, 0f, 0f);

        // Add smooth free fly camera controller if not present
        if (camObj.GetComponent<FreeFlyCamera>() == null)
        {
            camObj.AddComponent<FreeFlyCamera>();
        }
    }

    private Material CreateMaterial(string name, Color color, float metallic, float smoothness)
    {
        // Find suitable shader based on pipeline
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null) shader = Shader.Find("Diffuse");
        if (shader == null) shader = Shader.Find("Unlit/Color");

        Material mat = new Material(shader) { name = name };
        mat.color = color;
        if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
        if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", smoothness);
        if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);

        return mat;
    }

    private GameObject CreateEmpty(string name, Transform parent)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        return go;
    }

    private void RemoveCollider(GameObject go)
    {
        Collider col = go.GetComponent<Collider>();
        if (col != null) DestroyImmediate(col);
    }
}
