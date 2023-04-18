using Godot;
using Flowcharter.shapes;
using System.Linq;
using System;
using System.Collections.Generic;
namespace Flowcharter.flowcharter;
public partial class NewBlock : Node2D
{
    public NewBlock RootBlock;
    public BlockType Type = BlockType.DEF;
    public int LeadingSpaces = 0;
    public int Height
    {
        get
        {
            int farthestY = 0;
            Stack<Node2D> unVisited = new Stack<Node2D>();
            unVisited.Push(this);
            while (unVisited.Count != 0)
            {
                var current = unVisited.Pop();
                var children = current.GetChildren();
                foreach (var child in children)
                {
                    if (child is Shape shape)
                        farthestY = Math.Max(farthestY, (int)shape.GlobalPosition.Y);
                    if (child is NewBlock block)
                    {
                        if (block.Seperate)
                            continue;
                        unVisited.Push(block);
                    }
                }
            }
            return (int)(farthestY - GlobalPosition.Y) / 96 + 1;
        }
    }
    public int Width
    {
        get
        {
            int farthestX = 0;
            Node2D currentNode = this;
            Stack<Node2D> unVisited = new Stack<Node2D>();
            unVisited.Push(currentNode);
            while (unVisited.Count != 0)
            {
                var current = unVisited.Pop();
                var children = current.GetChildren();
                foreach (var child in children)
                {
                    if (child is Shape shape)
                    {
                        //GD.PrintT(Name, shape.GlobalPosition.X, farthestX, shape.Name);
                        if (shape.GlobalPosition.X > farthestX)
                            farthestX = (int)shape.GlobalPosition.X;
                    }
                    if (child is NewBlock block)
                    {
                        if (block.Seperate)
                            continue;
                        unVisited.Push(block);
                    }
                }
            }
            //GD.PrintT(Name, "Width called", (farthestX - GlobalPosition.X) / 128 + 1, farthestX, GlobalPosition);
            return (int)(farthestX - GlobalPosition.X) / 128 + 1;
        }
    }

    public bool Seperate = true;
    public List<NewBlock> InsideBlocks = new List<NewBlock>();
    public List<NewBlock> SeperateBlocks = new List<NewBlock>();
    public List<Node2D> Children
    {
        get
        {
            return GetChildren().Select(x => x as Node2D).ToList();
        }
    }
    public NewBlock Init(int leadingSpaces, string name, BlockType type)
    {
        this.LeadingSpaces = leadingSpaces;
        this.Name = name;
        this.Type = type;
        if (type == BlockType.DEF)
        {
            Seperate = true;
        }
        else
            Seperate = false;
        return this;
    }

    //for functions only
    public void UpdateSeperate()
    {
        int v = 0;
        int savedW = 0;
        int prevW = 0;
        foreach (var f in Children)
        {
            if (f is Shape shape)
            {
                shape.Position = new Vector2I(128 * savedW, 96 * v);
                prevW = 0;
                v++;
            }
            if (f is NewBlock block)
            {
                if (block.Seperate)
                    return;
                if (block.Type == BlockType.WHILE)
                    v++;
                block.Position = new Vector2I(128 * (savedW-prevW), 96 * v);
                block.Update();
                savedW = Width;
                prevW = block.Width;
                v += block.Height;
            }
        }
    }
    public void Update()
    {
        int h = 0;
        int previousWidth = 0;
        foreach (var f in Children)
        {
            //if the child is a shape
            if (f is Shape shape)
            {
                if (Type == BlockType.IF)
                    shape.Position = new Vector2I(128 * h, 96 * (Height - 1));
                if (Type == BlockType.ELSE)
                    shape.Position = new Vector2(128 * (Width - 1), 96 * h);
                h++;
                previousWidth = 1;
            }
            //if the child is a newblock
            if (f is NewBlock block)
            {
                if (block.Seperate)
                    return;
                block.Update();

                //when the parent is an if
                if (Type == BlockType.IF)
                {
                    if (block.Type == BlockType.IF)
                    {
                        block.Position = new Vector2I(128 * h, 0);
                        h += block.Width;
                    }
                    if (block.Type == BlockType.ELSE)
                    {
                        block.Position = new Vector2I(128 * (h - previousWidth), 96);
                        h += -previousWidth + block.Width;
                    }
                    previousWidth = block.Width;
                }
                //when the paret is an else
                if (Type == BlockType.ELSE)
                {
                    block.Position = new Vector2I(0, 96 * h);
                    h += block.Height;
                }
            }
        }
    }




}