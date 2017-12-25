using LeopotamGroup.Ecs;
using UnityEngine;

public sealed class GameObjectComponent : IEcsComponent {
	public GameObject Data;
	public Transform Transform;

	public void Init (GameObject go) {
		Data = go;
		Transform = go.transform;
	}
}