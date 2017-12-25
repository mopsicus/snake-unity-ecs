using LeopotamGroup.Ecs;
using UnityEngine;

public sealed class CollisionSystem : IEcsSystem, IEcsUpdateSystem {

	EcsWorld _world;
	EcsFilter _collisionObjects;
	EcsFilter _snakeObjects;
	ColliderComponent _snakeCollider;
	int _colliderComponentId;
	int _snakeComponentId;

	void IEcsSystem.Initialize (EcsWorld world) {
		_world = world;
		Debug.LogFormat ("{0} => initialize", GetType ().Name);
		_colliderComponentId = _world.GetComponentIndex<ColliderComponent> ();
		_snakeComponentId = _world.GetComponentIndex<SnakeComponent> ();
		_collisionObjects = _world.GetFilter<ColliderComponent, DestroyComponent> ();
		_snakeObjects = _world.GetFilter<ColliderComponent, SnakeComponent> ();
	}

	void IEcsSystem.Destroy () {
		Debug.LogFormat ("{0} => destroy", GetType ().Name);
	}

	void IEcsUpdateSystem.Update () {
		_snakeCollider = _world.GetComponent<ColliderComponent> (_snakeObjects.Entities[0], _colliderComponentId);
		SnakeComponent snakeComponent = _world.GetComponent<SnakeComponent> (_snakeObjects.Entities[0], _snakeComponentId);
		foreach (var entity in _collisionObjects.Entities) {
			var eCollider = _world.GetComponent<ColliderComponent> (entity, _colliderComponentId);
			if (eCollider.Collider.bounds.Intersects (_snakeCollider.Collider.bounds)) {
				snakeComponent.Length++;
				_world.PublishEvent<int> (snakeComponent.Length);
				var dData = new DestroyEvent ();
				dData.Index = entity;
				_world.PublishEvent<DestroyEvent> (dData);
				var sData = new SpawnEvent ();
				sData.Length = snakeComponent.Length;
				_world.PublishEvent<SpawnEvent> (sData);
			}

		}

	}

}