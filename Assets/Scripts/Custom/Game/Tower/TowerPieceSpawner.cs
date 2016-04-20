using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerPieceSpawner : MonoBehaviour
{
	public List<Material> materials;
	public List<TowerPieceInfo> towerPieceInfos;

	private IList<TowerPiece> towerPieces = new List<TowerPiece>();

	void SpawnTowerPiece()
	{
		var mtlIndex = Random.Range(0, materials.Capacity);
		var mtl = materials[mtlIndex];

		var tpiIndex = Random.Range(0, towerPieceInfos.Capacity);
		var tpi = towerPieceInfos[tpiIndex];

		var tp = TowerPiece.Create(tpi, mtl);
		towerPieces.Add(tp);
	}

	void Start ()
	{
		InvokeRepeating("SpawnTowerPiece", 0, 1.0f);
	}
}