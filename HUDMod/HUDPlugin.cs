using MelonLoader;
using Il2Cpp;
using UnityEngine;
using UnityEngine.UI;
using Il2CppInterop.Runtime;
using Il2CppTMPro;
using System;

[assembly: MelonInfo(typeof(HUDMod.HUDPlugin), "HUD", "0.1.0", "lukeod")]
[assembly: MelonGame("Shape Shop", "Mars First Logistics")]

namespace HUDMod
{
    public class HUDPlugin : MelonMod
    {
        private Game? gameInstance;
        private float updateTimer = 0f;
        private const float UPDATE_INTERVAL = 0.1f;

        // UI Components
        private GameObject? canvasGO;
        private GameObject? hudGO;
        private TextMeshProUGUI? speedText;
        private TextMeshProUGUI? compassText;
        private bool uiInitialized = false;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("HUD Mod v0.1.0 Loaded!");
        }

        private void InitializeUI()
        {
            try
            {
                // Create Canvas GameObject
                canvasGO = new GameObject("HUDCanvas");
                UnityEngine.Object.DontDestroyOnLoad(canvasGO);

                // Add Canvas component
                var canvas = canvasGO.AddComponent(Il2CppType.Of<Canvas>()).Cast<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 100;

                // Add CanvasScaler for proper scaling
                var scaler = canvasGO.AddComponent(Il2CppType.Of<CanvasScaler>()).Cast<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);

                // Add GraphicRaycaster
                canvasGO.AddComponent(Il2CppType.Of<GraphicRaycaster>());

                // Create hud panel
                hudGO = new GameObject("HUDPanel");
                hudGO.transform.SetParent(canvasGO.transform, false);

                var rectTransform = hudGO.GetComponent<RectTransform>();
                if (rectTransform == null)
                {
                    rectTransform = hudGO.AddComponent(Il2CppType.Of<RectTransform>()).Cast<RectTransform>();
                }

                // Position in top-left corner
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 1);
                rectTransform.anchoredPosition = new Vector2(20, -20);
                rectTransform.sizeDelta = new Vector2(400, 200);

                // Add background panel for better visibility
                var bgImage = hudGO.AddComponent(Il2CppType.Of<Image>()).Cast<Image>();
                bgImage.color = new Color(0, 0, 0, 0.6f);

                // Create speed text
                var speedTextGO = new GameObject("SpeedText");
                speedTextGO.transform.SetParent(hudGO.transform, false);
                var speedRT = speedTextGO.AddComponent(Il2CppType.Of<RectTransform>()).Cast<RectTransform>();
                speedRT.anchorMin = new Vector2(0, 0.5f);
                speedRT.anchorMax = new Vector2(1, 1);
                speedRT.pivot = new Vector2(0.5f, 1);
                speedRT.anchoredPosition = new Vector2(0, -10);
                speedRT.sizeDelta = new Vector2(-20, -20);

                speedText = speedTextGO.AddComponent(Il2CppType.Of<TextMeshProUGUI>()).Cast<TextMeshProUGUI>();
                speedText.fontSize = 28;
                speedText.color = new Color(0.2f, 1f, 0.4f, 1f);
                speedText.alignment = TextAlignmentOptions.Center;
                speedText.fontStyle = FontStyles.Bold;
                speedText.text = "0 km/h";

                var speedOutline = speedTextGO.AddComponent(Il2CppType.Of<Outline>()).Cast<Outline>();
                speedOutline.effectColor = new Color(0, 0, 0, 1f);
                speedOutline.effectDistance = new Vector2(2, -2);

                // Create compass text
                var compassTextGO = new GameObject("CompassText");
                compassTextGO.transform.SetParent(hudGO.transform, false);
                var compassRT = compassTextGO.AddComponent(Il2CppType.Of<RectTransform>()).Cast<RectTransform>();
                compassRT.anchorMin = new Vector2(0, 0);
                compassRT.anchorMax = new Vector2(1, 0.5f);
                compassRT.pivot = new Vector2(0.5f, 0);
                compassRT.anchoredPosition = new Vector2(0, 10);
                compassRT.sizeDelta = new Vector2(-20, -20);

                compassText = compassTextGO.AddComponent(Il2CppType.Of<TextMeshProUGUI>()).Cast<TextMeshProUGUI>();
                compassText.fontSize = 32;
                compassText.color = new Color(1f, 0.8f, 0.2f, 1f);
                compassText.alignment = TextAlignmentOptions.Center;
                compassText.fontStyle = FontStyles.Bold;
                compassText.text = "N";

                var compassOutline = compassTextGO.AddComponent(Il2CppType.Of<Outline>()).Cast<Outline>();
                compassOutline.effectColor = new Color(0, 0, 0, 1f);
                compassOutline.effectDistance = new Vector2(2, -2);

                canvasGO.SetActive(false);
                uiInitialized = true;
            }
            catch (Exception ex)
            {
                LoggerInstance.Error($"Failed to initialize UI: {ex.Message}");
            }
        }

        public override void OnUpdate()
        {
            updateTimer += Time.deltaTime;
            if (updateTimer >= UPDATE_INTERVAL)
            {
                updateTimer = 0f;
                UpdateHUD();
            }
        }

        private void UpdateHUD()
        {
            try
            {
                if (gameInstance == null)
                {
                    gameInstance = Conf.g;
                }

                if (gameInstance == null) return;

                if (!uiInitialized)
                {
                    InitializeUI();
                    return;
                }

                bool isDriving = gameInstance.state == Game.State.Driving;
                var player = gameInstance.player;
                var vehicle = player?.vehicle;

                if (canvasGO != null)
                {
                    bool shouldShow = isDriving && vehicle != null;
                    if (canvasGO.active != shouldShow)
                    {
                        canvasGO.SetActive(shouldShow);
                    }
                }

                if (isDriving && vehicle != null && player != null && speedText != null && compassText != null)
                {
                    // Get speed
                    float speedMPS = vehicle.vehicleSpeed;
                    float speedKPH = speedMPS * 3.6f;
                    float speedMPH = speedMPS * 2.23694f;

                    // Get velocity for vertical speed
                    var velocity = vehicle.vehicleVelocity;
                    float verticalSpeed = velocity.y * 3.6f;

                    // Update speed display
                    speedText.text = $"{speedKPH:F0} km/h  |  {speedMPH:F0} mph\n";
                    if (Math.Abs(verticalSpeed) > 1f)
                    {
                        string verticalIndicator = verticalSpeed > 0 ? "↑" : "↓";
                        speedText.text += $"Vertical: {Math.Abs(verticalSpeed):F0} km/h {verticalIndicator}";
                    }

                    // Calculate heading
                    var forward = player.forwardDir;
                    float angle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
                    if (angle < 0) angle += 360;

                    // Get compass direction and mass
                    string compassDir = GetCompassDirection(angle);
                    float totalMass = vehicle.totalMass;
                    compassText.text = $"{compassDir}  {angle:F0}°\n{totalMass:F0} kg";

                    // Dynamic opacity based on speed
                    float alpha = Mathf.Clamp01(0.5f + (speedMPS / 15f) * 0.5f);
                    speedText.color = new Color(0.2f, 1f, 0.4f, alpha);
                    compassText.color = new Color(1f, 0.8f, 0.2f, alpha);
                }
            }
            catch
            {
                // Silent fail
            }
        }

        private string GetCompassDirection(float angle)
        {
            string[] directions = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };
            int index = (int)((angle + 22.5f) / 45f) % 8;
            return directions[index];
        }
    }
}
