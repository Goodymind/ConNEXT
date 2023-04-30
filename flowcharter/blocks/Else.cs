namespace Flowcharter.flowcharter.blocks;

using Flowcharter.shapes;

public partial class Else : Block
{
    public Else() => Separate = false;
    public override void Update()
    {
        SeparateUpdate();
    }
}