using System;
using Godot;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;
public partial class Finally : Block
{
    public Finally() => Separate = false;
    public override void _Ready()
    {
        //AddChild(FlowchartGenerator.shapeScene.Instantiate<NewShape>().Init(Name, Line, NewShape.Shapes.PROCESS));
    }
    public override void Update()
    {
        SeparateUpdate();
    }
}