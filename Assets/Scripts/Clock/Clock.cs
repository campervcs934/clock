using System;
using Clock.API;
using Clock.Models;
using Clock.Models.Enums;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static System.Int32;

namespace Clock
{
    public class Clock : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputHour;
        [SerializeField] private TMP_InputField _inputMinute;
        [SerializeField] private TMP_InputField _inputSeconds;
        [Space]
        [SerializeField] private Transform hourTransform;
        [SerializeField] private Transform minuteTransform;
        [SerializeField] private Transform secondTransform;

        private readonly ITimeApi _timeApi = new YandexApi();

        private int _hours;
        private int _minutes;
        private int _seconds;
        private string _time;

        private TimeSpan _serverTime;
        private TimeSpan _currentTime;

        public TimeSpan ServerTime => _serverTime;

        private bool _onPause;
    
        void Start()
        {
            StartCoroutine(_timeApi.GetTime(time =>
            {
                _serverTime = time.LocalTime;
                _currentTime = _serverTime;
            }));
            AddListeners();
        }

        private void AddListeners()
        {
            _inputHour.onEndEdit.AddListener((input) => OnTimeInputEnd(input, TimeArrows.Hour));
            _inputMinute.onEndEdit.AddListener((input) => OnTimeInputEnd(input, TimeArrows.Minute));
            _inputSeconds.onEndEdit.AddListener((input) => OnTimeInputEnd(input, TimeArrows.Seconds));
        }

        private void FixedUpdate()
        {
            if (_currentTime == TimeSpan.Zero || _onPause) return;
            _currentTime = _currentTime.Add(TimeSpan.FromSeconds(Time.deltaTime));

            SetTime();
            UpdateInputFieldsUI();
            MoveArrows();
        }

        private void SetTime()
        {
            _hours = _currentTime.Hours;
            _minutes = _currentTime.Minutes;
            _seconds = _currentTime.Seconds;
        }

        private void UpdateInputFieldsUI()
        {
            _inputHour.text = $"{_hours}";
            _inputMinute.text = $"{_minutes}";
            _inputSeconds.text = $"{_seconds}";
        }

        private void OnTimeInputEnd(string inputText, TimeArrows type)
        {
            if (string.IsNullOrEmpty(inputText)) return;
        
            SetTime();
            
            switch (type)
            {
                case TimeArrows.Hour:
                    _hours = Mathf.Clamp(Parse(inputText), 0, 23);
                    break;
                case TimeArrows.Minute:
                    _minutes = Mathf.Clamp(Parse(inputText), 0, 59);
                    break;
                case TimeArrows.Seconds:
                    _seconds = Mathf.Clamp(Parse(inputText), 0, 59);
                    break;
            }
            
            _currentTime = TimeSpan.FromSeconds(GetTotalSeconds());
        }

        private double GetTotalSeconds()
        {
            double totalSeconds = _hours * 3600 + _minutes * 60 + _seconds;
            return totalSeconds;
        }

        private void MoveArrows()
        {
            hourTransform.DORotate(new Vector3(0, 0, _hours * -Constants.DegreesInHour), 2f, RotateMode.Fast);
            minuteTransform.DORotate(new Vector3(0, 0, _minutes * -Constants.DegreesInMinute), 2f, RotateMode.Fast);
            secondTransform.DORotate(new Vector3(0, 0, _seconds * -Constants.DegreesInSecond), 2f, RotateMode.Fast);
        }

        public void ChangePlayingServerTime(bool pause) => _onPause = pause;
    
        public void SetHours(int hours)
        {
            _hours = hours;
            _currentTime = TimeSpan.FromSeconds(GetTotalSeconds());
        }

        public void SetMinutes(int minutes)
        {
            _minutes = minutes;
            _currentTime = TimeSpan.FromSeconds(GetTotalSeconds());
        }
    
    }
}