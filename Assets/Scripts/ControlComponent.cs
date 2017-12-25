using LeopotamGroup.Ecs;

public enum MoveDirection {
	UP = 0,	
	DOWN = 1,
	LEFT = 2,
	RIGHT = 3,
	STAY = 4
}
public sealed class ControlComponent : IEcsComponent {
	public MoveDirection Direction;
}