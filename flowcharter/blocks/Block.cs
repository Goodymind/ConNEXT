using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;
public partial class Block : Node2D
{
    public static PackedScene lineScene = GD.Load<PackedScene>("res://flowcharter/line_drawer.tscn");
    public List<Vector2> Output
    {
        get
        {
            if (Children.Last() is NewShape shape)
            {
                if (shape.line.ToString().ContainsBreakers())
                {
                    return new List<Vector2>();
                }
                return new List<Vector2>() { shape.GlobalPosition };
            }
            List<Vector2> outputs = new List<Vector2>();
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                var c = Children[i];
                if (c is NewShape newshape)
                {
                    outputs.Add(c.GlobalPosition);
                    return outputs;
                }
                var child = c as Block;
                if (child is If)
                    outputs.AddRange(child.Output);
                if (child is Else || child is Elif)
                    outputs.AddRange(child.Output);
                if (child is While || child is For)
                    outputs.Add(child.GlobalPosition);
            }
            return outputs;
        }
    }
    public static int UniversalShapeWidth = 136;
    public static int UniversalShapeHeight = 102;
    public static Vector2I UnivScale => new Vector2I(UniversalShapeWidth, UniversalShapeHeight);
    public bool Separate;
    public int LeadingSpaces;
    protected bool Updated = false;
    public string Line;
    public int Width
    {
        get
        {
            int farthestX = 0;
            Node2D currentNode = this;
            Stack<Node2D> unVisited = new Stack<Node2D>();
            foreach (var node in Children)
            {
                unVisited.Push(node);
            }
            while (unVisited.Count != 0)
            {
                currentNode = unVisited.Pop();
                if (currentNode is NewShape shape)
                {
                    farthestX = Math.Max(farthestX, (int)currentNode.GlobalPosition.X);
                }
                else if (currentNode is Block block)
                {
                    if (block.Separate)
                        continue;
                    foreach (var child in block.Children)
                    {
                        unVisited.Push(child);
                    }
                }
            }
            return (int)(farthestX - GlobalPosition.X) / UniversalShapeWidth + 1;
        }
    }
    public int Height
    {
        get
        {
            int farthestX = 0;
            Node2D currentNode = this;
            Stack<Node2D> unVisited = new Stack<Node2D>();
            foreach (var node in Children)
            {
                unVisited.Push(node);
            }
            while (unVisited.Count != 0)
            {
                currentNode = unVisited.Pop();
                if (currentNode is NewShape shape)
                {
                    farthestX = Math.Max(farthestX, (int)currentNode.GlobalPosition.Y);
                }
                else if (currentNode is Block block)
                {
                    if (block.Separate)
                        continue;
                    foreach (var child in block.Children)
                        unVisited.Push(child);
                }
            }
            //GD.PrintT(Name, "Width called", (farthestX - GlobalPosition.X) / UniversalShapeWidth + 1, farthestX, GlobalPosition);

            return (int)(farthestX - GlobalPosition.Y) / UniversalShapeHeight + 1;
        }
    }
    public List<Node2D> Children => GetChildren().OfType<Node2D>().ToList();
    public virtual void Update() { }
    public void SeparateUpdate()
    {
        int v = 0;
        int prevIfWidth = 0;
        foreach (var (f, index) in Children.WithIndex())
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
                if (block is If || block is While || block is For || block is With || block is Try || block is Finally)
                {
                    block.Position = new Vector2I(UniversalShapeWidth * (prevIfWidth == 0 ? Width - 1 : Width), UniversalShapeHeight * v);
                    block.Update();
                    prevIfWidth = block.Width;
                }
                if (block is Else)
                {
                    var previous = Children[index - 1];
                    if (previous is If || previous is Elif || previous is While || previous is For)
                        block.Position = new Vector2I(UniversalShapeWidth * (Width - prevIfWidth), UniversalShapeHeight * v);
                    else if (previous is Except)
                        block.Position = new Vector2I(UniversalShapeWidth * (Width), UniversalShapeHeight * v);
                    block.Update();
                    prevIfWidth = Math.Max(prevIfWidth, block.Width);
                }
                if (block is Elif || block is Except)
                {
                    block.Position = new Vector2I(UniversalShapeWidth * (Width - prevIfWidth), UniversalShapeHeight * v);
                    block.Update();
                    prevIfWidth = Math.Max(prevIfWidth, block.Width);
                }
                v += block.Height;
            }
        }
        Updated = true;
        QueueRedraw();
    }
    public void RightWardUpdate()
    {
        int i = 0;
        int height = 0;
        int prevI = 0;
        int prevHeight = 0;
        foreach (var (c, index) in Children.WithIndex())
        {
            if (c is NewShape shape)
            {
                height += prevHeight == 0 ? 0 : 1;
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
                    height += block.Height - 1;
                    prevHeight = block.Height;
                }
                if (block is Elif || block is Except)
                {
                    height += 1;
                    block.Position = new Vector2I(UniversalShapeWidth * (prevI), UniversalShapeHeight * height);
                    height += block.Height - 1;
                    prevHeight = block.Height;
                    i = Math.Max(i, prevI + block.Width);
                }
                if (block is Else)
                {
                    height += 1;
                    var previous = Children[index - 1];
                    if (previous is If || previous is Elif || previous is While || previous is For)
                        block.Position = new Vector2I(UniversalShapeWidth * (prevI), UniversalShapeHeight * height);
                    else if (previous is Except)
                    {
                        block.Position = new Vector2I(UniversalShapeWidth * Width, UniversalShapeHeight * height);
                        i = Math.Max(i, prevI + block.Width) + 1;
                    }
                    height += block.Height - 1;
                    prevHeight = block.Height;
                    i = Math.Max(i, prevI + block.Width);
                }
            }
        }
        Updated = true;
        QueueRedraw();
    }
    public virtual Block Init(int leadingSpaces, string name)
    {
        this.LeadingSpaces = leadingSpaces;
        this.Name = name;
        this.Line = name;
        return this;
    }
    public override void _Draw()
    {
        if (!Updated)
            return;
        List<Vector2> positions = new List<Vector2>();
        for (int i = 0; i < Children.Count; i++)
        {
            var output = Children[i];
            var input = i == Children.Count - 1 ? null : Children[i + 1];
            if (output is NewShape shape)
            {
                if (input is null) continue;
                if (shape.line.ContainsBreakers()) continue;
                positions.Add(output.GlobalPosition);
                positions.Add(new Vector2(output.GlobalPosition.X, input.GlobalPosition.Y));
                positions.Add(new Vector2(output.GlobalPosition.X, input.GlobalPosition.Y));
                positions.Add(input.GlobalPosition);
            }
            else if (output is Block outblock)
            {
                if (output is If || output is Elif)
                {
                    if (input is null) continue;
                    positions.Add(output.GlobalPosition);
                    positions.Add(new Vector2(output.GlobalPosition.X, input.GlobalPosition.Y));
                    positions.Add(new Vector2(output.GlobalPosition.X, input.GlobalPosition.Y));
                    positions.Add(input.GlobalPosition);
                    int j = 1;
                    while (input is Elif || input is Else && i + j < Children.Count)
                    {
                        input = Children[i + j];
                        j++;
                    }
                    foreach (var pos in outblock.Output)
                    {
                        positions.Add(pos);
                        positions.Add(new Vector2(input.GlobalPosition.X, pos.Y));
                        positions.Add(new Vector2(input.GlobalPosition.X, pos.Y));
                        positions.Add(input.GlobalPosition);
                    }
                }
                if (output is Else)
                {
                    if (input is null) continue;
                    foreach (var pos in outblock.Output)
                    {
                        positions.Add(pos);
                        positions.Add(new Vector2(input.GlobalPosition.X, pos.Y));
                        positions.Add(new Vector2(input.GlobalPosition.X, pos.Y));
                        positions.Add(input.GlobalPosition);
                    }
                }
                if (output is While || output is For)
                {
                    foreach (var outp in outblock.Output)
                    {
                        if (outp == outblock.GlobalPosition) continue;
                        positions.Add(outp);
                        positions.Add(new Vector2(outblock.Width * UniversalShapeWidth + output.GlobalPosition.X, outp.Y));
                        positions.Add(new Vector2(outblock.Width * UniversalShapeWidth + output.GlobalPosition.X, outp.Y));
                        positions.Add(output.GlobalPosition + new Vector2(outblock.Width * UniversalShapeWidth, -UniversalShapeHeight / 2));
                        positions.Add(output.GlobalPosition + new Vector2(outblock.Width * UniversalShapeWidth, -UniversalShapeHeight / 2));
                        positions.Add(output.GlobalPosition + Vector2.Up * UniversalShapeHeight / 2);
                        positions.Add(output.GlobalPosition + Vector2.Up * UniversalShapeHeight / 2);
                        positions.Add(output.GlobalPosition);
                    }
                    if (input is null) continue;
                    positions.Add(output.GlobalPosition);
                    positions.Add(new Vector2(output.GlobalPosition.X, input.GlobalPosition.Y));
                    positions.Add(new Vector2(output.GlobalPosition.X, input.GlobalPosition.Y));
                    positions.Add(input.GlobalPosition);
                }
            }
        }
        if (positions.Count < 2)
            return;
        DrawMultiline(positions.Select(x => ToLocal(x)).ToArray(), Colors.Red, 3);
    }

}