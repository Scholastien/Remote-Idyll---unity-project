using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Remote_Idyll.Assets.Scripts.Game.Time_System
{
    public class TimeManager : SingletonMonobehaviour<TimeManager>
    {
        private int _gameYear = 1;
        private Season _gameSeason = Season.Spring;
        private int _gameDay = 1;
        private int _gameHour = 6;
        private int _gameMinute = 30;
        private int _gameSecond = 0;
        private string _gameDayOfWeek = "Mon";

        private bool _gameClockPaused = false;

        private float _gameTick = 0f;

        private void Start()
        {
            EventHandler.CallAdvanceGameMinuteEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);
        }

        private void Update()
        {
            if (!_gameClockPaused)
            {
                GameTick();
            }
        }

        private void GameTick()
        {
            _gameTick += Time.deltaTime;
            if (_gameTick >= Settings.secondsPerGameSecond)
            {
                _gameTick -= Settings.secondsPerGameSecond;

                UpdateGameSecond();
            }
        }

        private void UpdateGameSecond()
        {
            _gameSecond++;
            if (_gameSecond >= 60)
            {
                _gameSecond = 0;
                UpdateGameMinute();

                EventHandler.CallAdvanceGameMinuteEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);
            }
            // Debug.Log(
            //     " Year: " + _gameYear +
            //     " Season: " + _gameSeason +
            //     " Day: " + _gameDay +
            //     " Time: " + _gameHour +
            //     ":" + _gameMinute +
            //     ":" + _gameSecond);
        }

        private void UpdateGameMinute()
        {
            _gameMinute++;
            if (_gameMinute >= 60)
            {
                _gameMinute = 0;
                UpdateGameHour();

                EventHandler.CallAdvanceGameHourEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);
            }
        }

        private void UpdateGameHour()
        {
            _gameHour++;
            if (_gameHour >= 24)
            {
                _gameHour = 0;
                UpdateGameDay();

                _gameDayOfWeek = GetDayOfWeek();
                EventHandler.CallAdvanceGameDayEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);
            }
        }

        private void UpdateGameDay()
        {
            _gameDay++;
            if (_gameDay >= 30)
            {
                _gameDay = 1;
                UpdateGameSeason();

                EventHandler.CallAdvanceGameSeasonEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);
            }
        }

        private void UpdateGameSeason()
        {
            int gs = (int)_gameSeason;
            gs++;
            _gameSeason = (Season)gs;
            if (gs >= 4)
            {
                _gameYear++;
                gs = 0;
                _gameSeason = (Season)gs;

                EventHandler.CallAdvanceGameYearEvent(_gameYear, _gameSeason, _gameDay, _gameDayOfWeek, _gameHour, _gameMinute, _gameSecond);
            }
        }

        private string GetDayOfWeek()
        {
            int totalDays = ((int)_gameSeason * 30) + _gameDay;
            int dayOfWeek = totalDays % 7;

            switch (dayOfWeek)
            {
                case 1:
                    return "Mon";
                case 2:
                    return "Tue";
                case 3:
                    return "Wed";
                case 4:
                    return "Thu";
                case 5:
                    return "Fri";
                case 6:
                    return "Sat";
                case 0:
                    return "Sun";


                default:
                    return "";
            }
        }

        [ButtonGroup]
        [Button("+ minute")]
        public void TestAdvanceGameMinute()
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameSecond();
            }
        }

        [ButtonGroup]
        [Button("+ hour")]
        public void TestAdvanceGameHour()
        {
            for (int i = 0; i < 3600; i++)
            {
                UpdateGameSecond();
            }
        }
        [ButtonGroup]
        [Button("+ day")]
        public void TestAdvanceGameDay()
        {
            for (int i = 0; i < 86400; i++)
            {
                UpdateGameSecond();
            }
        }

    }
}