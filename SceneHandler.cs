using Godot;
using System.Collections.Generic;

public partial class SceneHandler : Node
{
	PackedScene flowchartEnvironment = GD.Load<PackedScene>("res://flowcharter/flowchart_environment.tscn");
	[Signal] public delegate void SceneChangedEventHandler(Node scene);
	public Dictionary<string, object> Data = new Dictionary<string, object>();
	private Dictionary<string, PackedScene> loadedScenes = new Dictionary<string, PackedScene>();
	public override void _Ready()
	{
		string path = (string)ProjectSettings.GetSetting("application/run/main_scene");
		loadedScenes[path] = ResourceLoader.Load<PackedScene>(path);
	}
	public void ChangeSceneTo(string path, Node currentScene)
	{
		bool sceneLoadedBefore = loadedScenes.TryGetValue(path, out PackedScene scene);
		if (!sceneLoadedBefore)
		{
			scene = ResourceLoader.Load<PackedScene>(path);
		}
		currentScene.QueueFree();
		currentScene = scene.Instantiate();
		GetTree().Root.AddChild(currentScene);
		EmitSignal(SignalName.SceneChanged, currentScene);
	}

}
