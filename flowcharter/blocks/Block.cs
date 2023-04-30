namespace Flowcharter.flowcharter.blocks;

using Flowcharter.shapes;

public partial class Block : Node2D
{
    public int UniversalShapeWidth = 136;
    public int UniversalShapeHeight = 102;
    public bool Separate;
    public int LeadingSpaces;
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
                    var previous = Children[index-1];
                    if (previous is If || previous is Elif)
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
    }
    public void RightWardUpdate()
    {
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
    }
    public virtual Block Init(int leadingSpaces, string name)
    {
        this.LeadingSpaces = leadingSpaces;
        this.Name = name;
        this.Line = name;
        return this;
    }

}