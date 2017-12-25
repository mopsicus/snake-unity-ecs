using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour {

	public Text Score;

	EcsWorld _world;

	void OnEnable () {
		_world = new EcsWorld ()
			.AddSystem (new SpawnSystem ())
			.AddSystem (new InputSystem ())
			.AddSystem (new ControlSystem ())
			.AddSystem (new MoveSystem ())
			.AddSystem (new CollisionSystem ())
			.AddSystem (new DestroySystem ());
		_world.Initialize ();

		_world.SubscribeToEvent<int> (OnScore);
		var stats = _world.GetStats ();
		Debug.LogFormat ("[Systems: {0}] [Entities: {1}/{2}] [Components: {3}] [Filters: {4}] [DelayedUpdates: {5}]",
			stats.AllSystems, stats.AllEntities, stats.ReservedEntities, stats.Components, stats.Filters, stats.DelayedUpdates);
	}

	void Update () {
		_world.Update ();
	}

	void OnDisable () {
		_world.UnsubscribeFromEvent<int> (OnScore);
		_world.Destroy ();
		_world = null;
	}

	void OnScore (int score) {
		Score.text = string.Format("Score: {0}", score);
	}
}