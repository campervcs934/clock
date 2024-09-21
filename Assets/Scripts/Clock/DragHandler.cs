using System;
using Clock.Models;
using Clock.Models.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Clock
{
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private TimeArrows _type;

        [SerializeField] private Clock _clock;
    
        [SerializeField] private float _dragSpeed = 100f;

        private bool _isPm;

        private bool IsServerTimePm() => _clock.ServerTime.Hours >= 12;

        public void OnBeginDrag(PointerEventData eventData) => _isPm = IsServerTimePm();

        public void OnEndDrag(PointerEventData eventData)
        {
            var zRotation = transform.eulerAngles.z;
            zRotation = Math.Abs(zRotation - 360f);
            switch (_type)
            {
                case TimeArrows.Hour:
                    var hours = Mathf.RoundToInt(zRotation/ Constants.DegreesInHour);
                    UpdateHours(hours);
                    break;
                case TimeArrows.Minute:
                    var minutes = Mathf.RoundToInt(zRotation / Constants.DegreesInMinute);
                    _clock.SetMinutes(Mathf.Abs(minutes));
                    break;
                case TimeArrows.Seconds:
                    break;
            }
        }

        private void UpdateHours(int hours)
        {
            var newHours = _isPm && IsServerTimePm() ? hours + 12 : hours;
            _clock.SetHours(newHours);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var rotationZ = (eventData.delta.x + eventData.delta.y) * Time.deltaTime * _dragSpeed;
            transform.Rotate(0, 0, rotationZ);
        }

        private void OnTriggerEnter2D(Collider2D other) => _isPm = !_isPm;
    
    }
}