using UnityEngine;
using System.Collections;

namespace com.jbrettob.screens
{
	public class PinchZoom : MonoBehaviour {
		[SerializeField]
		private float _orthographicZoomSpeed = .1f;
		[SerializeField]
		private Vector2 _orthographicSizeClamp = new Vector2(.1f, 10f);
		[SerializeField]
		private float _perspectiveZoomSpeed = .1f;
		[SerializeField]
		private Vector2 _fieldOfViewClamp = new Vector2(.1f, 179.9f);

		private Camera _camera;

		void Awake()
		{
			_camera = Camera.main;
		}

		void Update()
		{
			if (Input.touchCount == 2)
			{
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);

				Vector2 touchZeroPreviousPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePreviousPos = touchOne.position - touchOne.deltaPosition;

				float previousTouchDeltaMagnitute = (touchZeroPreviousPos - touchOnePreviousPos).magnitude;
				float touchDeltaMagnitute = (touchZero.position - touchOne.position).magnitude;

				float deltaMagnitudeDifference = previousTouchDeltaMagnitute - touchDeltaMagnitute;

				if (_camera.isOrthoGraphic)
				{
					_camera.orthographicSize += deltaMagnitudeDifference * _orthographicZoomSpeed;
					_camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _orthographicSizeClamp.x, _orthographicSizeClamp.y);
				}
				else
				{
					_camera.fieldOfView += deltaMagnitudeDifference * _perspectiveZoomSpeed;
					_camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, _fieldOfViewClamp.x, _fieldOfViewClamp.y);
				}
			}
		}
	}
}