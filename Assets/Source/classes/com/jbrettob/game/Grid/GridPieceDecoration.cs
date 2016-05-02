using UnityEngine;
using System.Collections;

public class GridPieceDecoration : MonoBehaviour {

	public GameObject[] _upperGridDecorations;
	public Transform[] _spawnTransforms;

	private int _spawnChance = 80;
	
	void Start()
	{
		foreach(Transform t in _spawnTransforms)
		{
			int Randomness = Random.Range(0,100);

			if(Randomness <= _spawnChance)
			{
				GameObject gridPieceDecorationPart = Instantiate(_upperGridDecorations[Random.Range(0, _upperGridDecorations.Length)], t.transform.position, t.transform.rotation) as GameObject;
				gridPieceDecorationPart.transform.parent = t.transform;
				gridPieceDecorationPart.transform.localPosition = Vector2.zero;
			}
		}
	}
}
