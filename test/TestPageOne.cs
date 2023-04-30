public partial class TestPageOne : Node
{
    public override void _Ready()
    {
		GetNode<Button>("Main").Pressed += () =>
			GetNode<SceneHandler>("/root/SceneHandler").ChangeSceneTo("res://main.tscn");
		GetNode<Button>("Right").Pressed += () =>
			GetNode<SceneHandler>("/root/SceneHandler").ChangeSceneTo("res://test/test_page_two.tscn");
    }
}
