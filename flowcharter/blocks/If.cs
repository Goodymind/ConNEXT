using Godot;
using Flowcharter.shapes;
using System;
namespace Flowcharter.flowcharter.blocks;

public partial class If : Block
{
    public If()
    {
        Separate = false;
    }
    public override void _Ready()
    {
        AddChild(FlowchartGenerator.shapeScene.Instantiate<NewShape>().Init(Name, Line, NewShape.Shapes.DECISION));
        GetChild(0).GetNode<Control>("Texts").Visible = true;
    }
    public override void Update()
    {
        RightWardUpdate();
    }
}