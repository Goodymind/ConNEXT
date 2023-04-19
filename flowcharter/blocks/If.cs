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
    }
    public override void Update()
    {
        int i = 0;
        int height = 0;
        int prevI = 0;
        int prevHeight = 0;
        foreach (var c in Children)
        {
            if (c is NewShape shape)
            {
                c.Position = new Vector2I(UniversalShapeWidth * i, UniversalShapeHeight * height);
                i += 1;
                prevHeight = 0;
            }
            if (c is Block block)
            {
                if (block.Separate)
                    continue;
                block.Update();
                //GD.PrintT(GetPath(), i, block.Name);
                if (block is If)
                {
                    height += Math.Max(prevHeight, 1);
                    block.Position = new Vector2I(UniversalShapeWidth * i, UniversalShapeHeight * height);
                    prevI = i;
                    i += block.Width;
                    prevHeight = block.Height;
                    height += block.Height;
                }
                if (block is Elif)
                {
                    block.Position = new Vector2I(UniversalShapeWidth * (prevI), UniversalShapeHeight * height);
                    height += block.Height;
                    prevHeight += block.Height;
                    i = Math.Max(i, prevI + block.Width);
                }
                if (block is Else)
                {
                    block.Position = new Vector2I(UniversalShapeWidth * (prevI), UniversalShapeHeight * height);
                    height += block.Height;
                    prevHeight += block.Height;
                    i = Math.Max(i, prevI + block.Width);
                }
            }
        }
    }
}