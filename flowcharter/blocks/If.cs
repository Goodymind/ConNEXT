using Godot;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;

public partial class If : Block
{
    public If()
    {
        Seperate = false;
    }
    public override void _Ready()
    {
        AddChild(FlowchartGenerator.shapeScene.Instantiate<NewShape>().Init(Name, Name, NewShape.Shapes.DECISION));
    }
    public override void Update()
    {
        int i = 0;
        int height = 0;
        int prevIfWidth = 0;
        foreach (var c in Children)
        {
            if (c is NewShape shape)
            {
                c.Position = new Vector2I(128 * i, 96 * height);
                i += 1;
            }
            if (c is Block block)
            {
                if (block.Seperate)
                    continue;
                block.Update();
                if (block is If)
                {
                    height += block.Height;
                    block.Position = new Vector2I(128 * i, 96 * height);
                    i += block.Width;
                    prevIfWidth= block.Width;
                    height += block.Height;
                }
                if (block is Else)
                {
                    block.Position = new Vector2I(128 * (i-prevIfWidth), 96 * height);
                    height += block.Height;
                }
            }
        }
    }
}