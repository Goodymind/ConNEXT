using System;
using Godot;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;
public partial class For : Block
{
    public For() => Separate = false;
    public override void _Ready()
    {
        AddChild(FlowchartGenerator.shapeScene.Instantiate<NewShape>().Init(Name, Line, NewShape.Shapes.DECISION));
        GetChild(0).GetNode<Control>("Texts").Visible = true;
    }
    public override void Update()
    {
        RightWardUpdate();
        /*
        int i = 0;
        int height = 0;
        int prevI = 0;
        int prevHeight = 0;
        foreach (var (c,index) in Children.WithIndex())
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
                if (block is If || block is While || block is For || block is With || block is Try || block is Finally)
                {
                    height += 1;
                    block.Position = new Vector2I(UniversalShapeWidth * i, UniversalShapeHeight * height);
                    prevI = i;
                    i += block.Width;
                    height += block.Height-1;
                    prevHeight = block.Height;
                }
                if (block is Elif || block is Except)
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
                    var previous = Children[index -1];
                    if (previous is If || previous is Elif)
                        block.Position = new Vector2I(UniversalShapeWidth * (prevI), UniversalShapeHeight * height);
                    else if (previous is Except)
                    {
                        block.Position = new Vector2I(UniversalShapeWidth * Width, UniversalShapeHeight * height);
                        i = Math.Max(i, prevI + block.Width) + 1;
                    }
                    height += block.Height-1;
                    prevHeight = block.Height;
                    i = Math.Max(i, prevI + block.Width);
                }
            }
            
        }
        */
    }


}