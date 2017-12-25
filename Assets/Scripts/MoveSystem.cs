using System.Collections.Generic;
using System.Linq;
using LeopotamGroup.Ecs;
using UnityEngine;

public sealed class MoveSystem : IEcsSystem, IEcsUpdateSystem {

	EcsWorld _world;
	EcsFilter _goObjects;
	int _goComponentId;
	int _controlComponentId;
	int _snakeComponentId;
	float _step = 0.05f;

	void IEcsSystem.Initialize (EcsWorld world) {
		_world = world;
		Debug.LogFormat ("{0} => initialize", GetType ().Name);
		_goObjects = _world.GetFilter<GameObjectComponent, SnakeComponent> ();
		_controlComponentId = _world.GetComponentIndex<ControlComponent> ();
		_goComponentId = _world.GetComponentIndex<GameObjectComponent> ();
		_snakeComponentId = _world.GetComponentIndex<SnakeComponent> ();
	}

	void IEcsSystem.Destroy () {
		Debug.LogFormat ("{0} => destroy", GetType ().Name);
	}

	void IEcsUpdateSystem.Update () { // need refactor
		foreach (var entity in _goObjects.Entities) {
			var eControl = _world.GetComponent<ControlComponent> (entity, _controlComponentId);
			var eGO = _world.GetComponent<GameObjectComponent> (entity, _goComponentId);
			var snake = _world.GetComponent<SnakeComponent> (entity, _snakeComponentId);
			var pos = eGO.Transform.localPosition;
			var posTail = eGO.Transform.localPosition;
			switch (eControl.Direction) {
				case MoveDirection.DOWN:
					pos.y -= _step;
					posTail.y += _step;
					break;
				case MoveDirection.UP:
					pos.y += _step;
					posTail.y -= _step;
					break;
				case MoveDirection.LEFT:
					pos.x -= _step;
					posTail.x += _step;
					break;
				case MoveDirection.RIGHT:
					pos.x += _step;
					posTail.x -= _step;
					break;
				case MoveDirection.STAY:
					break;
				default:
					break;
			}
			eGO.Transform.localPosition = pos;
			if (snake.Tail.Count > 0) { // need refactor
				snake.Tail.Last ().localPosition = posTail;
				snake.Tail.Insert (0, snake.Tail.Last ());
				snake.Tail.RemoveAt (snake.Tail.Count - 1);
			}
		}

	}

}