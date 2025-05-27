using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace SimpleInputNamespace
{
    public class AxisInputUIArrows : MonoBehaviour, ISimpleInputDraggable
    {
        public SimpleInput.AxisInput xAxis = new SimpleInput.AxisInput("Horizontal");
        public SimpleInput.AxisInput yAxis = new SimpleInput.AxisInput("Vertical");

        public float valueMultiplier = 1f;

        [Tooltip("Radius of the deadzone at the center of the arrows that will yield no input")]
        [SerializeField]
        private float deadzoneRadius = 10f;
        private float deadzoneRadiusSqr;

        private RectTransform rectTransform;

        private Vector2 m_value = Vector2.zero;
        public Vector2 Value { get { return m_value; } }

        private void Awake()
        {
            rectTransform = (RectTransform)transform;
            gameObject.AddComponent<SimpleInputDragListener>().Listener = this;

            deadzoneRadiusSqr = deadzoneRadius * deadzoneRadius;
        }

        private void OnEnable()
        {
            xAxis.StartTracking();
            yAxis.StartTracking();
        }

        private void OnDisable()
        {
            xAxis.StopTracking();
            yAxis.StopTracking();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            deadzoneRadiusSqr = deadzoneRadius * deadzoneRadius;
        }
#endif

        public void OnPointerDown(PointerEventData eventData)
        {
            CalculateInput(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            CalculateInput(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_value = Vector2.zero;

            xAxis.value = 0f;
            yAxis.value = 0f;
        }

        private void CalculateInput(PointerEventData eventData)
        {
            Vector2 pointerPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out pointerPos);

            if (pointerPos.sqrMagnitude <= deadzoneRadiusSqr)
            {
                m_value = Vector2.zero;
            }
            else
            {
                // Угол между pointerPos и Vector2.right (ось X)
                float angle = Vector2.SignedAngle(Vector2.right, pointerPos);
                if (angle < 0f)
                    angle += 360f;

                // Определение направления
                if (angle >= 45f && angle < 135f)
                    m_value.Set(0f, valueMultiplier); // вверх
                else if (angle >= 135f && angle < 225f)
                    m_value.Set(-valueMultiplier, 0f); // влево
                else if (angle >= 225f && angle < 315f)
                    m_value.Set(0f, -valueMultiplier); // вниз
                else
                    m_value.Set(valueMultiplier, 0f); // вправо
                Debug.Log($"Pointer Pos: {pointerPos}, Angle: {angle}");
            }

            xAxis.value = m_value.x;
            yAxis.value = m_value.y;
            
        }
    }
}
