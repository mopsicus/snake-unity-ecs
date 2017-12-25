using LeopotamGroup.Ecs;
using UnityEngine;

public sealed class InputSystem : IEcsSystem, IEcsUpdateSystem {

	EcsWorld _world;

	void IEcsSystem.Initialize (EcsWorld world) {
		_world = world;
		Debug.LogFormat ("{0} => initialize", GetType ().Name);
	}

	void IEcsSystem.Destroy () {
		Debug.LogFormat ("{0} => destroy", GetType ().Name);
	}

	void IEcsUpdateSystem.Update () {

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			SendInput(MoveDirection.UP);
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			SendInput(MoveDirection.DOWN);
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			SendInput(MoveDirection.LEFT);
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			SendInput(MoveDirection.RIGHT);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			SendInput(MoveDirection.STAY);
		}

	}

	void SendInput (MoveDirection direction) {
		var data = new ControlEvent ();
		data.Direction = direction;
		_world.PublishEvent (data);
	}

}