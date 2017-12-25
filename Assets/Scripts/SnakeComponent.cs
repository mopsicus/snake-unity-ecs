using LeopotamGroup.Ecs;
using UnityEngine;
using System.Collections.Generic;

public sealed class SnakeComponent : IEcsComponent {
	public int Length;
	public List<Transform> Tail;
}