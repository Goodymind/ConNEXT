using Godot;
using System.Linq;
using System.Collections.Generic;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;
public partial class Function : Block
{
    public Function()
    {
        Separate = true;
    }
    public override void _Ready()
    {
        AddChild(FlowchartGenerator.shapeScene.Instantiate<NewShape>().Init(Name, Line, NewShape.Shapes.TERMINATOR));
    }
    public override void Update()
    {
        SeparateUpdate();
    }
    
}