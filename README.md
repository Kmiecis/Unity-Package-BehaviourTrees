# Behaviour Trees

## Description

Simple and lightweight behaviour trees implementation that can be written entirely in code or entirely in the Inspector.
On the surface, it closely resembles Unreal Engine Behaviour Trees implementation, but under the hood it was written with simplicity and extendability in mind.

## Installation

Add the package via Package Manager by adding it from git URL:  
`https://github.com/Kmiecis/Unity-Package-BehaviourTrees.git`  
Package Manager can be found inside the Unity Editor in the Window tab

OR

Git add this repository as a submodule inside your Unity project Assets folder:  
`git submodule add https://github.com/Kmiecis/Unity-Package-BehaviourTrees`

## Examples

<details>
<summary>Cooldown</summary>
<p>

#### Cooldown example.
A tree that changes _color field to a random of three options and three wait values between cooldowns.

```cs
public class CustomTask : BT_ITask
{
    private Action _action;

    public CustomTask(Action action)
    {
        _action = action;
    }

    public BT_EStatus Update()
    {
        _action();
        return BT_EStatus.Success;
    }

    public void Abort()
    {
        // no-op
    }
}

private Color _color;

public BT_INode CreateRoot()
{
    return new BT_RootNode().AddChildren(
        new BT_RandomNode().AddChildren(
            new CustomTask(delegate { _color = Color.Red; }),
            new BT_Wait(1.0f),
            new CustomTask(delegate { _color = Color.Green; }),
            new BT_Wait(1.0f),
            new CustomTask(delegate { _color = Color.Blue; }),
            new BT_Wait(1.0f)
        ).AddConditionals(
            new BT_Cooldown(1.0f)
        )
    );
}
```

</p>
</details>

<details>
<summary>Repeat</summary>
<p>

#### Repeats example.
A tree does in sequence:
1. Changes _color field to a random of three options each frame for 3 seconds.
2. Changes _color field sequentially between three values each second 2 times.

```cs
public interface IColorContext
{
    Color Color { set; }
}

public class ChangeColorTask : BT_ATask
{
    private IColorContext _context;
    private Color _color;

    public ChangeColorTask(IColorContext context, Color color)
    {
        _context = context;
        _color = color;
    }

    protected override BT_EStatus OnUpdate()
    {
        _context.Color = _color;
        return BT_EStatus.Success;
    }
}

private IColorContext _context;

private BT_ITask CreateRoot()
{
    return new BT_RootNode().AddChildren(
        new BT_SequenceNode().AddChildren(
            new BT_RandomNode().AddChildren(
                new ChangeColorTask(_context, Color.red),
                new ChangeColorTask(_context, Color.green),
                new ChangeColorTask(_context, Color.blue)
            ).AddDecorators(
                new BT_RepeatFor(3.0f)
            ),
            new BT_SequenceNode().AddChildren(
                new ChangeColorTask(_context, Color.red),
                new BT_Wait(1.0f),
                new ChangeColorTask(_context, Color.green),
                new BT_Wait(1.0f),
                new ChangeColorTask(_context, Color.blue),
                new BT_Wait(1.0f)
            ).AddDecorators(
                new BT_Repeat(2)
            )
        )
    );
}
```

</p>
</details>
