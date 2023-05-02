using Godot;
using System;
using Flowcharter.flowcharter.blocks;
namespace Flowcharter.shapes;
public partial class NewShape : Sprite2D
{
    public enum Shapes
    {
        //process/operation symbols
        PROCESS,
        //Branching and control flow
        TERMINATOR, DECISION
    }
    private Shapes shape;
    public string line;
    public NewShape Init(string name, string line, Shapes shape)
    {
        this.shape = shape;
        Name = name;
        this.line = line;
        //GD.PrintT(line);
        return this;
    }
    public override void _Ready()
    {
        Texture = GD.Load<Texture2D>($"res://shapes/images/{shape}.svg");
        GetNode<Label>("Label").Text = line.Trim();
        GetNode<Label>("Label").TooltipText = line.Trim();

    }
}