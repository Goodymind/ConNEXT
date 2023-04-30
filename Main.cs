global using Godot;
global using System;
global using System.Linq;
global using System.Collections.Generic;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<SceneHandler>("/root/SceneHandler").currentScene = this;
		GetNode<Button>("Left").Pressed += () => 
			GetNode<SceneHandler>("/root/SceneHandler").ChangeSceneTo("res://test/test_page_one.tscn");
		GetNode<Button>("Right").Pressed += () =>
			GetNode<SceneHandler>("/root/SceneHandler").ChangeSceneTo("res://test/test_page_two.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
