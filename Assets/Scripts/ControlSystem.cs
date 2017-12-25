using LeopotamGroup.Ecs;
using UnityEngine;

public struct ControlEvent {
	public MoveDirection Direction;
}

public sealed class ControlSystem : IEcsSystem {

	EcsWorld _world;
	EcsFilter _controlEvent;
	EcsFilter _controlObjects;
	int _controlComponentId;

	void IEcsSystem.Initialize (EcsWorld world) {
		_world = world;
		Debug.LogFormat ("{0} => initialize", GetType ().Name);
		_world.SubscribeToEvent<ControlEvent> (OnControlEntity);
		_controlObjects = _world.GetFilter<ControlComponent> ();
		_controlComponentId = _world.GetComponentIndex<ControlComponent> ();
	}

	void IEcsSystem.Destroy () {
		Debug.LogFormat ("{0} => destroy", GetType ().Name);
		_world.UnsubscribeFromEvent<ControlEvent> (OnControlEntity);
	}

	void OnControlEntity (ControlEvent data) {
		foreach (var entity in _controlObjects.Entities) {
			var eControl = _world.GetComponent<ControlComponent> (entity, _controlComponentId);
			eControl.Direction = data.Direction;
		}
	}


}