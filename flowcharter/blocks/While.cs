using System;
using Godot;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;
public partial class While : Block
{
    public While() => Separate = false;
    public override void _Ready() =>
        AddChild(FlowchartGenerator.shapeScene.Instantiate<NewShape>().Init(Name, Line, NewShape.Shapes.DECISION));
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
                height += prevHeight == 0? 0: 1;
                c.Position = new Vector2I(UniversalShapeWidth * i, UniversalShapeHeight * height);
                i += 1;
                prevHeight = 0;
            }
            if (c is Block block)
            {
                if (block.Separate)
                    continue;
                block.Update();
                if (block is If || block is While || block is For)
                {
                    height += 1;
                    block.Position = new Vector2I(UniversalShapeWidth * i, UniversalShapeHeight * height);
                    prevI = i;
                    i += block.Width;
                    height += block.Height-1;
                    prevHeight = block.Height;
                }
                if (block is Elif)
                {
                    height += 1;
                    block.Position = new Vector2I(UniversalShapeWidth * (prevI), UniversalShapeHeight * height);
                    height += block.Height-1;
                    prevHeight = block.Height;
                    i = Math.Max(i, prevI + block.Width);
                }
                if (block is Else)
                {
                    height += 1;
                    block.Position = new Vector2I(UniversalShapeWidth * (prevI), UniversalShapeHeight * height);
                    height += block.Height-1;
                    prevHeight = block.Height;
                    i = Math.Max(i, prevI + block.Width);
                }
            }
        }
    }


}