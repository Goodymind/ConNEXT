using System.Collections.Generic;
using Godot;
using Flowcharter.flowcharter.blocks;
namespace Flowcharter.flowcharter;
public partial class EnvironmentUI : Control
{
    [Signal] public delegate void FileSelectedSignalEventHandler(string path);
    [Signal] public delegate void ItemSelectedEventHandler();
    ItemList List;
    ItemList ClassList;
    List<Function> Functions;
    List<Class> Classes;
    Vector2 previousMousePos = Vector2.Zero;
    AnimationPlayer animPlayer;
    bool show;
    bool ShowUI
    {
        get {return show;}
        set 
        {
            if (value != show)
            {
                animPlayer.Play(value ? "slide_in": "slide_out");
                show = value;
            }
        }
    }
    public override void _Ready()
    {
        List = GetNode<ItemList>("VBoxContainer/ItemList");
        ClassList = GetNode<ItemList>("VBoxContainer/ClassList");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("slide_out");
    }
    /*
    public void Update(List<NewBlock> function)
    {
        Functions = function;
        List.Clear();
        foreach (var item in function)
        {
            List.AddItem(item.Name);
            item.Visible = false;
        }
        Functions[0].Visible = true;
    }
    */
    public void Update(List<Function> function, List<Class> classes)
    {
        Functions = function;
        Classes = classes;
        List.Clear();
        foreach (var item in function)
        {
            List.AddItem(item.Name);
            item.Visible = false;
        }
        ClassList.Clear();
        foreach (var _class in classes)
        {
            ClassList.AddItem(_class.Name);
            _class.Visible = false;
        }
        Functions[0].Visible = true;
    }
    public void ItemListSelected(int index, Vector2 position, int mouse_index)
    {
        if (mouse_index != (int)MouseButton.Left) return;
        if (Functions is null)
            return;
        foreach (var f in Functions)
        {
            f.Visible = false;
            if (f == Functions[index])
            {
                f.Visible = true;
            }
        }
        foreach (var c in Classes)
            c.Visible = false;
        EmitSignal(SignalName.ItemSelected);
    }
    public void ClassItemListSelected(int index, Vector2 position,int mouse_index)
    {
        if (Classes is null)
            return;
        foreach (var f in Classes)
        {
            f.Visible = false;
            if (f == Classes[index])
            {
                f.Visible = true;
            }
        }
        foreach (var f in Functions)
            f.Visible = false;
        EmitSignal(SignalName.ItemSelected);
    }
    private void SelectFile()
    {
        GetNode("FileSelecter").Call("select_file");
    }
    private void FileSelected(string path)
    {
        EmitSignal(SignalName.FileSelectedSignal, path);
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            ShowUI = mouseMotion.GlobalPosition.X < 350;
        }
    }

}