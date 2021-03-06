# Common Behaviour Trees

## Description

Code-only simple and lightweight behaviour trees implementation.
On the surface, it closely resembles UE Behaviour Trees implementation, but under the hood it was written with simplicity and extendability in mind.

## Examples

<details>
<summary>Cooldown</summary>
<p>

#### Cooldown example. A tree that changes _color field to a random of three options between cooldowns.

```cs
return new BT_TreeNode()
{
    Task = new BT_RandomNode()
    {
        Conditional = new BT_Cooldown(2.0f),
        Tasks = new BT_ITask[]
        {
            new BT_DelegateTask() { OnStartAction = delegate { _color = Color.red; } },
            new BT_DelegateTask() { OnStartAction = delegate { _color = Color.green; } },
            new BT_DelegateTask() { OnStartAction = delegate { _color = Color.blue; } },
        }
    }
};
```

</p>
</details>

<details>
<summary>Wait and Limit</summary>
<p>

#### Wait and Limit example. A tree that changes _color field sequentially between three values each second and halts midway last awaiter.

```cs
return new BT_TreeNode()
{
    Task = new BT_SequenceNode()
    {
        Conditional = new BT_Limit(2.5f),
        Tasks = new BT_ITask[]
        {
            new BT_DelegateTask() { OnStartAction = delegate { _color = Color.red; } },
            new BT_Wait(1.0f),
            new BT_DelegateTask() { OnStartAction = delegate { _color = Color.green; } },
            new BT_Wait(1.0f),
            new BT_DelegateTask() { OnStartAction = delegate { _color = Color.blue; } },
            new BT_Wait(1.0f),
        }
    }
};
```

</p>
</details>

<details>
<summary>Custom task with context and Repeats</summary>
<p>

#### Repeats example with custom contextual task. A tree does in sequence:
1. Changes _color field to a random of three options each frame for 3 seconds.
2. Changes _color field sequentially between three values each second 2 times
3. Changes _color field to a random of three options each frame for 120 frames.

```cs
private class ColorContext
{
    public Color color;
}

private class ChangeColorTask : BT_ATask<ColorContext>
{
    private readonly Color _color;

    public ChangeColorTask(ColorContext context, Color color) :
        base(context)
    {
        _color = color;
    }

    protected override BT_EStatus OnExecute()
    {
        _context.color = _color;
        return BT_EStatus.Success;
    }
}

private ColorContext _colorContext = new ColorContext();

private BT_ITask CreateBehaviourTree()
{
    return new BT_TreeNode()
    {
        Task = new BT_SequenceNode()
        {
            Tasks = new BT_ITask[]
            {
                new BT_RandomNode()
                {
                    Tasks = new BT_ITask[]
                    {
                        new ChangeColorTask(_colorContext, Color.red),
                        new ChangeColorTask(_colorContext, Color.green),
                        new ChangeColorTask(_colorContext, Color.blue),
                    },
                    Decorator = new BT_RepeatFor(3.0f)
                },
                new BT_SequenceNode()
                {
                    Tasks = new BT_ITask[]
                    {
                        new ChangeColorTask(_colorContext, Color.red),
                        new BT_Wait(1.0f),
                        new ChangeColorTask(_colorContext, Color.green),
                        new BT_Wait(1.0f),
                        new ChangeColorTask(_colorContext, Color.blue),
                        new BT_Wait(1.0f),
                    },
                    Decorator = new BT_Repeat(2)
                },
                new BT_RandomNode()
                {
                    Tasks = new BT_ITask[]
                    {
                        new ChangeColorTask(_colorContext, Color.red),
                        new ChangeColorTask(_colorContext, Color.green),
                        new ChangeColorTask(_colorContext, Color.blue),
                    },
                    Decorator = new BT_RepeatForFrames(120)
                },
            }
        }
    };
}
```

</p>
</details>
