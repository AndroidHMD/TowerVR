using UnityEngine;
using System.Collections;

/**
 * Marker class for a Tower Piece.
 * 
 * This component should be attached to a GameObject that has a mesh, a mesh collider and a material.
 * */
public class TowerPiece : NetworkedBehaviour
{
	public TowerPieceDifficulty difficulty 
	{ 
		get; private set;
	}

	public MeshFilter meshFilter
	{ 
		get { return internalGetComponent<MeshFilter>(); } 
	}

	public MeshCollider meshCollider 
	{ 
		get { return internalGetComponent<MeshCollider>(); }
	}


	public Material material 
	{ 
		get { return internalGetComponent<Material>(); }
	}

	/**
	 * Internal helper to get a component. Logs an error if none attached.
	 * */
	private T internalGetComponent<T>()
	{
		var component = gameObject.GetComponent<T>();
		if (component == null)
		{
			Debug.LogError("TowerPiece GameObject has no " + typeof(T).GetType().Name + " attached.");
		}
		return component;
	}

	/**
	 * Instantiates a new GameObject and configures it as a tower piece with the specified info.
	 * */
	public static TowerPiece Create(TowerPieceInfo info, Material material)
	{
		GameObject newTowerPiece = new GameObject("TowerPiece");

		// Attached visual components
		newTowerPiece.AddComponent<MeshFilter>().sharedMesh = info.mesh;
		newTowerPiece.AddComponent<MeshRenderer>().sharedMaterial = material;
		newTowerPiece.AddComponent<Rigidbody>().useGravity = true;
		var mc = newTowerPiece.AddComponent<MeshCollider>();
		mc.sharedMesh = info.mesh;
		mc.convex = true;

		// Attach the difficulty
		var tp = newTowerPiece.AddComponent<TowerPiece>();
		tp.difficulty = info.difficulty;

		return tp;
	}
}