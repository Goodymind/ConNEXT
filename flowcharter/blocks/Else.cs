using Godot;
using System;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;
public partial class Else : Block
{
    public Else() => Separate = false;
    public override void Update()
    {
        SeparateUpdate();
    }
}