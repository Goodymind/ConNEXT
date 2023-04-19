using Godot;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;
public partial class Else : Block
{
    public Else() => Separate = false;
    public override void Update()
    {
        int v = 0;
        foreach (var f in Children)
        {
            if (f is NewShape shape)
            {
                shape.Position = new Vector2I(0, UniversalShapeHeight * v);
                v += 1;
            }
            if (f is Block block)
            {
                if (block.Separate)
                    continue;
                block.Update();
                block.Position = new Vector2I(0, UniversalShapeHeight * v);
                int h = block.Height;
                v += h;
            }
        }
    }
}