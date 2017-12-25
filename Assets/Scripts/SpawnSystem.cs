using System;
using System.Collections.Generic;
using LeopotamGroup.Ecs;
using UnityEngine;

public struct SpawnEvent {
	public int Length;
}

public sealed class SpawnSystem : IEcsSystem, IEcsUpdateSystem {

	EcsWorld _world;

	DateTime _lastSpawn;
	double _spawnStep = 2f;
	GameObject _foodPrefab;
	GameObject _tailPrefab;
	int _snakeEntity;

	void IEcsSystem.Initialize (EcsWorld world) {
		_world = world;
		Debug.LogFormat ("{0} => initialize", GetType ().Name);
		_world.SubscribeToEvent<SpawnEvent> (OnSpawnTail);
		_foodPrefab = Resources.Load<GameObject> ("Food");
		_tailPrefab = Resources.Load<GameObject> ("SnakeTail");
		_lastSpawn = DateTime.Now;
		SpawnSnake ();
	}

	void IEcsSystem.Destroy () {
		Debug.LogFormat ("{0} => destroy", GetType ().Name);
		_world.SubscribeToEvent<SpawnEvent> (OnSpawnTail);
	}

	void IEcsUpdateSystem.Update () {
		TimeSpan diff = System.DateTime.Now - _lastSpawn;
		if (diff.TotalSeconds >= _spawnStep) {
			SpawnFood ();
			_lastSpawn = DateTime.Now;
		}
	}

	void SpawnSnake () {
		Debug.Log ("Spawn snake");
		GameObject headPrefab = Resources.Load<GameObject> ("SnakeHead");
		GameObject head = GameObject.Instantiate (headPrefab, Vector3.zero, Quaternion.identity);
		_snakeEntity = _world.CreateEntity ();
		SnakeComponent sc = _world.AddComponent<SnakeComponent> (_snakeEntity);
		sc.Length = 0;
		sc.Tail = new List<Transform> ();
		_world.AddComponent<ControlComponent> (_snakeEntity);
		_world.AddComponent<GameObjectComponent> (_snakeEntity).Init (head);
		_world.AddComponent<ColliderComponent> (_snakeEntity).Collider = head.GetComponent<Collider2D> ();
	}

	void SpawnFood () {
		Debug.Log ("Spawn food");
		GameObject food = GameObject.Instantiate (_foodPrefab, new Vector3 (UnityEngine.Random.Range (-3f, 3f), UnityEngine.Random.Range (-3f, 3f), 0f), Quaternion.identity);
		int _foodEntity = _world.CreateEntity ();
		_world.AddComponent<ControlComponent> (_foodEntity);
		_world.AddComponent<DestroyComponent> (_foodEntity).SpawnTime = DateTime.Now.AddSeconds (UnityEngine.Random.Range (0f, 5f));
		_world.AddComponent<GameObjectComponent> (_foodEntity).Init (food);
		_world.AddComponent<ColliderComponent> (_foodEntity).Collider = food.GetComponent<Collider2D> ();
	}

	void OnSpawnTail (SpawnEvent data) {
		Debug.Log ("Spawn tail");
		SnakeComponent snake = _world.GetComponent<SnakeComponent> (_snakeEntity);
		GameObjectComponent snakeGO = _world.GetComponent<GameObjectComponent> (_snakeEntity);
		GameObject tail = GameObject.Instantiate (_tailPrefab, snakeGO.Transform.localPosition, Quaternion.identity);
		snake.Tail.Insert (0, tail.transform);

	}

}