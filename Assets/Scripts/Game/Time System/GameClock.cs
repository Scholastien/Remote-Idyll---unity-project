using TMPro;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText = null;
    [SerializeField] private TextMeshProUGUI dateText = null;
    [SerializeField] private TextMeshProUGUI seasonText = null;
    [SerializeField] private TextMeshProUGUI yearText = null;

    private void OnEnable()
    {
        EventHandler.AdvanceGameMinuteEvent += UpdateGameTime;
    }

    private void OnDisable()
    {
        EventHandler.AdvanceGameMinuteEvent -= UpdateGameTime;
    }

    private void UpdateGameTime(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        // update time
        //gameMinute -= (gameMinute % 10);

        string AmPm = (gameHour >= 12) ? "pm" : "am";

        gameHour = (gameHour >= 13) ? gameHour - 12 : gameHour;

        string hour = (gameHour < 10) ? (" " + gameHour.ToString()) : gameHour.ToString();

        string minute = (gameMinute < 10) ? ("0" + gameMinute.ToString()) : gameMinute.ToString();

        string time = hour + ":" + minute + AmPm;

        timeText.SetText("<mspace=mspace=4.5>" + time + "</mspace>");
        dateText.SetText(gameDayOfWeek + ". " + gameDay.ToString());
        seasonText.SetText(gameSeason.ToString());
        yearText.SetText("Year " + gameYear.ToString());
    }
}
