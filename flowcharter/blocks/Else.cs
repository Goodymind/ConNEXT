using Godot;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;
public partial class Else : Block
{
    public Else() => Seperate = false;
    public override void Update()
    {
        int v = 0;
        foreach (var f in Children)
        {
            if (f is NewShape shape)
            {
                shape.Position = new Vector2I(0, 96 * v);
                v += 1;
            }
            if (f is Block block)
            {
                if (block.Seperate)
                    continue;
                block.Update();
                block.Position = new Vector2I(0, 96 * v);
                int h = block.Height;
                v += h;
            }
        }
    }
}