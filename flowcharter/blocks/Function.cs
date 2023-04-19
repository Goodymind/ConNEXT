using Godot;
using System;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;
public partial class Function: Block
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
        int v = 0;
        int prevIfWidth = 0;
        foreach (var f in Children)
        {
            if (f is NewShape shape)
            {
                shape.Position = new Vector2I(UniversalShapeWidth * ( prevIfWidth == 0 ? (Width - 1): Width), UniversalShapeHeight * v);
                v += 1;
                prevIfWidth = 0;
            }
            if (f is Block block)
            {
                if (block.Separate)
                    continue;
                if (block is If)
                {
                    block.Position = new Vector2I(UniversalShapeWidth * (prevIfWidth == 0? Width - 1: Width), UniversalShapeHeight * v);
                    block.Update();
                    prevIfWidth = block.Width;
                }
                if (block is Else)
                {
                    block.Position = new Vector2I(UniversalShapeWidth * (Width - prevIfWidth), UniversalShapeHeight * v);
                    block.Update();
                    prevIfWidth = Math.Max(prevIfWidth, block.Width);
                }
                if (block is Elif)
                {
                    block.Position = new Vector2I(UniversalShapeWidth * (Width - prevIfWidth), UniversalShapeHeight * v);
                    block.Update();
                    prevIfWidth = Math.Max(prevIfWidth, block.Width);
                }
                if (block is While)
                {
                    v += 1;
                    block.Position = new Vector2I(UniversalShapeWidth * (Width - Math.Max(1,prevIfWidth)), UniversalShapeHeight * v);
                    block.Update();
                    prevIfWidth = Math.Max(prevIfWidth, block.Width);
                }
                block.Update();
                v += block.Height;
            }
        }
    }
}