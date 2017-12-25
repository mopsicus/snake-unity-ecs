using System;
using LeopotamGroup.Ecs;
using UnityEngine;

public struct DestroyEvent {
	public int Index;
}

public sealed class DestroySystem : IEcsSystem, IEcsUpdateSystem {

	EcsWorld _world;
	EcsFilter _destroyObjects;
	int _destroyComponentId;
	double _lifeTime = 2f;

	void IEcsSystem.Initialize (EcsWorld world) {
		_world = world;
		Debug.LogFormat ("{0} => initialize", GetType ().Name);
		_destroyObjects = _world.GetFilter<DestroyComponent> ();
		_destroyComponentId = _world.GetComponentIndex<DestroyComponent> ();
		_world.OnComponentDetach += OnComponentDetach;
		_world.SubscribeToEvent<DestroyEvent> (OnDestroyEntity);
	}

	void IEcsSystem.Destroy () {
		_world.OnComponentDetach -= OnComponentDetach;
		_world.UnsubscribeFromEvent<DestroyEvent> (OnDestroyEntity);
		Debug.LogFormat ("{0} => destroy", GetType ().Name);
	}

	void OnDestroyEntity (DestroyEvent entity) {
		Debug.Log ("Eat food");
		_world.RemoveEntity (entity.Index);
	}

	void IEcsUpdateSystem.Update () {
		foreach (var entity in _destroyObjects.Entities) {
			var eDestroy = _world.GetComponent<DestroyComponent> (entity, _destroyComponentId);
			TimeSpan diff = DateTime.Now - eDestroy.SpawnTime;
			if (diff.TotalSeconds >= _lifeTime) {
				Debug.Log ("Destroy uneaten food");
				_world.RemoveEntity (entity);
			} 
		}
	}

	void OnComponentDetach (int entity, IEcsComponent component) {
		if (component is GameObjectComponent) {
			GameObject.Destroy ((component as GameObjectComponent).Data);
		}
	}

}