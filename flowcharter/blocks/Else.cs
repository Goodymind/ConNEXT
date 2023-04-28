using Godot;
using System;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;
public partial class Else : Block
{
    public Else() => Separate = false;
    public override void Update()
    {int v = 0;
        int prevIfWidth = 0;
        foreach (var f in Children)
        {
            if (f is NewShape shape)
            {
                shape.Position = new Vector2I(UniversalShapeWidth * (prevIfWidth == 0 ? (Width - 1) : Width), UniversalShapeHeight * v);
                v += 1;
                prevIfWidth = 0;
            }
            if (f is Block block)
            {
                if (block.Separate)
                    continue;
                if (block is If || block is While || block is For || block is With)
                {
                    block.Position = new Vector2I(UniversalShapeWidth * (prevIfWidth == 0 ? Width - 1 : Width), UniversalShapeHeight * v);
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
                v += block.Height;
            }
        }
    }
}