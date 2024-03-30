using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Asterism.UI
{
    [AddComponentMenu("UI/Asterism/AsterismCustomButton")]
    public class AsterismCustomButton : Selectable, IPointerMoveHandler, IPointerClickHandler, ISubmitHandler
    {
        [Serializable]
        public class ButtonDownEvent : UnityEvent { }
        [Serializable]
        public class ButtonUpEvent : UnityEvent<float> { }
        [Serializable]
        public class ButtonEnterEvent : UnityEvent<bool> { }
        [Serializable]
        public class ButtonExitEvent : UnityEvent<bool> { }
        [Serializable]
        public class ButtonClickEvent : UnityEvent { }

        [Header("Custom Action")]
        [FormerlySerializedAs("Allow Movement"), SerializeField]
        private bool _allowMovement = false;

        [Header("Event")]
        [FormerlySerializedAs("onDown"), SerializeField]
        private ButtonDownEvent _onDown = new ButtonDownEvent();
        [FormerlySerializedAs("onUp"), SerializeField]
        private ButtonUpEvent _onUp = new ButtonUpEvent();
        [FormerlySerializedAs("onEnter"), SerializeField]
        private ButtonEnterEvent _onEnter = new ButtonEnterEvent();
        [FormerlySerializedAs("onExit"), SerializeField]
        private ButtonExitEvent _onExit = new ButtonExitEvent();
        [FormerlySerializedAs("onClick"), SerializeField]
        private ButtonClickEvent _onClick = new ButtonClickEvent();

        /// <summary>  </summary>
        public ButtonDownEvent OnDown => _onDown;
        /// <summary>  </summary>
        public ButtonUpEvent OnUp => _onUp;
        /// <summary>  </summary>
        public ButtonEnterEvent OnEnter => _onEnter;
        /// <summary>  </summary>
        public ButtonExitEvent OnExit => _onExit;

        /// <summary>  </summary>
        public bool AllowMovement => _allowMovement;

        /// <summary>  </summary>
        private bool _isDown = false;
        /// <summary>  </summary>
        private bool _isMove = false;
        /// <summary>  </summary>
        private bool _isExit = false;
        /// <summary>  </summary>
        private float _clickTime = 0;

        protected AsterismCustomButton()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventData"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Press();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Asterism.CustomButton.Click", this);
            _onClick.Invoke();
        }

        /// <summary>
        /// Evaluate current state and transition to pressed state.
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            base.OnPointerDown(eventData);
            _isMove = _isExit = false;
            _isDown = true;
            _clickTime = eventData.clickTime;
            _onDown.Invoke();

            UISystemProfilerApi.AddMarker("Asterism.CustomButton.Down", this);
        }

        /// <summary>
        /// Evaluate eventData and transition to appropriate state.
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            base.OnPointerUp(eventData);
            _onUp.Invoke(eventData.clickTime - _clickTime);
            UISystemProfilerApi.AddMarker("Asterism.CustomButton.Up", this);

            var movement = AllowMovement ? false : _isMove;

            if (!movement && !_isExit && _isDown)
            {
                _onClick.Invoke();
                _isDown = false;
                UISystemProfilerApi.AddMarker("Asterism.CustomButton.Click", this);
            }
        }

        /// <summary>
        /// Evaluate current state and transition to appropriate state.
        /// New state could be pressed or hover depending on pressed state.
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            base.OnPointerEnter(eventData);
            _onEnter.Invoke(_isDown);

            UISystemProfilerApi.AddMarker("Asterism.CustomButton.Enter", this);
        }

        /// <summary>
        /// Evaluate current state and transition to normal state.
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            base.OnPointerExit(eventData);
            _isExit = true;
            _onExit.Invoke(_isDown);

            // îÕàÕäOÇ©ÇÁèoÇΩÇÁNormalÇ…ñﬂÇ∑
            DoStateTransition(SelectionState.Normal, false);
            _isDown = false;

            UISystemProfilerApi.AddMarker("Asterism.CustomButton.Exit", this);
        }

        /// <summary>
        /// Move Event
        /// </summary>
        /// <param name="eventData"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnPointerMove(PointerEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            _isMove = true;
        }

        /// <summary>
        /// Set Selection and trasition to appropriate state.
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnSelect(BaseEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            base.OnSelect(eventData);
        }

        /// <summary>
        /// Unset selection and transition to appropriate state.
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnDeselect(BaseEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            base.OnDeselect(eventData);
        }

    }
}
