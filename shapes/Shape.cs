using System.Collections.Generic;
using Godot;
namespace Flowcharter.shapes
{
    public partial class Shape : Sprite2D
    {
        public enum Shapes
        {
            //process/operation symbols
            PROCESS,
            //Branching and control flow
            TERMINATOR, DECISION
        }
        public Shapes shape;
        string line = "";
        public List<Shape> Inputs = new List<Shape>();
        public List<Shape> Outputs = new List<Shape>();
        public Shape Init(string line, string name,string type = "")
        {
            if (type == "def")
            {
                shape = Shapes.TERMINATOR;
            }
            else if (type == "if")
            {
                shape = Shapes.DECISION;
            }
            else
            {
                shape = Shapes.PROCESS;
            }
            this.line = line;
            this.Name = name;
            return this;
            
        }
        public override void _Ready()
        {
            Texture = GD.Load<Texture2D>($"res://shapes/images/{shape}.svg");
            GetNode<Label>("Label").Text = line.Trim();
        }
        
    }
}