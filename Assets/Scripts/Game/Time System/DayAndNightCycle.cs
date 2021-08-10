using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;
using Sirenix.OdinInspector;

public class DayAndNightCycle : MonoBehaviour
{
    [SerializeField] private Gradient lightColor;
    [SerializeField] private GameObject globalLight;

    private int days;
    public int Days => days;
    [Range(0f, 1f)]
    public float time = 0;
    private bool canChangeDay = true;
    public delegate void OnDayChanged();
    public OnDayChanged DayChanged;

    private void OnEnable()
    {
        // UpdateGameMinute
        EventHandler.AdvanceGameMinuteEvent += UpdateLightColor;
    }
    private void OnDisable()
    {
        // UpdateGameMinute
        EventHandler.AdvanceGameMinuteEvent += UpdateLightColor;
    }

    private void UpdateLightColor(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        time = (float)(gameHour * 60 + gameMinute) % 1440 / 1440;
        // 1440 max minutes in a day
        // 360 = 6hours => start the sunrise at 6am

        globalLight.GetComponent<Light2D>().color = lightColor.Evaluate(time);
    }
}
