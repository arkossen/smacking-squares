using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public float strength = 0.2f;

	private Vector3 _startPos;
	private Camera _cameraToShake;
	private float _timeSinceLastOffsetChange;
	private float _durationStillShaking;
	private float _bonusStrength = 0;

	#region unity
	void Start()
	{
		_startPos = Camera.main.transform.localPosition;
	}
	
	void Update()
	{
		if (!_cameraToShake) { return; }
		_durationStillShaking -= Time.deltaTime;

		if (_durationStillShaking <= 0) {
			_cameraToShake.transform.localPosition = _startPos;
			_cameraToShake = null;
		} else {
			_timeSinceLastOffsetChange += Time.deltaTime;
			// if (timeSinceLastOffsetChange < cameraShakeValues.shakeInterval) { return; }
			_timeSinceLastOffsetChange = 0.0f;
			
			float randomX = Random.Range (-1.0f, 1.0f);
			float randomY = Random.Range (-1.0f, 1.0f);
			float randomZ = Random.Range (-1.0f, 1.0f);
			
			Vector3 randomDirection = new Vector3 (randomX, randomY, randomZ);
			randomDirection.Normalize ();
			
			Vector3 shakeOffset = randomDirection * _durationStillShaking * (strength + _bonusStrength);
			
			_cameraToShake.transform.localPosition = _startPos + shakeOffset;
		}
	}
	#endregion

	#region public
	public void ShakeCamera(Camera camera, float duration = 1.0f, float extra = 0, bool additive = true)
	{
		this._cameraToShake = camera;
		if (extra > 0) {
			_bonusStrength = extra;
		} else {
			_bonusStrength = 0;
		}
		
		if (additive) {
			if (_durationStillShaking <= 1) {
				this._durationStillShaking += duration;
			}
		} else {
			this._durationStillShaking = duration;
		}
	}
	
	public void StopShakingCamera()
	{
		_cameraToShake.transform.localPosition = _startPos;
		_cameraToShake = null;
	}
	
	public bool IsShakingCamera()
	{
		return _cameraToShake != null;
	}
	#endregion
}

