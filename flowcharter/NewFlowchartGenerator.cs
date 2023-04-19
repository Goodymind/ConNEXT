using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Flowcharter.shapes;
namespace Flowcharter.flowcharter;
public partial class NewFlowchartGenerator : Node2D
{
    NewBlock mostRecentParent;
    List<string> file = new List<string>();
    public static PackedScene shapeScene = GD.Load<PackedScene>("res://shapes/shape.tscn");
    public static PackedScene newBlock = GD.Load<PackedScene>("res://flowcharter/newBlock.tscn");
    List<NewBlock> Functions = new List<NewBlock>();
    EnvironmentUI UI;
    public override void _Ready()
    {
        UI = GetNode<EnvironmentUI>("CanvasLayer/Control");
        Start();
    }
    public void Start()
    {
        GD.PrintT("Test");
        Read(@"flowcharter\test.py");
        var parent = newBlock.Instantiate<NewBlock>().Init(0, "MAIN", BlockType.DEF);
        Functions.Add(parent);
        //UI.Update(Functions);
        mostRecentParent = parent;
        AddChild(parent);
        parent.AddChild(shapeScene.Instantiate<Shape>().Init("MAIN", "MAIN", "def"));
        for (int i = 0; i < file.Count; i++)
        {
            if (String.IsNullOrWhiteSpace(file[i]) || String.IsNullOrEmpty(file[i]))
                continue;
            Analyze(file[i], i);
        }
        GD.Print(String.Join('\t', Functions.Select(x => x.Name)));
        Reparenting();
    }
    private void Analyze(string line, int i = 0)
    {
        int leadingSpaces = line.LeadingSpaces();
        string trim = line.Trim();
        while (leadingSpaces < mostRecentParent.LeadingSpaces)
        {
            mostRecentParent = mostRecentParent.GetParent<NewBlock>();
        }
        if (trim.Substring(0, 4) == "def ")
        {
            var newParent = newBlock.Instantiate<NewBlock>().Init(file[i + 1].LeadingSpaces(), trim, BlockType.DEF);
            mostRecentParent.AddChild(newParent);
            mostRecentParent = newParent;
            mostRecentParent.AddChild(shapeScene.Instantiate<Shape>().Init(trim, trim, "def"));
            Functions.Add(newParent);
            //UI.Update(Functions);
            return;
        }
        if (trim.Substring(0, 3) == "if ")
        {
            var newif = newBlock.Instantiate<NewBlock>().Init(file[i + 1].LeadingSpaces(), trim, BlockType.IF);
            mostRecentParent.AddChild(newif);
            mostRecentParent = newif;
            mostRecentParent.AddChild(shapeScene.Instantiate<Shape>().Init(trim, trim, "if"));
            return;
        }
        if (trim.Length >= 5)
        {
            if (trim.Replace(" ", "").Substring(0, 5) == "else:")
            {
                var newif = newBlock.Instantiate<NewBlock>().Init(file[i + 1].LeadingSpaces(), trim, BlockType.ELSE);
                mostRecentParent.AddChild(newif);
                mostRecentParent = newif;
                return;
            }
        
            if (trim.Substring(0, 5) == "elif ")
            {
                var newif = newBlock.Instantiate<NewBlock>().Init(file[i + 1].LeadingSpaces(), trim, BlockType.IF);
                mostRecentParent.AddChild(newif);
                mostRecentParent = newif;
                mostRecentParent.AddChild(shapeScene.Instantiate<Shape>().Init(trim, trim, "if"));
                GD.Print("hey");
                return;
            }
        }
        if (trim.Length >= 6)
            if (trim.Substring(0,6) == "while ")
            {
                var newWhile = newBlock.Instantiate<NewBlock>().Init(file[i+1].LeadingSpaces(), trim, BlockType.WHILE);
                mostRecentParent.AddChild(newWhile);
                mostRecentParent = newWhile;
                mostRecentParent.AddChild(shapeScene.Instantiate<Shape>().Init(trim, trim, "if"));
            }

        mostRecentParent.AddChild(shapeScene.Instantiate<Shape>().Init(trim, trim));


    }
    private void Read(string path)
    {
        file = File.ReadAllLines(path).ToList<string>();
    }
    private void Reparenting()
    {
        foreach (var f in Functions)
        {
            f.Reparent(GetNode("Functions"), false);
        }
        foreach (var f in Functions)
        {
            f.UpdateSeparate();
        }
    }

}