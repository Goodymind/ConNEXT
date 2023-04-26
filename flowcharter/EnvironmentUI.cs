using System.Collections.Generic;
using Godot;
using Flowcharter.flowcharter.blocks;
namespace Flowcharter.flowcharter;
public partial class EnvironmentUI : Control
{
    ItemList List;
    ItemList ClassList;
    List<Function> Functions;
    List<Class> Classes;
    public override void _Ready()
    {
        List = GetNode<ItemList>("VBoxContainer/ItemList");
        ClassList = GetNode<ItemList>("VBoxContainer/ClassList");
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
    public void ItemListSelected(int index)
    {
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

    }
}