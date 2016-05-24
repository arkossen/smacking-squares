using UnityEngine;
using System.Collections;

public class ShaderSwitcher : MonoBehaviour {

	public Transform[] transforms;

	private static Shader _SELECTION_SHADER;
	private static Shader _NON_SELECTION_SHADER;

	private bool _enabled;
	
	void Awake()
	{
		if (_SELECTION_SHADER == null) _SELECTION_SHADER = Shader.Find("Custom/DissolveForceField");
		if (_NON_SELECTION_SHADER == null) _NON_SELECTION_SHADER = Shader.Find("Diffuse");
	}

	public void SwitchShader()
	{
		foreach(Transform t in transforms)
		{
			t.renderer.material.shader = (_enabled) ? _NON_SELECTION_SHADER : _SELECTION_SHADER;
		}

		_enabled = !_enabled;
	}

	public bool isEnabled
	{
		get { return _enabled; }
	}
}
