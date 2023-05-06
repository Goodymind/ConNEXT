using Godot;

public partial class Main : Node
{
	public override void _Ready()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("intro");
	}
	private void LoadEnvironment()
	{
		GetNode<SceneHandler>("/root/SceneHandler").ChangeSceneTo("res://flowcharter/flowchart_environment.tscn", this);
	}
}
