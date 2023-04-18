using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter.blocks;
public partial class Block : Node2D
{
    public bool Seperate;
    public int LeadingSpaces;
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
                    if (block.Seperate)
                        continue;
                    foreach (var child in block.Children)
                    {
                        unVisited.Push(child);
                    }
                }
            }
            return (int)(farthestX - GlobalPosition.X) / 128 + 1;
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
                    if (block.Seperate)
                        continue;
                    foreach (var child in block.Children)
                        unVisited.Push(child);
                }
            }
            //GD.PrintT(Name, "Width called", (farthestX - GlobalPosition.X) / 128 + 1, farthestX, GlobalPosition);

            return (int)(farthestX - GlobalPosition.Y) / 96 + 1;
        }
    }
    public List<Node2D> Children => GetChildren().OfType<Node2D>().ToList();
    public virtual void Update() { }
    public virtual Block Init(int leadingSpaces, string name)
    {
        this.LeadingSpaces = leadingSpaces;
        this.Name = name;
        return this;
    }

}