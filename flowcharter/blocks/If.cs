namespace Flowcharter.flowcharter.blocks;

using Flowcharter.shapes;

public partial class If : Block
{
    public If()
    {
        Separate = false;
    }
    public override void _Ready()
    {
        AddChild(FlowchartGenerator.shapeScene.Instantiate<NewShape>().Init(Name, Line, NewShape.Shapes.DECISION));
    }
    public override void Update()
    {
        RightWardUpdate();
    }
}